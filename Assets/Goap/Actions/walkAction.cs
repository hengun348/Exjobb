using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class walkAction : Action {
	
	public walkAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("hasDestination", true);
		
		postConditions = new WorldState();
		postConditions.setProperty("reachedDestination", true);
		
		cost = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("normal");
		
		this.actionName = "walkAction";
		
	}
	
}
