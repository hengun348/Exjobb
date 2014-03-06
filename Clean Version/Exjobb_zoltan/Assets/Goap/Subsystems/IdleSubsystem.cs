using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IdleSubsystem : MonoBehaviour {	
	Vector3 moveToPosition;
	GameObject agentObject;
	WalkSubsystem walker;
	Agent agent;
	int tick; //time counter
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agent = ((Agent)agentObject.GetComponent("Agent"));
	}
	
	void Start(){
		
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = agentObject.transform.position; 
		walker.StartWalking(moveToPosition);
	}
	
	void Update()
	{
		tick ++;
		if(tick == 50) //destroy the idlesubsystem each second (so the agent can check for new actions)
		{
			if(BlackBoard.Instance.GetActionForAgent(agent.GetClan(), agent) != "IdleAction")
			{
				Destroy (gameObject);
			}
			
			tick = 0;
		}
	}
}
