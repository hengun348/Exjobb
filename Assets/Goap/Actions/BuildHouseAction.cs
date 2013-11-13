using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildHouseAction : Action {
	
	public BuildHouseAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("stoneIsAvailable", new WorldStateValue(true));
		preConditions.setProperty("woodIsAvailable", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("houseIsBuilt", new WorldStateValue(true));
		postConditions.setProperty("stoneIsAvailable", new WorldStateValue(false));
		postConditions.setProperty("woodIsAvailable", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("commanderAgent");
		agentTypes.Add("SpecialAgent");
		
		this.actionName = "BuildHouseAction";
		
		
	}
}
