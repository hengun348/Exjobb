using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildBlackTowerAction : Action {
	
	public BuildBlackTowerAction()
	{
		preConditions = new WorldState();
		preConditions.SetProperty("greenResourceIsCollected", new WorldStateValue(true));
		preConditions.SetProperty("orangeResourceIsCollected", new WorldStateValue(true));
		preConditions.SetProperty("magentaResourceIsCollected", new WorldStateValue(true));
		
		postConditions = new WorldState();
		postConditions.SetProperty("blackTowerIsBuilt", new WorldStateValue(true));
		postConditions.SetProperty("greenResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("orangeResourceIsCollected", new WorldStateValue(false));
		postConditions.SetProperty("magentaResourceIsCollected", new WorldStateValue(false));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("Yellow&Red&Blue"); //A yellow, red and blue agent has to cooperate to build the black tower

		this.actionName = "BuildBlackTowerAction";
		
		
	}
}
