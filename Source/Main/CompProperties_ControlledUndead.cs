using RimWorld;
using RimWorld.Planet;
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
    public class CompProperties_AutoResurrector : CompProperties
    {

        public CompProperties_AutoResurrector()
        {
            compClass = typeof(ThingComp_AutoResurrector);
        }

    }

    public class ThingComp_AutoResurrector : ThingComp
    {

        private int resurrectionDelay;

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            resurrectionDelay = Find.TickManager.TicksGame + 60000;
        }

        public override void CompTickRare()
        {
            UndeadTick();
        }

        public void UndeadTick()
        {
            if (Find.TickManager.TicksGame < resurrectionDelay)
            {
                return;
            }
            if (parent is not Pawn pawn)
            {
                return;
            }
            if (pawn.Corpse?.Map == null)
            {
                return;
            }
            ResurrectionUtility.TryResurrect(pawn);
            if (pawn.GetOverseer() != null)
            {
                return;
            }
            Pawn overseer = pawn.Map != null ? pawn.Map?.mapPawns?.AllHumanlike?.Where((p) => p.mechanitor != null && p.Faction == Faction.OfPlayer)?.RandomElement() : pawn.GetCaravan()?.PawnsListForReading?.Where((p) => p.mechanitor != null && p.Faction == Faction.OfPlayer)?.RandomElement();
            if (overseer != null)
            {
                pawn.SetOverseer(overseer);
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref resurrectionDelay, "resurrectionDelay_BMT_Undead", 0);
        }

    }

    /// <summary>
    /// Only internal use. Do not touch.
    /// Needed to remove feral check. May cause conflict with mods that have crooked CompProperties_OverseerSubject check, but it is not our problem.
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
