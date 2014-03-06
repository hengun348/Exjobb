using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildGreenFactoryAction : Action {
	
	public BuildGreenFactoryAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("yellowResourceIsCollected", new WorldStateValue(true));
		preConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(true));
		//preConditions.setProperty("greenResourceIsAvailable", new WorldStateValue(false));
		
		postConditions = new WorldState();
		postConditions.SetProperty("greenFactoryIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("yellowResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("greenResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue&Yellow");
		
		this.actionName = "BuildGreenFactoryAction";
		
		
	}
}
