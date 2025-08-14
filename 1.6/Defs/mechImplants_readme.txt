

  Simple example

  <ThingDef Abstract="True">
	<recipeMaker>
	</recipeMaker>
	<thingCategories>
	</thingCategories>
	<tradeTags Inherit="False">
	</tradeTags>
	<techHediffsTags Inherit="False">
	</techHediffsTags>
	<comps>
	  <li Class="CompProperties_Usable">
		<useJob>UseItem</useJob>
		<useLabel>Use {0_label}</useLabel>
		<useDuration>0</useDuration>
		<warmupMote>Mote_ResurrectAbility</warmupMote>
	  </li>
	  <li Class="CompProperties_Targetable">
		<compClass>BMT_Undeads.CompTargetable_DormantPlayerMechanoid</compClass>
	  </li>
	  <li Class="BMT_Undeads.CompProperties_TargetEffect_InstallImplantInTarget">
		<jobDef>BMT_InstalUndeadBodypart</jobDef>
		<bodyPart>TargetBodyPart</bodyPart>
		<hediffDef>New_ImplantDef</hediffDef>
		<moteDef>Mote_ResurrectFlash</moteDef>
	  </li>
	</comps>
  </ThingDef>