using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IdleSubsystem : MonoBehaviour {	
	Vector3 moveToPosition;
	GameObject agentObject;
	WalkSubsystem walker;
	Agent agent;
	
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
		if(agent.GetTick() == 49)
		{
			if(BlackBoard.Instance.GetActionForAgent(agent.GetClan(), agent) != "IdleAction")
			{
				Destroy (gameObject);
			}
		}
	}
}
