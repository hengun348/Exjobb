using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AimAction : Action {
	
	public AimAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("enemyVisible", new WorldStateValue(true));
		preConditions.setProperty("weaponLoaded", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("enemyLinedUp", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("normal");
		
		this.actionName = "AimAction";
	}
}
