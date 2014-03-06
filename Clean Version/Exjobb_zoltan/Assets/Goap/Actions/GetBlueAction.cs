using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetBlueAction : Action {
	
	public GetBlueAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("blueResourceIsAvailable", new WorldStateValue(true));
				
		postConditions = new WorldState();
		postConditions.SetProperty("blueResourceIsCollected", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue");
		
		this.actionName = "GetBlueAction";

	}
}
