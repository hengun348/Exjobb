using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AgentBuildPyramidAction : Action {
	
	public AgentBuildPyramidAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("stoneIsAvailable", new WorldStateValue(true));
		preConditions.setProperty("woodIsAvailable", new WorldStateValue(true));
		
		//preConditions.setProperty("houseIsBuilt", true);
		
		postConditions = new WorldState();
		postConditions.setProperty("pyramidIsBuilt", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("SpecialAgent");
		
		this.actionName = "AgentBuildPyramidAction";
	}
}
