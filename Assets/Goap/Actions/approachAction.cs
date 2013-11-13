using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class approachAction : Action {
	
	public approachAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("enemyVisible", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("nearEnemy", new WorldStateValue(true));
		
		cost = 3.0f;
		time = 2.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("specialAgent");
		
		this.actionName = "approachAction";

	}
	
}
