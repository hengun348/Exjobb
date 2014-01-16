using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildBlueHouseAction : Action {
	
	public BuildBlueHouseAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("blueResourceIsCollected", new WorldStateValue(true, 2));
		
		postConditions = new WorldState();
		postConditions.setProperty("blueHouseIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("blueResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue");
		
		this.actionName = "BuildBlueHouseAction";
		
		
	}
}
