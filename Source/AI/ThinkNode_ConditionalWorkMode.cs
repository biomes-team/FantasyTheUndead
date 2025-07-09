using RimWorld;
using Verse;
using Verse.AI;

namespace BMT_Undeads
{
    public class ThinkNode_ConditionalWorkMode : ThinkNode_Conditional
	{
		public SubHumanWorkModeDef workMode;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalWorkMode obj = (ThinkNode_ConditionalWorkMode)base.DeepCopy(resolve);
			obj.workMode = workMode;
			return obj;
		}

		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			// TEMP
			return pawn.GetComp<CompControlledUndead>()?.CurrentMode == workMode;
		}
	}

}
