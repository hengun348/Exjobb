using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IdleSubsystem : MonoBehaviour {	
	Vector3 moveToPosition;
	GameObject agentObject;
	WalkSubsystem walker;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
	}
	
	void Start(){
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = agentObject.transform.position; 
		walker.StartWalking(moveToPosition);
	}
}
