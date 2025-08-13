using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using static CortexControl.CC_RecipeUtils;
namespace CortexControl
{
  public class Recipe_ReprogramControlChip : Recipe_Surgery
  {
    public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
    {
      if (!recipe.HasModExtension<CCRecipeDefModExtension>()) yield break;
      List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
      foreach (Hediff hediff in allHediffs)
      {
        if(recipe.GetModExtension<CCRecipeDefModExtension>().requiredHediffs.Contains(hediff.def)) yield return hediff.Part;
      }
    }

    public override void ApplyOnPawn(Pawn patient, BodyPartRecord part, Pawn surgeon, List<Thing> ingredients, Bill bill)
    {
      if (surgeon == null || CheckSurgeryFail(surgeon, patient, ingredients, part, bill))
      {
        return;
      }
      if(recipe.isViolation)  ReportViolation(patient, surgeon, patient.HomeFaction, -200);
      BrainwashPawn(patient, surgeon);
      TaleRecorder.RecordTale(TaleDefOf.DidSurgery, (object) surgeon, (object) patient);
    }
  }
}