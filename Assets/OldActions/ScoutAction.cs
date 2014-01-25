using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoutAction : Action {
	
	public ScoutAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("armedWithGun", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("enemyVisible", new WorldStateValue(true));
		
		cost = 4.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("CommanderAgent");
		agentTypes.Add("Builder");
		
		this.actionName = "ScoutAction";
	}
}
