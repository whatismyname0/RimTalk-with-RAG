using RimTalk.Data;
using UnityEngine;
using Verse;

namespace RimTalk;

public partial class Settings
{
    private void DrawAIContextSettings(Listing_Standard listingStandard)
    {
        RimTalkSettings settings = Get();

        _textAreaBuffer = string.IsNullOrWhiteSpace(settings.CustomContext) 
            ? Constant.DefaultContext 
            : settings.CustomContext;

        var activeConfig = settings.GetActiveConfig();
        var modelName = activeConfig?.SelectedModel ?? "N/A";

        var aiContext = "RimTalk.Settings.AIContextPrompt".Translate(modelName);
        var aiContextPromptRect = listingStandard.GetRect(Text.CalcHeight(aiContext, listingStandard.ColumnWidth));
        Widgets.Label(aiContextPromptRect, aiContext);
        listingStandard.Gap(6f);

        // Use a fixed height for the text area
        float textAreaHeight = 350f;
        Rect textAreaRect = listingStandard.GetRect(textAreaHeight);
            
        // Draw the text area - Unity's TextArea handles its own scrolling internally
        string newContext = Widgets.TextArea(textAreaRect, _textAreaBuffer);

        // Update buffer and settings logic
        if (newContext != _textAreaBuffer)
        {
            string processedContext = newContext.Replace("\\n", "\n");

            _textAreaBuffer = processedContext;
            if (processedContext == Constant.DefaultContext)
            {
                settings.CustomContext = "";
            }
            else
            {
                settings.CustomContext = processedContext;
            }
        }

        listingStandard.Gap(6f);

        // Reset to default button
        Rect resetButtonRect = listingStandard.GetRect(30f);
        if (Widgets.ButtonText(resetButtonRect, "RimTalk.Settings.ResetToDefault".Translate()))
        {
            settings.CustomContext = "";
            _textAreaBuffer = Constant.DefaultContext;
        }

        // Add some extra space at the bottom to ensure everything is visible
        listingStandard.Gap(10f);
    }
}