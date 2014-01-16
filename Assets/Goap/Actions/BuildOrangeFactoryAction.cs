using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildOrangeFactoryAction : Action {
	
	public BuildOrangeFactoryAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("yellowResourceIsCollected", new WorldStateValue(true));
		preConditions.setProperty("redResourceIsCollected", new WorldStateValue(true));
		//preConditions.setProperty("orangeResourceIsAvailable", new WorldStateValue(false));
		
		postConditions = new WorldState();
		postConditions.setProperty("orangeFactoryIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("redResourceIsCollected", new WorldStateValue(false));
		postConditions.setProperty("yellowResourceIsCollected", new WorldStateValue(false));
		postConditions.setProperty("orangeResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Yellow&Red");
		
		this.actionName = "BuildOrangeFactoryAction";
		
		
	}
}
