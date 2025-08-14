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

		public int regen = 50;

		public CompProperties_ControlledUndead()
		{
			compClass = typeof(CompControlledUndead);
		}

		public override void ResolveReferences(ThingDef parentDef)
		{
			foreach (CompProperties comp in parentDef.comps.ToList())
			{
				if (comp is CompProperties_MechRepairable)
				{
                    parentDef.comps.Remove(comp);
                }
                if (comp is CompProperties_OverseerSubject)
                {
                    parentDef.comps.Remove(comp);
                    parentDef.comps.Add(new CompProperties_OverseerSubject_Undead());
                }
            }
		}

	}


	/// <summary>
	/// Main undead comp. All undead tickers should be here.
	/// </summary>
	public class CompControlledUndead : ThingComp
	{

		public CompProperties_ControlledUndead Props => (CompProperties_ControlledUndead)props;

		//private int nextTick = 240;
		public override void CompTickInterval(int delta)
		{
			if (!parent.IsHashIntervalTick(4333, delta))
			{
				return;
			}
            Regen(4333);
		}

		public void Regen(int tick)
		{
			Utility.Regeneration(parent as Pawn, Props.regen, tick);
        }

		//public void Energy()
		//{
		//	if (parent is Pawn pawn && pawn.needs?.energy != null)
		//	{
		//		pawn.needs.energy.CurLevel = pawn.needs.energy.MaxLevel;
		//	}
		//}

	}

    /// <summary>
    /// Only internal use. Do not touch.
    /// Needed to remove feral check. May cause conflict with mods that have crooked check, but it is not our problem.
    /// </summary>
    public class CompProperties_OverseerSubject_Undead : CompProperties_OverseerSubject
    {

        public CompProperties_OverseerSubject_Undead()
        {
            compClass = typeof(CompOverseerSubject_Undead);
        }

    }

	public class CompOverseerSubject_Undead : CompOverseerSubject
	{

		public override void CompTick()
		{
			//Log.Error("AAAAAAAAAAA");
		}

	}

}
