using RimWorld;
using Verse;

namespace BMT_Undeads
{
    public class JobGiver_AIDefendNecromancer : JobGiver_AIDefendPawn
	{
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.GetComp<CompControlledUndead>()?.Master;
		}

		protected override float GetFlagRadius(Pawn pawn)
		{
			return 5f;
		}
	}

}
