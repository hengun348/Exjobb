using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoutYellowAction : Action {
	
	public ScoutYellowAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("yellowResourceIsAvailable", new WorldStateValue(false));
				
		postConditions = new WorldState();
		postConditions.setProperty("yellowResourceIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		//agentTypes.Add("Blue");
		//agentTypes.Add("Red");
		agentTypes.Add("Yellow");
		
		this.actionName = "ScoutYellowAction";
		
		
	}
}
