using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildMagentaFactoryAction : Action {
	
	public BuildMagentaFactoryAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("redResourceIsCollected", new WorldStateValue(true));
		preConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.SetProperty("magentaFactoryIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("redResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("magentaResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue&Red"); //A blue and a red agent has to cooperate to do this action
		
		this.actionName = "BuildMagentaFactoryAction";
		
		
	}
}
