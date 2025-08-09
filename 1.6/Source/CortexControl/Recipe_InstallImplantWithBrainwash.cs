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
            if (!patient.Dead)
            {
                ConvertPawn(patient, surgeon); 
                RecruitPawn(patient, surgeon);
            }
        }
        
        private void RecruitPawn(Pawn patient, Pawn surgeon)
        {
            Log.Message("Faction set for patient: " + patient.Name + " to " + surgeon.Faction);
            if (patient.guest != null)
            {
                patient.guest.SetGuestStatus(null);
            }
            if (patient.Faction != surgeon.Faction)
            {
                patient.SetFaction(surgeon.Faction, surgeon);
            }
            if (patient.guest != null)
            {
                patient.guest.Notify_PawnRecruited();
            }
            InteractionWorker_RecruitAttempt.DoRecruit(surgeon, patient);
        }

        private void ConvertPawn(Pawn patient, Pawn surgeon)
        {
            if (ModLister.IdeologyInstalled)
            {
                Log.Message("Ideology set for patient: " + patient.Name + " to " + surgeon.Ideo);
                patient.ideo.SetIdeo(surgeon.Ideo);
                patient.ideo.OffsetCertainty(100);
            }
        }
    }
}