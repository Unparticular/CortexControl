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
                if (CheckSurgeryFail(surgeon, patient, ingredients, part, bill))
                    return;
                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, (object) surgeon, (object) patient);
            }
            TryRemoveHediff(patient);
            patient.health.AddHediff(recipe.addsHediff, part);
            IsCCInstallViolation(patient, part, surgeon);
        }
        protected void IsCCInstallViolation(Pawn patient, BodyPartRecord part, Pawn surgeon)
        {
            if (!patient.Dead)
            {
                if (IsViolationOnPawn(patient, part, Faction.OfPlayer))
                    ReportViolation(patient, surgeon, patient.HomeFaction, -180);
                return;
            }
            ThoughtUtility.GiveThoughtsForPawnExecuted(patient, surgeon, PawnExecutionKind.GenericBrutal);
        }

        protected void TryRemoveHediff(Pawn patient)
        {
            Hediff hediffToRemove;
            if (recipe.HasModExtension<CCRecipeDefModExtension>())
            {
                foreach (HediffDef hediff in recipe.GetModExtension<CCRecipeDefModExtension>().hediffsToRemove)
                {
                     hediffToRemove = patient.health.hediffSet.GetFirstHediffOfDef(hediff);
                    if (hediffToRemove != null)
                    {
                        patient.health.RemoveHediff(hediffToRemove);
                    }
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