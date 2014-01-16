using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildBlackTowerAction : Action {
	
	public BuildBlackTowerAction()
	{
		preConditions = new WorldState();
		//preConditions.setProperty("greenResourceIsCollected", new WorldStateValue(true));
		preConditions.setProperty("orangeResourceIsCollected", new WorldStateValue(true));
		//preConditions.setProperty("magentaResourceIsCollected", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.setProperty("blackTowerIsBuilt", new WorldStateValue(true));
		//postConditions.setProperty("greenResourceIsCollected", new WorldStateValue(false));
		postConditions.setProperty("orangeResourceIsCollected", new WorldStateValue(false));
		//postConditions.setProperty("magentaResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		//agentTypes.Add("Yellow&Red&Blue");
		//agentTypes.Add("Yellow&Red");
		agentTypes.Add("Yellow");
		
		
		this.actionName = "BuildBlackTowerAction";
		
		
	}
}
