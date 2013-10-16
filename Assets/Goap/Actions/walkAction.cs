using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class walkAction : Action {
	
	public walkAction()
	{
		
		this.preConditions = new WorldState();
		this.preConditions.setProperty("hasDestination", new WorldStateValue(true));
		
		this.postConditions = new WorldState();
		this.postConditions.setProperty("reachedDestination", new WorldStateValue(5));
		
		this.cost = 4.0f;
		
		this.agentType.Add("normal");
		
	}
	
}
