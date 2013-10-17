using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class normalAgent: Agent {
	
	public Planner planner;
	
	public normalAgent () 
	{
		this.agentType = "normal";
		
		availableActions = new List<Action>();
		
		foreach( Action action in ActionManager.Instance.actionsList )
		{
			
			//if this agent can do the action, add to list
			if(action.getAgentTypes().Contains(this.agentType))
			{
				
				availableActions.Add(action);
				
			}
			
		}
		
		planner = new Planner();
		
		
		
		
		
	/*	wMemory = new WorkingMemory();
		
		bBoard = new BlackBoard();

	*/	
	}
	
}