using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceSensor: MonoBehaviour {
	GameObject agentObject; //the agent gameObject
	Agent agentComponent;	//the agent script
	public List<Collider> colliderList; //a list of all objects the sensors collide with
	
	void Start(){
		agentObject = gameObject.transform.parent.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		
		colliderList = new List<Collider>();
		SphereCollider sensor = gameObject.AddComponent<SphereCollider>(); //the sensor object
		sensor.radius = 5;
		sensor.isTrigger = true;
	}
	
	public void OnTriggerEnter(Collider other) {
		colliderList.Add(other); //add detected object to list of "seen" objects
		if (other.gameObject.name == "Resource(Clone)") { //if it is a resource
			Debug.Log("Found " + other.tag + "!"); 
			
			//get color of resource
			string color = "";
			for(int i = 1; i < other.tag.Length; i++)
			{
				if(char.IsUpper(other.tag[i]))
				{
					color = other.tag.Substring(0, i);
					break;
				}
			}
			
			//update the workingmemory that a resource is at this position
			((Agent)agentComponent).GetWMemory().SetFact(color, new WorkingMemoryValue(other.transform.position)); 
			
			//update the currentWorldState that resrouces of this color is available 
			WorldState newState = new WorldState();
			newState.SetProperty(char.ToLower(color[0]) + color.Substring(1) + "ResourceIsAvailable", new WorldStateValue(true));
			BlackBoard.Instance.SetCurrentWorldstate(agentComponent.GetClan(), newState);
		}
	}
	
	void OnTriggerExit(Collider other) //remove an object from the list of "seen" objects if it is outside the sensor
	{
		colliderList.Remove(other);
	}
	
	
	void Update()
	{
		//clean up the collider list of null objects
		List<Collider> tempList = new List<Collider>();
		foreach(Collider collider in colliderList)
		{
			if(collider != null)
			{
				tempList.Add(collider);
			}
		}
		colliderList = tempList;
	}
}