using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetStoneAction : Action {
	
	public GetStoneAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("stoneIsAvailable", new WorldStateValue(false));

		
		postConditions = new WorldState();
		postConditions.setProperty("stoneIsAvailable", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("SpecialAgent");
		
		this.actionName = "GetStoneAction";
		
		
	}
}
