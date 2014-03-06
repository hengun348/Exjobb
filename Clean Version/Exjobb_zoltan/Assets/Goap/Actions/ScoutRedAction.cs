using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoutRedAction : Action {
	
	public ScoutRedAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("redResourceIsAvailable", new WorldStateValue(false));
				
		postConditions = new WorldState();
		postConditions.SetProperty("redResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Red");
		
		this.actionName = "ScoutRedAction";
		
		
	}
}
