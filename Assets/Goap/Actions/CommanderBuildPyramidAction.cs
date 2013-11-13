using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CommanderBuildPyramidAction : Action {
	
	public CommanderBuildPyramidAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("houseIsBuilt", new WorldStateValue(true, 10));
		
		//preConditions.setProperty("houseIsBuilt", true);
		
		postConditions = new WorldState();
		postConditions.setProperty("pyramidIsBuilt", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("commanderAgent");
		
		this.actionName = "CommanderBuildPyramidAction";
	}
}
