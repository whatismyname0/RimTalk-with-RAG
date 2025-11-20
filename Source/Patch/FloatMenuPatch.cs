using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimTalk.Service;
using RimTalk.UI;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimTalk.Patch;

[HarmonyPatch(typeof(FloatMenuMakerMap), nameof(FloatMenuMakerMap.GetOptions))]
public static class FloatMenuPatch
{
    private const float ClickRadius = 1.2f; // Radius in cells to check around click position
   
    public static void Postfix(List<Pawn> selectedPawns, Vector3 clickPos, FloatMenuContext context,
        ref List<FloatMenuOption> __result)
    {
        if (selectedPawns is not { Count: 1 }) return;

        Pawn pawn = selectedPawns[0];
        if (!Settings.Get().AllowCustomConversation) return;
        if (pawn == null || pawn.Drafted) return;
        
        IntVec3 cell = IntVec3.FromVector3(clickPos);

        // Check for other pawns in a radius around click position
        List<Thing> thingsInRadius = GenRadial.RadialDistinctThingsAround(cell, pawn.Map, ClickRadius, true).ToList();
        
        foreach (Thing thing in thingsInRadius)
        {
            if (thing is Pawn targetPawn && 
                (targetPawn.RaceProps.Humanlike||targetPawn.RaceProps.ToolUser))
            {
                if (pawn.IsTalkEligible() && (pawn == targetPawn || pawn.CanReach(targetPawn, PathEndMode.Touch, Danger.None)))
                {
                    AddTalkOption(__result, pawn, targetPawn);
                }
                break;
            }
        }
    }

    private static void AddTalkOption(List<FloatMenuOption> result, Pawn initiator, Pawn target)
    {
        Pawn localInitiator = initiator;
        Pawn localTarget = target;
        
        result.Add(new FloatMenuOption(
            "RimTalk.FloatMenu.ChatWith".Translate(target.LabelShortCap),
            delegate 
            { 
                Find.WindowStack.Add(new CustomDialogueWindow(localInitiator, localTarget)); 
            },
            MenuOptionPriority.Default,
            null,
            target
        ));
    }
}