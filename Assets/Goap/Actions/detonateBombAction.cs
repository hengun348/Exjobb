using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class detonateBombAction : Action {
	
	public detonateBombAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("armedWithBomb", new WorldStateValue(true));
		preConditions.setProperty("nearEnemy", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("agentAlive", new WorldStateValue(false));
		postConditions.setProperty("enemyAlive", new WorldStateValue(false));
		
		cost = 6.0f;
		time = 6.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("specialAgent");
		
		this.actionName = "detonateBombAction";
	}
}
