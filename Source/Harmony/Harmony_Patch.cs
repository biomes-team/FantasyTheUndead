using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace BMT_Undeads
{

    [StaticConstructorOnStartup]
	public static class BMT_Undeads
	{
		static BMT_Undeads()
		{
			new Harmony("biomesteam.fantasytheundead").PatchAll();
		}
    }

}
