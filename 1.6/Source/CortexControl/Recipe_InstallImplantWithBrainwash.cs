using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CortexControl
{
    public class Recipe_InstallImplantWithBrainwash : Recipe_InstallImplant
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
                RecruitPawn(patient);
                ConvertPawn(patient, surgeon);
            }
        }
        
        private void RecruitPawn(Pawn patient)
        {
            patient.SetFaction(Faction.OfPlayer);
        }

        private void ConvertPawn(Pawn patient, Pawn surgeon)
        {
            if (ModLister.IdeologyInstalled)
            {
                patient.ideo.SetIdeo(surgeon.Ideo);
            }
        }
    }
}