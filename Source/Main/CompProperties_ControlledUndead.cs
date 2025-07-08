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

		public PawnKindDef defaultDryadPawnKindDef;

		public string uniqueTag = "BMT_UNDEAD";

		public CompProperties_ControlledUndead()
		{
			compClass = typeof(CompControlledUndead);
		}

	}

	public class CompControlledUndead : ThingComp
	{

		public CompProperties_ControlledUndead Props => (CompProperties_ControlledUndead)props;

		public SubHumanWorkModeDef currentMode = null;

		private Pawn undeadMaster;

		public bool Controlled => undeadMaster != null;

		// [Unsaved(false)]
		// private Pawn cachedConnectedPawn;

		// public Pawn Master
		// {
		// get
		// {
		// if (parent is Pawn pawn && (cachedConnectedPawn == null || pawn.Faction != cachedConnectedPawn.Faction))
		// {
		// cachedConnectedPawn = pawn?.connections?.ConnectedThings.FirstOrDefault() is Pawn master ? master : null;
		// }
		// return cachedConnectedPawn;
		// }
		// }

		public Pawn Master
		{
			get
			{
				return undeadMaster;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				Pawn pawn = parent as Pawn;
				if (pawn?.needs?.rest != null)
				{
					pawn.needs.rest.CurLevel = pawn.needs.rest.MaxLevel;
				}
				// ResetOverseerTick();
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (!Controlled)
			{
				return;
			}
			Pawn pawn = parent as Pawn;
		}

		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			if (!Controlled)
			{
				return;
			}
			RemoveThisUndead(true);
			SetMaster(null);
		}

		public void RemoveThisUndead(bool gainMemory = false)
		{
			Pawn pawn = parent as Pawn;
		}

		//public override IEnumerable<Gizmo> CompGetGizmosExtra()
		//{
		//	Pawn pawn = parent as Pawn;
		//	if (!Controlled || pawn.Faction != Faction.OfPlayer)
		//	{
		//		yield break;
		//	}
		//	yield return new Command_Action
		//	{
		//		defaultLabel = "ChangeMode".Translate(),
		//		defaultDesc = "WVC_XaG_CompGauranlenDryad_ChangeMode".Translate(),
		//		icon = currentMode?.pawnKindDef != null ? Widgets.GetIconFor(currentMode.pawnKindDef.race) : Widgets.GetIconFor(pawn.kindDef.race),
		//		action = delegate
		//		{
		//			Find.WindowStack.Add(new Dialog_ChangeDryadCaste(Gene_GauranlenConnection, this));
		//		}
		//	};
		//	yield return new Command_Action
		//	{
		//		defaultLabel = "WVC_XaG_GeneDryadsSelectDryadQueen".Translate(),
		//		defaultDesc = "WVC_XaG_GeneDryadsSelectDryadQueen_Desc".Translate(),
		//		icon = ContentFinder<Texture2D>.Get("WVC/UI/XaG_General/UI_SelectDryadQueen"),
		//		Order = -87f,
		//		action = delegate
		//		{
		//			Find.Selector.ClearSelection();
		//			Find.Selector.Select(undeadMaster);
		//		}
		//	};
		//}

		public void SetMaster(Pawn master)
		{
			undeadMaster = master;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look(ref currentMode, "currentMode_" + Props.uniqueTag);
			Scribe_References.Look(ref undeadMaster, "dryadMaster_" + Props.uniqueTag);
		}

	}

}
