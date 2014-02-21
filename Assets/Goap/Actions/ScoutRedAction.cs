using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoutRedAction : Action {
	
	public ScoutRedAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("redResourceIsAvailable", new WorldStateValue(false));
				
		postConditions = new WorldState();
		postConditions.setProperty("redResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Red");
		//agentTypes.Add("Blue");
		//agentTypes.Add("Yellow");
		
		this.actionName = "ScoutRedAction";
		
		
	}
}
