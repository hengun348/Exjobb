using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetWoodAction : Action {
	
	public GetWoodAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("woodIsAvailable", new WorldStateValue(false));
		
		postConditions = new WorldState();
		postConditions.setProperty("woodIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("SpecialAgent");
		
		this.actionName = "GetWoodAction";
		
		
	}
}
