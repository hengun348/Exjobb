using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetOrangeAction : Action {
	
	public GetOrangeAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("orangeResourceIsAvailable", new WorldStateValue(true));
				
		postConditions = new WorldState();
		postConditions.SetProperty("orangeResourceIsCollected", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Red");
		agentTypes.Add("Yellow");
		
		this.actionName = "GetOrangeAction";
		
		
	}
}
