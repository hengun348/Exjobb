using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent {
	
	/*public float health;
	public Vector3 position;
	public Vector3 facing;
	public WorkingMemory wMemory;
	public Planner planner;
	public BlackBoard bBoard;
	*/
	
	protected List<Action> availableActions;
	protected WorldState wState;
	protected string agentType { get; set; }
	
	/*public bool hasAction(Action action)
	{
		
		if(availableActions.Contains(action))
		{
			
			return true;
			
		}
		else
		{
			
			return false;
			
		}
		
	}*/
	
}