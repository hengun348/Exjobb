using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildPurpleHouseAction : Action {
	
	public BuildPurpleHouseAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("blueResourceIsAvailable", new WorldStateValue(true));
		preConditions.setProperty("redResourceIsAvailable", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("purpleHouseIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("blueResourceIsAvailable", new WorldStateValue(false));
		postConditions.setProperty("redResourceIsAvailable", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
				
		agentTypes = new List<string>();
		agentTypes.Add("BlueBuilder&RedBuilder");
		agentTypes.Add("OrangeBuilder");
		
		this.actionName = "BuildPurpleHouseAction";
		
		
	}
}
