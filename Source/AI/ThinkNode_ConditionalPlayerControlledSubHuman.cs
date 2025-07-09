using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BMT_Undeads
{

    public class ThinkNode_ConditionalPlayerControlledSubHuman : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetComp<CompControlledUndead>()?.Controlled == true;
		}
	}

}
