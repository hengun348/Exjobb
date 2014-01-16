using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CommanderBuildPyramidAction : Action {
	
	public CommanderBuildPyramidAction()
	{
		preConditions = new WorldState();
		preConditions.setProperty("houseIsBuilt", new WorldStateValue(true, 15));
		//preConditions.setProperty("house2IsBuilt", new WorldStateValue(true, 8));
		
		//preConditions.setProperty("houseIsBuilt", true);
		
		postConditions = new WorldState();
		postConditions.setProperty("pyramidIsBuilt", new WorldStateValue(true));
		
		cost = 1.0f;
		time = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("CommanderAgent");

		
		this.actionName = "CommanderBuildPyramidAction";
	}
	
	public override bool PreConditionsFulfilled(){
	
		int amountAction = 0;
		
		foreach(KeyValuePair<string, WorldStateValue> pair in preConditions.getProperties())
			{
				for(int j = 0; j < (int)pair.Value.propertyValues["amount"]; j++)
				{
					amountAction = (int)pair.Value.propertyValues["amount"];
				}
			}
		
		
		if(BlackBoard.Instance.GetFact("Building").Count == amountAction)
		{
						
			return true;
			
		} else {
						
			return false;
		}
	}
}
