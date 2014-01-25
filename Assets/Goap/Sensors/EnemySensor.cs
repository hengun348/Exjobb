using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySensor: MonoBehaviour {
	GameObject agentObject;
	Component agentComponent;
	string clan;
	
	void Start(){
		agentObject = gameObject.transform.parent.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent");
		
		SphereCollider sensor = gameObject.AddComponent<SphereCollider>();;
		sensor.radius = 7;
		sensor.isTrigger = true;
		clan = ((Agent)agentComponent).GetClan();
	
	}
	
	void OnTriggerEnter(Collider other) { 
		if(other.gameObject.tag == "Enemy")
		{

			Debug.Log ("ENEMY!!!!");
						
			WorldState currentWorldState = (WorldState)BlackBoard.Instance.GetFact(clan, "currentWorldState")[0].GetFactValue();
			
			currentWorldState.setProperty("enemyVisible", new WorldStateValue(true));
			
			
			WorldState goalWorldState = new WorldState();
			goalWorldState.setProperty("enemyAlive", new WorldStateValue(false));
			
			
			
			((Agent)agentComponent).CreateNewPlan(currentWorldState, goalWorldState);
			

		}
	}
}