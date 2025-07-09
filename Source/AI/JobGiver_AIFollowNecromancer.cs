using RimWorld;
using Verse;
using Verse.AI;

namespace BMT_Undeads
{
    public class JobGiver_AIFollowNecromancer : JobGiver_AIFollowPawn
	{
		protected override int FollowJobExpireInterval => 200;

		protected override Pawn GetFollowee(Pawn pawn)
		{
			return pawn.GetComp<CompControlledUndead>()?.Master;
		}

		protected override float GetRadius(Pawn pawn)
		{
			return 5f;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetComp<CompControlledUndead>()?.Master == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}

}
