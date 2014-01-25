using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FleeAction : Action {
	
	public FleeAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("enemyVisible", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("nearEnemy", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 3.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("CommanderAgent");
		agentTypes.Add("Builder");
		
		this.actionName = "FleeAction";

	}
	
}
