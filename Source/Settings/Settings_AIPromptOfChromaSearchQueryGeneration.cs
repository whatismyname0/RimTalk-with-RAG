using RimTalk.Data;
using UnityEngine;
using Verse;

namespace RimTalk;

public partial class Settings
{
    private void DrawAIPromptOfChromaSearchQueryGenerationSettings(Listing_Standard listingStandard)
    {
        RimTalkSettings settings = Get();

        _textAreaBuffer = string.IsNullOrWhiteSpace(settings.CustomAIPromptOfChromaSearchQueryGeneration) 
            ? Constant.DefaultAIPromptOfChromaSearchQueryGeneration 
            : settings.CustomAIPromptOfChromaSearchQueryGeneration;

        var activeConfig = settings.GetActiveConfig();
        var modelName = activeConfig?.SelectedModel ?? "N/A";

        var AIPromptOfChromaSearchQueryGeneration = "RimTalk.Settings.AIPromptOfChromaSearchQueryGenerationPrompt".Translate(modelName);
        var AIPromptOfChromaSearchQueryGenerationPromptRect = listingStandard.GetRect(Text.CalcHeight(AIPromptOfChromaSearchQueryGeneration, listingStandard.ColumnWidth));
        Widgets.Label(AIPromptOfChromaSearchQueryGenerationPromptRect, AIPromptOfChromaSearchQueryGeneration);
        listingStandard.Gap(6f);

        // Use a fixed height for the text area
        float textAreaHeight = 350f;
        Rect textAreaRect = listingStandard.GetRect(textAreaHeight);
            
        // Draw the text area - Unity's TextArea handles its own scrolling internally
        string newAIPromptOfChromaSearchQueryGeneration = Widgets.TextArea(textAreaRect, _textAreaBuffer);

        // Update buffer and settings logic
        if (newAIPromptOfChromaSearchQueryGeneration != _textAreaBuffer)
        {
            string processedAIPromptOfChromaSearchQueryGeneration = newAIPromptOfChromaSearchQueryGeneration.Replace("\\n", "\n");

            _textAreaBuffer = processedAIPromptOfChromaSearchQueryGeneration;
            if (processedAIPromptOfChromaSearchQueryGeneration == Constant.DefaultAIPromptOfChromaSearchQueryGeneration)
            {
                settings.CustomAIPromptOfChromaSearchQueryGeneration = "";
            }
            else
            {
                settings.CustomAIPromptOfChromaSearchQueryGeneration = processedAIPromptOfChromaSearchQueryGeneration;
            }
        }

        listingStandard.Gap(6f);

        // Reset to default button
        Rect resetButtonRect = listingStandard.GetRect(30f);
        if (Widgets.ButtonText(resetButtonRect, "RimTalk.Settings.ResetToDefault".Translate()))
        {
            settings.CustomAIPromptOfChromaSearchQueryGeneration = "";
            _textAreaBuffer = Constant.DefaultAIPromptOfChromaSearchQueryGeneration;
        }

        // Add some extra space at the bottom to ensure everything is visible
        listingStandard.Gap(10f);
    }
}