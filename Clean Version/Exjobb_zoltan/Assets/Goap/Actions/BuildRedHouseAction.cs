using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildRedHouseAction : Action {
	
	public BuildRedHouseAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("redResourceIsCollected", new WorldStateValue(true, 2));
		
		postConditions = new WorldState();
		postConditions.SetProperty("redHouseIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("redResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Red");
		
		this.actionName = "BuildRedHouseAction";
		
		
	}
}
