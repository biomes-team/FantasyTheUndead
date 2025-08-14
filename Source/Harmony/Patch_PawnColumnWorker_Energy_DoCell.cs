using HarmonyLib;
using RimWorld;
using Verse;

namespace BMT_Undeads
{
    [HarmonyPatch(typeof(PawnColumnWorker_Energy), "DoCell")]
    public static class Patch_PawnColumnWorker_Energy_DoCell
    {
        [HarmonyPrefix]
        public static bool DisableUndeadNeeds(ref Pawn pawn)
        {
            if (pawn.IsUndead())
            {
                return false;
            }
            return true;
        }
    }

}
