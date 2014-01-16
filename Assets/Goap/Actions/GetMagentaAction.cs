using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetMagentaAction : Action {
	
	public GetMagentaAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("magentaResourceIsAvailable", new WorldStateValue(true));
				
		postConditions = new WorldState();
		postConditions.setProperty("magentaResourceIsCollected", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Blue");
		agentTypes.Add("Red");
		
		this.actionName = "GetMagentaAction";
		
		
	}
}
