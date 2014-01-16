using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildRedHouseAction : Action {
	
	public BuildRedHouseAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("redResourceIsCollected", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("redHouseIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("redResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Red");
		
		this.actionName = "BuildRedHouseAction";
		
		
	}
}
