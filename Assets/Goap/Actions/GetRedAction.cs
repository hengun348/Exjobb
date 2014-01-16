using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetRedAction : Action {
	
	public GetRedAction()
	{
		
		preConditions = new WorldState();
		//preConditions.setProperty("redResourceIsCollected", new WorldStateValue(false));
		preConditions.setProperty("redResourceIsAvailable", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("redResourceIsCollected", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Red");
		
		this.actionName = "GetRedAction";
		
		
	}
}
