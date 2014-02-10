using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildRedFloorAction : Action {
	
	public BuildRedFloorAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("redResourceIsCollected", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("redFloorIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("redResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Red");
		
		this.actionName = "BuildRedFloorAction";
		
		
	}
}
