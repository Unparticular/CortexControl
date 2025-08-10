using System.Collections.Generic;
using Verse;
using RimWorld;

namespace CortexControl
{
    public class Recipe_InstallImplantCC : Recipe_InstallImplant
    {
        public override void ApplyOnPawn(
            Pawn patient,
            BodyPartRecord part,
            Pawn surgeon,
            List<Thing> ingredients,
            Bill bill)
        {
            if (surgeon != null)
            {
                var surgeryFailed = CheckSurgeryFail(surgeon, patient, ingredients, part, bill);
                IsCCInstallViolation(patient, part, surgeon, surgeryFailed);
                if (surgeryFailed) return;
                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, (object) surgeon, (object) patient);
            }
            TryRemoveHediff(patient);
            patient.health.AddHediff(recipe.addsHediff, part);
        }

        private void IsCCInstallViolation(Pawn patient, BodyPartRecord part, Pawn surgeon, bool surgeryFailed)
        {
            if (!IsViolationOnPawn(patient, part, Faction.OfPlayer)) return;
            
            Log.Message("Reporting installation violation for patient: " + patient.Name);
            
            if (!surgeryFailed)
            {
                ReportViolation(patient, surgeon, patient.HomeFaction, -200);
                return;
            }
            ReportViolation(patient, surgeon, patient.HomeFaction, -125);
        }

        private void TryRemoveHediff(Pawn patient)
        {
            Hediff hediffToRemove;
            if (recipe.HasModExtension<CCRecipeDefModExtension>())
            {
                foreach (HediffDef hediff in recipe.GetModExtension<CCRecipeDefModExtension>().hediffsToRemove)
                {
                     hediffToRemove = patient.health.hediffSet.GetFirstHediffOfDef(hediff);
                     if (hediffToRemove == null) continue;
                     Log.Message("Removed hediff: " + hediffToRemove.Label + "from " + patient.Name);
                     patient.health.RemoveHediff(hediffToRemove);
                }
                return;
            }
            hediffToRemove = patient.health.hediffSet.GetFirstHediffOfDef(recipe.removesHediff);
            if (hediffToRemove != null)
            {
                patient.health.RemoveHediff(hediffToRemove);
            }
        }
    }
}