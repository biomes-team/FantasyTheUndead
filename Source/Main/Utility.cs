using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BMT_Undeads
{

    public static class Utility
    {

        public static bool IsUndead(this Pawn pawn)
        {
            return pawn.HasComp<CompControlledUndead>();
        }

        // Regeneration
        public static bool Regeneration(Pawn pawn, float regeneration, int tick)
        {
            List<Hediff_Injury> tmpHediffInjuries = [];
            List<Hediff_MissingPart> tmpHediffMissing = [];
            regeneration /= 60000;
            if (tick > 0f)
            {
                regeneration *= tick;
            }
            bool woundRegen = false;
            if (regeneration > 0f)
            {
                pawn.health.hediffSet.GetHediffs(ref tmpHediffInjuries, (Hediff_Injury h) => h.def != null);
                foreach (Hediff_Injury tmpHediffInjury in tmpHediffInjuries)
                {
                    float num5 = Mathf.Min(regeneration, tmpHediffInjury.Severity);
                    regeneration -= num5;
                    tmpHediffInjury.Heal(num5);
                    pawn.health.hediffSet.Notify_Regenerated(num5);
                    woundRegen = true;
                    if (regeneration <= 0f)
                    {
                        break;
                    }
                }
                if (regeneration > 0f)
                {
                    pawn.health.hediffSet.GetHediffs(ref tmpHediffMissing, (Hediff_MissingPart missingPart) => missingPart.Part.parent != null && !tmpHediffInjuries.Any((Hediff_Injury injury) => injury.Part == missingPart.Part.parent) && pawn.health.hediffSet.GetFirstHediffMatchingPart<Hediff_MissingPart>(missingPart.Part.parent) == null && pawn.health.hediffSet.GetFirstHediffMatchingPart<Hediff_AddedPart>(missingPart.Part.parent) == null);
                    using List<Hediff_MissingPart>.Enumerator enumerator3 = tmpHediffMissing.GetEnumerator();
                    if (enumerator3.MoveNext())
                    {
                        Regenerate(pawn, enumerator3.Current);
                        woundRegen = true;
                    }
                }
            }
            return woundRegen;
        }

        public static void Regenerate(Pawn pawn, Hediff hediff)
        {
            BodyPartRecord part = hediff.Part;
            pawn.health.RemoveHediff(hediff);
            Regenerate(pawn, part);
        }

        public static void Regenerate(Pawn pawn, BodyPartRecord part, int initialAgeTicks = 0)
        {
            Hediff hediff2 = pawn.health.AddHediff(HediffDefOf.Misc, part);
            float partHealth = pawn.health.hediffSet.GetPartHealth(part);
            hediff2.Severity = Mathf.Max(partHealth - 1f, partHealth * 0.9f);
            hediff2.ageTicks = initialAgeTicks;
            pawn.health.hediffSet.Notify_Regenerated(partHealth - hediff2.Severity);
        }

    }

}
