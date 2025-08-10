using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace CortexControl
{
    public class Recipe_RemoveImplantControl :Recipe_RemoveImplant
    {
        public override void ApplyOnPawn(Pawn patient, BodyPartRecord part, Pawn surgeon, List<Thing> ingredients, Bill bill)
        {
            MedicalRecipesUtility.IsClean(patient, part);
            if (surgeon == null) return;
            bool surgeryFailed = CheckSurgeryFail(surgeon, patient, ingredients, part, bill);
            IsCCRemoveViolation(patient, part, surgeon, surgeryFailed);
            if (surgeryFailed) return;
            TaleRecorder.RecordTale(TaleDefOf.DidSurgery, surgeon, patient);
            if (!patient.health.hediffSet.GetNotMissingParts().Contains(part))  return;

            Hediff hediff = patient.health.hediffSet.hediffs.FirstOrDefault((Hediff x) => x.def == recipe.removesHediff);
            if (hediff == null) return;
            if (hediff.def.spawnThingOnRemoved != null)
            {
                GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, surgeon.Position, surgeon.Map);
            }
            patient.health.RemoveHediff(hediff);
            patient.health.AddHediff(recipe.addsHediff, part);
            patient.ChangeKind(PawnKindDefOf.WildMan);
            if (patient.Faction != null)
            {
                patient.SetFaction(null);
            }
        }
        
        private void IsCCRemoveViolation(Pawn patient, BodyPartRecord part, Pawn surgeon, bool surgeryFailed)
        {
            if (!IsViolationOnPawn(patient, part, Faction.OfPlayer)) return;
            
            Log.Message("Reporting removal violation for patient: " + patient.Name);
            
            if (!surgeryFailed)
            {
                ReportViolation(patient, surgeon, patient.HomeFaction, -100);
                return;
            }
            ReportViolation(patient, surgeon, patient.HomeFaction, -50);
        }
    }
}