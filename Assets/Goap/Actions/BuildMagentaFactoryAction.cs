using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildMagentaFactoryAction : Action {
	
	public BuildMagentaFactoryAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("redResourceIsCollected", new WorldStateValue(true));
		preConditions.setProperty("blueResourceIsCollected", new WorldStateValue(true));
		//preConditions.setProperty("magentaResourceIsAvailable", new WorldStateValue(false));
		
		
		postConditions = new WorldState();
		postConditions.setProperty("magentaFactoryIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("blueResourceIsCollected", new WorldStateValue(false));
		postConditions.setProperty("redResourceIsCollected", new WorldStateValue(false));
		postConditions.setProperty("magentaResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue&Red");
		
		this.actionName = "BuildMagentaFactoryAction";
		
		
	}
}
