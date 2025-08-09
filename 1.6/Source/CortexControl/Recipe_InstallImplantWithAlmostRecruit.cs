using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CortexControl
{
    public class Recipe_InstallImplantWithAlmostRecruit : Recipe_InstallImplant
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
            AlmostRecruitPawn(patient, bill);
            patient.health.AddHediff(recipe.addsHediff, part);
            var hediffToRemove = patient.health.hediffSet.GetFirstHediffOfDef(recipe.removesHediff);
            if (hediffToRemove != null)
            {
                patient.health.RemoveHediff(hediffToRemove);
            }
            if (!patient.Dead)
            {
                if (IsViolationOnPawn(patient, part, Faction.OfPlayer))
                    ReportViolation(patient, surgeon, patient.HomeFaction, -180);
                return;
            }
            ThoughtUtility.GiveThoughtsForPawnExecuted(patient, surgeon, PawnExecutionKind.GenericBrutal);
        }

        private void AlmostRecruitPawn(Pawn patient, Bill bill)
        {
            if (!patient.guest.Recruitable)
            {
                var loyaltyMessage = $"{patient.Name}'s Unwavering Loyalty has been removed due to {recipe.addsHediff.label}.";
                patient.guest.Recruitable = true;
                Log.Message(loyaltyMessage);
                Messages.Message(loyaltyMessage, MessageTypeDefOf.PositiveEvent);
            }
            if (patient.guest.resistance > 0)
            {
                patient.guest.resistance = 0;
            }
            if (patient.guest.will > 0)
            {
                patient.guest.will = 0;
            }
        }
    }
}