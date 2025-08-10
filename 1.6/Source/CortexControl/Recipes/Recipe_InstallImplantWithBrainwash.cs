using RimWorld;
using Verse;
using System.Collections.Generic;
using static CortexControl.CC_RecipeUtils;

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
            BrainwashPawn(patient, surgeon);
        }
    }
}