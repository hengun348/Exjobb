using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action {
	
	protected WorldState preConditions;
	protected WorldState postConditions;
	protected float cost;
	protected List<string> agentType;
	
	public List<string> getAgentType()
	{
		
		return agentType;
		
	}

}