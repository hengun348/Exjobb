using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShootAction : Action {
	
	public ShootAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("enemyLinedUp", new WorldStateValue(true));
	
		postConditions = new WorldState();
		postConditions.setProperty("enemyAlive", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 2.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("CommanderAgent");
		agentTypes.Add("Builder");
		
		this.actionName = "ShootAction";
	}
}
