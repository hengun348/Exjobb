using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkSubsystem : MonoBehaviour{
	
	public bool hasArrived;
	Vector3 moveToPosition;
	Agent agentComponent;
	AIPath walker;
	GameObject agentObject;
	
	void Awake()
	{
		agentObject = gameObject.transform.parent.gameObject;
		walker = (AIPath)agentObject.GetComponent("AIPath");
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		//moveToPosition = new Vector3();
		hasArrived = false;
	}
	
	public void StartWalking(Vector3 moveToPosition) //TODO: kontrollera att man itne går utanför griden
	{
		hasArrived = false;
		walker.canMove = true;
		walker.canSearch = true;
		agentComponent.getTarget().transform.position = moveToPosition;
		walker.target = agentComponent.getTarget().transform;
		if(Vector3.Distance(agentObject.transform.position, moveToPosition) < 2.0f)
		{
			walker.canMove = false;
			walker.canSearch = false;
			hasArrived = true;
		}
	}
}