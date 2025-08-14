using HarmonyLib;
using RimWorld;
using Verse;

namespace BMT_Undeads
{
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    public static class Patch_Pawn_NeedsTracker_ShouldHaveNeed
    {
        [HarmonyPrefix]
        public static bool DisableUndeadNeeds(ref bool __result, Pawn ___pawn)
        {
            if (___pawn.IsUndead())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

}
