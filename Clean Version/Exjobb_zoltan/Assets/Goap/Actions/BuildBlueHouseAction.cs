using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildBlueHouseAction : Action {
	
	public BuildBlueHouseAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(true, 2));
		
		postConditions = new WorldState();
		postConditions.SetProperty("blueHouseIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue"); //Only a blue agent can do this action
		
		this.actionName = "BuildBlueHouseAction";
		
		
	}
}
