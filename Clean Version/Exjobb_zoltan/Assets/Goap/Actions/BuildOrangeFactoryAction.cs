using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildOrangeFactoryAction : Action {
	
	public BuildOrangeFactoryAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("yellowResourceIsCollected", new WorldStateValue(true));
		preConditions.SetProperty("redResourceIsCollected", new WorldStateValue(true));
		//preConditions.setProperty("orangeResourceIsAvailable", new WorldStateValue(false));
		
		postConditions = new WorldState();
		postConditions.SetProperty("orangeFactoryIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("redResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("yellowResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("orangeResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Yellow&Red");
		
		this.actionName = "BuildOrangeFactoryAction";
		
		
	}
}
