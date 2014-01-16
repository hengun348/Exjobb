using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetYellowAction : Action {
	
	public GetYellowAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("yellowResourceIsAvailable", new WorldStateValue(true));
				
		postConditions = new WorldState();
		postConditions.setProperty("yellowResourceIsCollected", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Yellow");
		
		this.actionName = "GetYellowAction";
		
		
	}
}
