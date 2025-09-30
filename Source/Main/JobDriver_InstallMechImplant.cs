using BMT_Undeads;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BMT_Undeads
{

    public class CompTargetable_DormantPlayerMechanoid : CompTargetable
    {
        protected override bool PlayerChoosesTarget => true;

        public CompTargetEffect_InstallImplantInTarget Install_comp => parent.TryGetComp<CompTargetEffect_InstallImplantInTarget>();

        protected override TargetingParameters GetTargetingParameters()
        {
            return new TargetingParameters
            {
                canTargetPawns = true,
                canTargetBuildings = false,
                canTargetItems = false,
                mapObjectTargetsMustBeAutoAttackable = false,
                validator = x => x.Thing is Pawn pawn && ValidateTarget(x.Thing) && Install_comp != null && Utility.HasUnoccupiedBodyParts(pawn, Install_comp)
            };
        }

        public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
        {
            yield return targetChosenByPlayer;
        }
    }

    public class CompProperties_TargetEffect_InstallImplantInTarget : CompProperties
	{
		public JobDef jobDef;

		public ThingDef moteDef;

		public HediffDef hediffDef;

		public BodyPartDef bodyPart;

		public CompProperties_TargetEffect_InstallImplantInTarget()
		{
			compClass = typeof(CompTargetEffect_InstallImplantInTarget);
		}
	}

	public class CompTargetEffect_InstallImplantInTarget : CompTargetEffect
	{

		public CompProperties_TargetEffect_InstallImplantInTarget Props => (CompProperties_TargetEffect_InstallImplantInTarget)props;

		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (user.IsColonistPlayerControlled && user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
			{
				Job job = JobMaker.MakeJob(Props.jobDef, target, parent);
				job.count = 1;
				user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}
		}
	}

	public class JobDriver_InstallMechImplant : JobDriver
	{

		private Mote warmupMote;

		private Pawn Pawn => (Pawn)job.GetTarget(TargetIndex.A);

		private Thing Item => job.GetTarget(TargetIndex.B).Thing;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (pawn.Reserve(Pawn, job, 1, -1, null, errorOnFailed))
			{
				return pawn.Reserve(Item, job, 1, -1, null, errorOnFailed);
			}
			return false;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(600);
			toil.WithProgressBarToilDelay(TargetIndex.A);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.tickAction = delegate
			{
				CompUsable compUsable = Item.TryGetComp<CompUsable>();
				if (compUsable != null && warmupMote == null && compUsable.Props.warmupMote != null)
				{
					warmupMote = MoteMaker.MakeAttachedOverlay(Pawn, compUsable.Props.warmupMote, Vector3.zero);
				}
				warmupMote?.Maintain();
			};
			yield return toil;
			yield return Toils_General.Do(Install);
		}

		private void Install()
		{
			CompTargetEffect_InstallImplantInTarget install_comp = Item?.TryGetComp<CompTargetEffect_InstallImplantInTarget>();
			if (install_comp == null)
			{
				return;
			}
			if (install_comp.Props.bodyPart == null || install_comp.Props.hediffDef == null)
			{
				return;
			}
			// Try Install In Unoccupied Part
			List<BodyPartRecord> bodyPartRecords = Pawn.RaceProps.body.GetPartsWithDef(install_comp.Props.bodyPart);
			for (int i = 0; i < bodyPartRecords.Count; i++)
			{
				if (bodyPartRecords[i] != null && Utility.GetFirstHediffOnPart(Pawn.health.hediffSet.hediffs, bodyPartRecords[i]) == null)
				{
                    Utility.InstallPartTo(Pawn, bodyPartRecords[i], install_comp.Props.hediffDef, install_comp.Props.moteDef, Item);
					return;
				}
			}
			// Try Install In First Part
			for (int i = 0; i < bodyPartRecords.Count; i++)
			{
				if (bodyPartRecords[i] != null && !Pawn.health.hediffSet.HasHediff(install_comp.Props.hediffDef, bodyPartRecords[i]))
				{
					ThingDef oldPart = Utility.GetFirstHediffOnPart(Pawn.health.hediffSet.hediffs, bodyPartRecords[i])?.def?.spawnThingOnRemoved;
					if (oldPart != null)
					{
						Thing thing = ThingMaker.MakeThing(oldPart);
						thing.stackCount = 1;
						GenPlace.TryPlaceThing(thing, Pawn.Position, Pawn.Map, ThingPlaceMode.Near, out var lastResultingThing, null, default);
					}
                    // Log.Error(bodyPartRecords[i].Label);
                    Utility.InstallPartTo(Pawn, bodyPartRecords[i], install_comp.Props.hediffDef, install_comp.Props.moteDef, Item);
					break;
				}
			}
		}
	}

}
