using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildBlueFloorAction : Action {
	
	public BuildBlueFloorAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.SetProperty("blueFloorIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue");
		
		this.actionName = "BuildBlueFloorAction";
		
		
	}
}
