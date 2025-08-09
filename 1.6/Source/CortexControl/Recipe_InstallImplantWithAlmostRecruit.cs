using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CortexControl
{
    public class Recipe_InstallImplantWithAlmostRecruit : Recipe_InstallImplantCC
    {
        public override void ApplyOnPawn(
            Pawn patient,
            BodyPartRecord part,
            Pawn surgeon,
            List<Thing> ingredients,
            Bill bill)
        {
            base.ApplyOnPawn(patient, part, surgeon, ingredients, bill);
            if (!patient.Dead) AlmostRecruitPawn(patient);
        }
        
        private void AlmostRecruitPawn(Pawn patient)
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