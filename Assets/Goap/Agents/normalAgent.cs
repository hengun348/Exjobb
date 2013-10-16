using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class normalAgent: Agent {
	
	public normalAgent () 
	{
		this.agentType = "normal";
		
		availableActions = new List<Action>();
		
		foreach( Action action in ActionManager.Instance.actionsList )
		{
			
			//if this agent can do the action, add to list
			if(action.getAgentType().Contains(this.agentType))
			{
				
				availableActions.Add(action);
				
			}
			
		}
		
		
		
		
		
		
		
	/*	wMemory = new WorkingMemory();
		planner = new Planner();
		bBoard = new BlackBoard();
		
		
		
		//---------------Working memory
		wMemory.setFact(WorldState.getProperties());
	*/	
	}
	
}