using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class scoutAction : Action {
	
	public scoutAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("armedWithGun", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("enemyVisible", new WorldStateValue(true));
		
		cost = 4.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("normal");
		agentTypes.Add("specialAgent");
		
		this.actionName = "scoutAction";
	}
}
