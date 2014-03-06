using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildYellowFloorAction : Action {
	
	public BuildYellowFloorAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("yellowResourceIsCollected", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.SetProperty("yellowFloorIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("yellowResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Yellow");
		
		this.actionName = "BuildYellowFloorAction";
		
		
	}
}
