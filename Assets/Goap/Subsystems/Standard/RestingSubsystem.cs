using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RestingSubsystem : MonoBehaviour {	
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
		moveToPosition = new Vector3(30, 0, 30);
		walker.StartWalking(moveToPosition);
	}
	
	void Update()
	{
		if(agent.GetEnergy() >= 100)
		{
			BlackBoard.Instance.GetTaskTree(agent.GetClan()).RemoveNode(agent);
			Destroy(gameObject);
		}
	}
}
