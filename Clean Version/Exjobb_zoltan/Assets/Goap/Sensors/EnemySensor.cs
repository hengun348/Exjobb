using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySensor: MonoBehaviour {
	GameObject agentObject; //the agent gameObject
	Component agentComponent; //the agent script
	string clan;
	
	void Start(){
		agentObject = gameObject.transform.parent.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent");
		
		SphereCollider sensor = gameObject.AddComponent<SphereCollider>(); //the sensor object
		sensor.radius = 7;
		sensor.isTrigger = true;
		clan = ((Agent)agentComponent).GetClan();
	
	}
	
	void OnTriggerEnter(Collider other) { 
		if(other.gameObject.tag == "Enemy") //if an enemy is detected
		{

			Debug.Log ("ENEMY!!!!");
			
			//Update the current worldstate that an enemy is visible 
			WorldState currentWorldState = (WorldState)BlackBoard.Instance.GetFact(clan, "currentWorldState")[0].GetFactValue();
			currentWorldState.SetProperty("enemyVisible", new WorldStateValue(true));
			
			//add new goal to kill enemy (enemyAlive = false)
			WorldState goalWorldState = new WorldState();
			goalWorldState.SetProperty("enemyAlive", new WorldStateValue(false));
			
			((Agent)agentComponent).CreateNewPlan(currentWorldState, goalWorldState); //create new plan to respond to the threat
			
		}
	}
}