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

		[Obsolete]
		public string defaultMutantDef;

		[Obsolete]
		public string uniqueTag = "BMT_UNDEAD";

		public CompProperties_ControlledUndead()
		{
			compClass = typeof(CompControlledUndead);
		}

		//public override void ResolveReferences(ThingDef parentDef)
		//{
		//	parentDef.inspectorTabs.Remove(typeof(ITab_Pawn_Character));
		//}

	}

	public class CompControlledUndead : ThingComp
	{

		public CompProperties_ControlledUndead Props => (CompProperties_ControlledUndead)props;

		private int nextTick = 240;
		public override void CompTickInterval(int delta)
		{
			nextTick -= delta;
			if (nextTick > 0)
			{
				return;
			}
			Energy();
			nextTick = 125543;
		}

		public void Energy()
        {
			if (parent is Pawn pawn && pawn.needs?.energy != null)
            {
				pawn.needs.energy.CurLevel = pawn.needs.energy.MaxLevel;
			}
        }

	}

	//public class CompControlledUndead : ThingComp
	//{

	//	public CompProperties_ControlledUndead Props => (CompProperties_ControlledUndead)props;

	//	//public SubHumanWorkModeDef currentMode = null;

	//	private Pawn undeadMaster;

	//	public SubHumanWorkModeDef CurrentMode => undeadMaster.GetComp<CompNecromancer>()?.currentMode;

	//	public bool Controlled => undeadMaster != null;

	//	public Pawn Master
	//	{
	//		get
	//		{
	//			return undeadMaster;
	//		}
	//	}

	//	//public override void PostPostMake()
	// //      {
	// //          SetupWorkAI();
	// //      }

	//	private void SetupWorkAI()
	//	{
	//		if (Props.defaultMutantDef != null && parent is Pawn undead)
	//           {
	//               MutantUtility.SetPawnAsMutantInstantly(undead, Props.defaultMutantDef);
	//               undead.SetFaction(Faction.OfPlayer);
	//               undead.skills = new(undead);
	//               if (undead.skills != null)
	//               {
	//                   // fixed skills setup
	//                   SetupSkills(undead);
	//               }
	//               undead.story = new(undead);
	//               if (undead.story != null)
	//               {
	//                   // story is required to use skills. Child27 is the default without affecting skills.
	//                   undead.story.Childhood = MiscDefOf.Child27;
	//                   undead.story.Adulthood = null;
	//               }
	//               undead.workSettings = new(undead);
	//               undead.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
	//               undead.drafter = new(undead);
	//               SetMaster(undead.Map.mapPawns.AllHumanlike.First((colonist) => colonist.IsColonistPlayerControlled));
	//               SetupWork(undead);
	//           }
	//       }

	//       private static void SetupWork(Pawn undead)
	//       {
	//           foreach (WorkTypeDef workType in DefDatabase<WorkTypeDef>.AllDefsListForReading)
	//           {
	//               if (!undead.RaceProps.mechEnabledWorkTypes.NullOrEmpty() && undead.RaceProps.mechEnabledWorkTypes.Contains(workType))
	//               {
	//                   undead.workSettings.SetPriority(workType, 3);
	//               }
	//               else
	//               {
	//                   undead.workSettings.SetPriority(workType, 0);
	//               }
	//           }
	//       }

	//       public static void SetupSkills(Pawn pawn)
	//	{
	//		foreach (SkillRecord skill in pawn.skills.skills.ToList())
	//		{
	//			pawn.skills.skills.Remove(skill);
	//			SkillRecord item = new(pawn, skill.def)
	//			{
	//				levelInt = pawn.RaceProps.mechFixedSkillLevel,
	//				passion = Passion.None,
	//				xpSinceLastLevel = 0,
	//				xpSinceMidnight = 0
	//			};
	//			item.xpSinceLastLevel = item.XpRequiredForLevelUp / 2;
	//			pawn.skills.skills.Add(item);
	//		}
	//	}

	//	public override void PostSpawnSetup(bool respawningAfterLoad)
	//       {
	//           if (!respawningAfterLoad)
	//		{
	//			SetupWorkAI();
	//		}
	//       }

	//       public override void PostDestroy(DestroyMode mode, Map previousMap)
	//	{
	//		if (!Controlled)
	//		{
	//			return;
	//		}
	//		//Master.
	//	}

	//	private int nextTick = 240;
	//	public override void CompTickInterval(int delta)
	//	{
	//		nextTick -= delta;
	//		if (nextTick > 0)
	//           {
	//			return;
	//           }
	//		Debug();
	//		nextTick = 55543;
	//	}

	//	public void Debug()
	//	{
	//		Pawn undead = parent as Pawn;
	//		SetupWork(undead);
	//		SetupSkills(undead);
	//	}

	//	public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
	//	{
	//		if (!Controlled)
	//		{
	//			return;
	//		}
	//		RemoveThisUndead();
	//		SetMaster(null);
	//	}

	//	public void RemoveThisUndead()
	//	{
	//		//Master.;
	//	}

	//	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	//	{
	//		if (DebugSettings.ShowDevGizmos)
	//		{
	//			yield return new Command_Action
	//			{
	//				defaultLabel = "DEV: SetMaster" + (undeadMaster != null ? undeadMaster.Name : "None"),
	//				action = delegate
	//				{
	//					Pawn pawn = parent as Pawn;
	//					List<FloatMenuOption> list = new();
	//					List<Pawn> mutants = pawn.Map.mapPawns.AllHumanlike.Where((master) => master.IsColonistPlayerControlled).ToList();
	//					for (int i = 0; i < mutants.Count; i++)
	//					{
	//						Pawn geneSet = mutants[i];
	//						list.Add(new FloatMenuOption(geneSet.LabelCap, delegate
	//						{
	//							SetMaster(geneSet);
	//						}, orderInPriority: 0 - i));
	//					}
	//					Find.WindowStack.Add(new FloatMenu(list));
	//				}
	//			};
	//			yield return new Command_Action
	//			{
	//				defaultLabel = "DEV: SetMutantDef",
	//				action = delegate
	//				{
	//					Pawn pawn = parent as Pawn;
	//					List<FloatMenuOption> list = new();
	//					List<MutantDef> mutants = DefDatabase<MutantDef>.AllDefsListForReading;
	//					for (int i = 0; i < mutants.Count; i++)
	//					{
	//						MutantDef geneSet = mutants[i];
	//						list.Add(new FloatMenuOption(geneSet.LabelCap, delegate
	//						{
	//							MutantUtility.SetPawnAsMutantInstantly(pawn, geneSet);
	//							pawn?.Drawer?.renderer?.SetAllGraphicsDirty();
	//							SetMaster(pawn.Map.mapPawns.AllHumanlike.First((colonist) => colonist.IsColonistPlayerControlled));
	//						}, orderInPriority: 0 - i));
	//					}
	//					Find.WindowStack.Add(new FloatMenu(list));
	//				}
	//			};
	//			yield return new Command_Action
	//			{
	//				defaultLabel = "DEV: SetMutantRotStage",
	//				action = delegate
	//				{
	//					Pawn pawn = parent as Pawn;
	//					List<FloatMenuOption> list = new();
	//					List<RotStage> mutants = new() { RotStage.Dessicated, RotStage.Fresh, RotStage.Rotting };
	//					for (int i = 0; i < mutants.Count; i++)
	//					{
	//						RotStage geneSet = mutants[i];
	//						list.Add(new FloatMenuOption(geneSet.ToString(), delegate
	//						{
	//							pawn.mutant.rotStage = geneSet;
	//							pawn?.Drawer?.renderer?.SetAllGraphicsDirty();
	//						}, orderInPriority: 0 - i));
	//					}
	//					Find.WindowStack.Add(new FloatMenu(list));
	//				}
	//			};
	//		}
	//	}

	//	public void SetMaster(Pawn master)
	//	{
	//		undeadMaster = master;
	//	}

	//	public override void PostExposeData()
	//	{
	//		base.PostExposeData();
	//		//Scribe_Defs.Look(ref currentMode, "currentMode_" + Props.uniqueTag);
	//		Scribe_References.Look(ref undeadMaster, "dryadMaster_" + Props.uniqueTag);
	//	}

	//}

}
