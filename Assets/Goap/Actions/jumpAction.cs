using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class jumpAction : Action {
	
	public jumpAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("needToJump", true);
		
		postConditions = new WorldState();
		postConditions.setProperty("hasJumped", true);
		
		cost = 4.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("normal");
		
		this.actionName = "jumpAction";

	}
	
}
