using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoutBlueAction : Action {
	
	public ScoutBlueAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("blueResourceIsAvailable", new WorldStateValue(false));
				
		postConditions = new WorldState();
		postConditions.setProperty("blueResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue");
		//agentTypes.Add("Yellow");
		//agentTypes.Add("Red");
		
		this.actionName = "ScoutBlueAction";
		
		
	}
}
