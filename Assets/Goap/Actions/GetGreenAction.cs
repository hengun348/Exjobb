using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetGreenAction : Action {
	
	public GetGreenAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("greenResourceIsAvailable", new WorldStateValue(true));
				
		postConditions = new WorldState();
		postConditions.setProperty("greenResourceIsCollected", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue");
		agentTypes.Add("Yellow");
		
		this.actionName = "GetGreenAction";
		
		
	}
}
