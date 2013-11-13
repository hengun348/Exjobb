using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class fleeAction : Action {
	
	public fleeAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("enemyVisible", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("nearEnemy", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 3.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("normal");
		agentTypes.Add("specialAgent");
		
		this.actionName = "fleeAction";

	}
	
}
