using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_Undeads
{

	public class CompProperties_Necromancer : CompProperties
	{

		//public PawnKindDef defaultDryadPawnKindDef;

		public string uniqueTag = "BMT_UNDEAD";

		public CompProperties_Necromancer()
		{
			compClass = typeof(CompNecromancer);
		}

	}

	public class CompNecromancer : ThingComp
	{

		public CompProperties_Necromancer Props => (CompProperties_Necromancer)props;

		public SubHumanWorkModeDef currentMode = null;

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: SetWorkMode",
					action = delegate
					{
						Pawn pawn = parent as Pawn;
						List<FloatMenuOption> list = new();
						List<SubHumanWorkModeDef> mutants = DefDatabase<SubHumanWorkModeDef>.AllDefsListForReading;
						for (int i = 0; i < mutants.Count; i++)
						{
							SubHumanWorkModeDef geneSet = mutants[i];
							list.Add(new FloatMenuOption(geneSet.LabelCap, delegate
							{
								currentMode = geneSet;
							}, orderInPriority: 0 - i));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
				//yield return new Command_Action
				//{
				//	defaultLabel = "DEV: SetMutantDef",
				//	action = delegate
				//	{
				//		Pawn pawn = parent as Pawn;
				//		List<FloatMenuOption> list = new();
				//		List<MutantDef> mutants = DefDatabase<MutantDef>.AllDefsListForReading;
				//		for (int i = 0; i < mutants.Count; i++)
				//		{
				//			MutantDef geneSet = mutants[i];
				//			list.Add(new FloatMenuOption(geneSet.LabelCap, delegate
				//			{
				//				MutantUtility.SetPawnAsMutantInstantly(pawn, geneSet);
				//				pawn?.Drawer?.renderer?.SetAllGraphicsDirty();
				//			}, orderInPriority: 0 - i));
				//		}
				//		Find.WindowStack.Add(new FloatMenu(list));
				//	}
				//};
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look(ref currentMode, "currentMode_" + Props.uniqueTag);
		}

	}

}
