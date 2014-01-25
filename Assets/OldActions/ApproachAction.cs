using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ApproachAction : Action {
	
	public ApproachAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("enemyVisible", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("nearEnemy", new WorldStateValue(true));
		
		cost = 3.0f;
		time = 2.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("CommanderAgent");
		agentTypes.Add("Builder");
		
		this.actionName = "ApproachAction";

	}
	
}
