using HarmonyLib;
using RimWorld;
using Verse;

namespace BMT_Undeads
{

    /// <summary>
    /// Temp patch. Req optimization
    /// </summary>


    [HarmonyPatch(typeof(CompOverseerSubject), "CanGoFeral")]
    public static class Patch_CompOverseerSubject_CanGoFeral
    {
        [HarmonyPostfix]
        public static void FeralFix(ref bool __result, Pawn pawn)
        {
            if (__result && pawn.IsUndead())
            {
                __result = false;
            }
        }
    }

}
