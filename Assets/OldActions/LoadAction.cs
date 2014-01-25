using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LoadAction : Action {
	
	public LoadAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("armedWithGun", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("weaponLoaded", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 1.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("CommanderAgent");
		agentTypes.Add("Builder");
		
		this.actionName = "LoadAction";
	}
}
