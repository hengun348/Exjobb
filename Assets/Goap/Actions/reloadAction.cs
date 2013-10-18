using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class reloadAction : Action {
	
	public reloadAction()
	{
		
		preConditions = new WorldState();
		preConditions.setProperty("needToJump", true);
		
		postConditions = new WorldState();
		postConditions.setProperty("hasJumped", true);
		
		cost = 2.0f;
		
		agentTypes = new List<string>();
		agentTypes.Add("normal");
		
		this.actionName = "reloadAction";
		
	}
	
}
