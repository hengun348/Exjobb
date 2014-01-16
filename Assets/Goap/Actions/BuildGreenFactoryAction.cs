using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildGreenFactoryAction : Action {
	
	public BuildGreenFactoryAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("yellowResourceIsCollected", new WorldStateValue(true));
		preConditions.setProperty("blueResourceIsCollected", new WorldStateValue(true));
		//preConditions.setProperty("greenResourceIsAvailable", new WorldStateValue(false));
		
		postConditions = new WorldState();
		postConditions.setProperty("greenFactoryIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("blueResourceIsCollected", new WorldStateValue(false));
		postConditions.setProperty("yellowResourceIsCollected", new WorldStateValue(false));
		postConditions.setProperty("greenResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue&Yellow");
		
		this.actionName = "BuildGreenFactoryAction";
		
		
	}
}
