using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceSensor: MonoBehaviour {
	GameObject agentObject;
	Component agentComponent;
	
	void Start(){
		agentObject = gameObject.transform.parent.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent");
		
		SphereCollider sensor = gameObject.AddComponent<SphereCollider>();;
		sensor.radius = 5;
		sensor.isTrigger = true;
	}
	
	void OnTriggerEnter(Collider other) { 
		if (other.gameObject.tag == "BlueSource") {
			//Debug.Log("Yay!"); 
			((Agent)agentComponent).GetWMemory().SetFact("Blue", new WorkingMemoryValue(other.transform.position)); 
		} else if(other.gameObject.tag == "RedSource"){
		
			((Agent)agentComponent).GetWMemory().SetFact("Red", new WorkingMemoryValue(other.transform.position)); 
		} else if(other.gameObject.tag == "YellowSource"){
		
			((Agent)agentComponent).GetWMemory().SetFact("Yellow", new WorkingMemoryValue(other.transform.position)); 
		}
	}
}