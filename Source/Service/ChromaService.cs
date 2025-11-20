using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimTalk.Data;
using RimTalk.Util;
using Verse;

namespace RimTalk.Service;

/// <summary>
/// Service for managing ChromaDB integration - storing conversations and retrieving historical context.
/// Handles asynchronous communication with the Python ChromaManager via stdio/IPC.
/// </summary>
public static class ChromaService
{
    private static string _currentSaveId = "";
    private static ChromaClient _client;
    private static bool _initialized = false;

    /// <summary>
    /// Initialize ChromaService for a specific save.
    /// Must be called when loading/creating a save.
    /// </summary>
    public static void InitializeForSave(string saveId)
    {
        if (_initialized && _currentSaveId == saveId)
            return;

        try
        {
            _currentSaveId = saveId;
            _client = new ChromaClient();
            
            try
            {
                _client.Initialize(saveId);
                _initialized = true;
                _client.updateBackground(_currentSaveId,Constant.Context);
                Logger.Message($"[ChromaService] Initialized for save: {saveId}");
            }
            catch (Exception initEx)
            {
                Logger.Error($"[ChromaService] Failed to initialize ChromaClient: {initEx.Message}");
                Logger.Error($"[ChromaService] ChromaDB features will be disabled for this session");
                _initialized = false;
                _client = null;
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"[ChromaService] Failed to initialize: {ex.Message}");
            _initialized = false;
        }
    }

    /// <summary>
    /// Close the current save's ChromaDB connection.
    /// Call when unloading a save.
    /// </summary>
    public static void CloseSave()
    {
        if (_client != null)
        {
            _client?.Close(_currentSaveId);
            _client = null;
        }
        _initialized = false;
        _currentSaveId = null;
    }

    public static void UpdateBackground(
        string backgrounds
    )
    {
        if (!_initialized || _client == null)
            return;

        Task.Run(() =>
        {
            try
            {
                _client.updateBackground(
                    _currentSaveId,
                    backgrounds
                );
            }
            catch (Exception ex)
            {
                Logger.Error($"[ChromaService] Error updating background: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Asynchronously store a conversation turn in ChromaDB.
    /// </summary>
    public static void StoreConversationAsync(
        List<TalkResponse> responses,
        Pawn initiator,
        List<Pawn> allInvolvedPawns,
        string gameDate)
    {
        if (!_initialized || _client == null)
            return;

        Task.Run(() =>
        {
            try
            {
                var speakers = new List<string> { initiator.LabelShort };
                var listeners = allInvolvedPawns.Select(p => p.LabelShort).ToList();

                _client.AddConversation(
                    _currentSaveId,
                    responses,
                    speakers,
                    listeners,
                    gameDate
                );
            }
            catch (Exception ex)
            {
                Logger.Error($"[ChromaService] Error storing conversation: {ex.Message}");
            }
        });
    }

    // <summary>
    /// Asynchronously query relevant historical context.
    /// </summary>
    public static async Task<List<ContextEntry>> QueryRelevantContextAsync(
        List<string> searchQueries, // CHANGED: string prompt -> List<string> searchQueries
        List<Pawn> relevantPawns,
        int maxResults = 5)
    {
        if (!_initialized || _client == null)
        {
            // ... existing null checks ...
             return new List<ContextEntry>();
        }

        try
        {
            // Fallback if list is empty
            if (searchQueries == null || searchQueries.Count == 0)
                searchQueries = new List<string> { "conversation" };

            Logger.Debug($"[ChromaService] Starting query for save: {_currentSaveId}");
            Logger.Debug($"[ChromaService] Queries: {string.Join(" | ", searchQueries)}");

            var listeners = relevantPawns.Select(p => p.LabelShort).ToList();
            
            var results = await Task.Run(() =>
                _client.QueryContext(
                    _currentSaveId,
                    searchQueries, // Pass the list
                    listeners,
                    10 // query more to filter locally if needed, or let python handle it
                )
            );

            Logger.Debug($"[ChromaService] Query completed successfully");
            Logger.Debug($"[ChromaService] Returned {results.Count} results");

            if (results.Count > 0)
            {
                Logger.Debug($"[ChromaService] Results details:");
                for (int i = 0; i < results.Count; i++)
                {
                    Logger.Debug($"[ChromaService]   Result {i + 1}: Speaker={results[i].Speaker}, " +
                        $"Relevance={results[i].Relevance:F4}, Type={results[i].TalkType}");
                    Logger.Debug($"[ChromaService]   {results[i].Text}");
                }
            }
            return results.Take(maxResults).ToList();
        }
        catch (Exception ex)
        {
            Logger.Error($"[ChromaService] Error querying context: {ex.Message}");
            Logger.Error($"[ChromaService] Exception type: {ex.GetType().Name}");
            Logger.Error($"[ChromaService] Stack trace: {ex.StackTrace}");
            return new List<ContextEntry>();
        }
    }

    /// <summary>
    /// Get a summary of relevant historical context for inclusion in the prompt.
    /// </summary>
    public static string FormatContextForPrompt(List<ContextEntry> contextEntries)
    {
        if (!contextEntries.Any())
            return "";

        var sb = new StringBuilder();
        sb.AppendLine("\n[关键信息]");

        foreach (var entry in contextEntries)
            if (entry.Relevance >= .72f)
            {
                sb.AppendLine($"说话人:{entry.Speaker} (日期:{entry.Date}): {entry.Text}");
                if (entry.Listeners.Any())
                {
                    sb.AppendLine($"  → 听众: {string.Join(", ", entry.Listeners)}");
                }
            }

        sb.AppendLine("[关键信息结束]");
        sb.AppendLine("[其它相关信息]");

        foreach (var entry in contextEntries)
            if (entry.Relevance < .72f && entry.Relevance >= .6f)
            {
                sb.AppendLine($"{entry.Speaker} ({entry.Date}): {entry.Text}");
                if (entry.Listeners.Any())
                {
                    sb.AppendLine($"  → 听众: {string.Join(", ", entry.Listeners)}");
                }
            }

        sb.AppendLine("[背景信息结束]\n");
        return sb.ToString();
    }
}

/// <summary>
/// Data class for historical context entries from ChromaDB.
/// </summary>
public class ContextEntry
{
    public string Text { get; set; }
    public string Speaker { get; set; }
    public List<string> Listeners { get; set; } = new List<string>();
    public string Date { get; set; }
    public string TalkType { get; set; }
    public float Relevance { get; set; } // Distance score (lower = more relevant)
}
