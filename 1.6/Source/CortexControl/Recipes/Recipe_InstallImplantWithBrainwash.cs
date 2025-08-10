using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CortexControl
{
    public class Recipe_InstallImplantWithBrainwash : Recipe_InstallImplantCC
    {
        public override void ApplyOnPawn(
            Pawn patient,
            BodyPartRecord part,
            Pawn surgeon,
            List<Thing> ingredients,
            Bill bill)
        {
            base.ApplyOnPawn(patient, part, surgeon, ingredients, bill);
            if (patient.Dead) return;
            ConvertPawn(patient, surgeon); 
            RecruitPawn(patient, surgeon);
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