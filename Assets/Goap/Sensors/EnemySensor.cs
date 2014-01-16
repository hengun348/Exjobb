using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySensor: MonoBehaviour {
	GameObject agentObject;
	Component agentComponent;
	
	void Start(){
		agentObject = gameObject.transform.parent.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent");
		
		SphereCollider sensor = gameObject.AddComponent<SphereCollider>();;
		sensor.radius = 7;
		sensor.isTrigger = true;
	}
	
	void OnTriggerEnter(Collider other) { 
		if(other.gameObject.tag == "Enemy")
		{
			
			
			
			Debug.Log ("ENEMY!!!!");
						
			WorldState currentWorldState = (WorldState)BlackBoard.Instance.GetFact("currentWorldState")[0].GetFactValue();
			
			currentWorldState.setProperty("enemyVisible", new WorldStateValue(true));
			
			
			WorldState goalWorldState = new WorldState();
			goalWorldState.setProperty("enemyAlive", new WorldStateValue(false));
			
			
			
			((Agent)agentComponent).CreateNewPlan(currentWorldState, goalWorldState);
			

		}
	}
}