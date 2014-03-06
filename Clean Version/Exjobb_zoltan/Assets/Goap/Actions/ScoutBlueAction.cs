using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoutBlueAction : Action {
	
	public ScoutBlueAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("blueResourceIsAvailable", new WorldStateValue(false));
				
		postConditions = new WorldState();
		postConditions.SetProperty("blueResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue");
		
		this.actionName = "ScoutBlueAction";
		
		
	}
}
