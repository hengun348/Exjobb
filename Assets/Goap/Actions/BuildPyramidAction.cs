using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildPyramidAction : Action {
	
	public BuildPyramidAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("houseIsBuilt", new WorldStateValue(true, 3));
		preConditions.setProperty("stoneIsAvailable", new WorldStateValue(true));
		preConditions.setProperty("woodIsAvailable", new WorldStateValue(true));
		
		//preConditions.setProperty("houseIsBuilt", true);
		
		postConditions = new WorldState();
		postConditions.setProperty("pyramidIsBuilt", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("commanderAgent");
		agentTypes.Add("specialAgent");
		
		this.actionName = "BuildPyramidAction";
	}
}
