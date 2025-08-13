using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CortexControl
{
    public static class CC_RecipeUtils
    {
        public static void BrainwashPawn(Pawn patient, Pawn surgeon)
        {
            if (surgeon == null) return;
            RecruitPawn(patient, surgeon);
            ConvertPawn(patient, surgeon);
        }
        private static void RecruitPawn(Pawn patient, Pawn surgeon)
        {
            if (patient.Faction == surgeon.Faction) return;
            Log.Message("Faction set for patient: " + patient.Name + " to " + surgeon.Faction);
            patient.guest?.SetGuestStatus(null);
            patient.guest?.Notify_PawnRecruited();
            patient.SetFaction(surgeon.Faction, surgeon);
            InteractionWorker_RecruitAttempt.DoRecruit(surgeon, patient);
        }

        private static void ConvertPawn(Pawn patient, Pawn surgeon)
        {
            if (!ModLister.IdeologyInstalled) return;
            Log.Message("Ideology set for patient: " + patient.Name + " to " + surgeon.Ideo);
            patient.ideo.SetIdeo(surgeon.Ideo);
            patient.ideo.OffsetCertainty(100);
        }
    }
}