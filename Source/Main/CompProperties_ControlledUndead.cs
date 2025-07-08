using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_Undeads
{

	public class CompProperties_ControlledUndead : CompProperties
	{

		public MutantDef defaultMutantDef;

		public string uniqueTag = "BMT_UNDEAD";

		public CompProperties_ControlledUndead()
		{
			compClass = typeof(CompControlledUndead);
		}

	}

	public class CompControlledUndead : ThingComp
	{

		public CompProperties_ControlledUndead Props => (CompProperties_ControlledUndead)props;

		//public SubHumanWorkModeDef currentMode = null;

		private Pawn undeadMaster;

		public SubHumanWorkModeDef CurrentMode => undeadMaster.GetComp<CompNecromancer>()?.currentMode;

		public bool Controlled => undeadMaster != null;

		public Pawn Master
		{
			get
			{
				return undeadMaster;
			}
		}

		//public override void PostPostMake()
  //      {
  //          SetupWorkAI();
  //      }

        private void SetupWorkAI()
        {
            if (Props.defaultMutantDef != null && parent is Pawn pawn)
            {
                MutantUtility.SetPawnAsMutantInstantly(pawn, Props.defaultMutantDef);
				pawn.SetFaction(Faction.OfPlayer);
                pawn.skills = new(pawn);
				pawn.story = new(pawn);
				pawn.workSettings = new(pawn);
				pawn.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
				SetMaster(pawn.Map.mapPawns.AllHumanlike.First((colonist) => colonist.IsColonistPlayerControlled));
				if (pawn.RaceProps.mechEnabledWorkTypes.NullOrEmpty())
                {
					return;
                }
                foreach (WorkTypeDef workType in pawn.RaceProps.mechEnabledWorkTypes)
                {
                    if (pawn.RaceProps.mechEnabledWorkTypes.Contains(workType))
                    {
                        pawn.workSettings.SetPriority(workType, 3);
                    }
                    else
                    {
                        pawn.workSettings.SetPriority(workType, 0);
                    }
                }
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (!respawningAfterLoad)
			{
				SetupWorkAI();
			}
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (!Controlled)
			{
				return;
			}
			//Master.
		}

		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			if (!Controlled)
			{
				return;
			}
			RemoveThisUndead();
			SetMaster(null);
		}

		public void RemoveThisUndead()
		{
			//Master.;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: SetMaster" + (undeadMaster != null ? undeadMaster.Name : "None"),
					action = delegate
					{
						Pawn pawn = parent as Pawn;
						List<FloatMenuOption> list = new();
						List<Pawn> mutants = pawn.Map.mapPawns.AllHumanlike.Where((master) => master.IsColonistPlayerControlled).ToList();
						for (int i = 0; i < mutants.Count; i++)
						{
							Pawn geneSet = mutants[i];
							list.Add(new FloatMenuOption(geneSet.LabelCap, delegate
							{
								SetMaster(geneSet);
							}, orderInPriority: 0 - i));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: SetMutantDef",
					action = delegate
					{
						Pawn pawn = parent as Pawn;
						List<FloatMenuOption> list = new();
						List<MutantDef> mutants = DefDatabase<MutantDef>.AllDefsListForReading;
						for (int i = 0; i < mutants.Count; i++)
						{
							MutantDef geneSet = mutants[i];
							list.Add(new FloatMenuOption(geneSet.LabelCap, delegate
							{
								MutantUtility.SetPawnAsMutantInstantly(pawn, geneSet);
								pawn?.Drawer?.renderer?.SetAllGraphicsDirty();
								SetMaster(pawn.Map.mapPawns.AllHumanlike.First((colonist) => colonist.IsColonistPlayerControlled));
							}, orderInPriority: 0 - i));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: SetMutantRotStage",
					action = delegate
					{
						Pawn pawn = parent as Pawn;
						List<FloatMenuOption> list = new();
						List<RotStage> mutants = new() { RotStage.Dessicated, RotStage.Fresh, RotStage.Rotting };
						for (int i = 0; i < mutants.Count; i++)
						{
							RotStage geneSet = mutants[i];
							list.Add(new FloatMenuOption(geneSet.ToString(), delegate
							{
								pawn.mutant.rotStage = geneSet;
								pawn?.Drawer?.renderer?.SetAllGraphicsDirty();
							}, orderInPriority: 0 - i));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
			}
		}

		public void SetMaster(Pawn master)
		{
			undeadMaster = master;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			//Scribe_Defs.Look(ref currentMode, "currentMode_" + Props.uniqueTag);
			Scribe_References.Look(ref undeadMaster, "dryadMaster_" + Props.uniqueTag);
		}

	}

}
