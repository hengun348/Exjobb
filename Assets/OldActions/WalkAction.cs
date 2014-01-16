using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WalkAction : Action {
	
	public WalkAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("hasDestination", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("reachedDestination", new WorldStateValue(true));
		
		cost = 4.0f;
		time = 9.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("CommanderAgent");
		agentTypes.Add("Builder");
		
		this.actionName = "WalkAction";
		
	}
	
}
