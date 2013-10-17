using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action {
	
	protected WorldState preConditions;
	protected WorldState postConditions;
	public float cost;
	public string actionName;
	protected List<string> agentTypes;
	
	public List<string> getAgentTypes()
	{
		
		return agentTypes;
		
	}

}