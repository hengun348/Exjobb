using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceSensor: MonoBehaviour {
	GameObject agentObject;
	Agent agentComponent;
	public List<Collider> colliderList; 
	
	void Start(){
		agentObject = gameObject.transform.parent.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		
		colliderList = new List<Collider>();
		SphereCollider sensor = gameObject.AddComponent<SphereCollider>();;
		sensor.radius = 5;
		sensor.isTrigger = true;
	}
	
	public void OnTriggerEnter(Collider other) {
		colliderList.Add(other);
		if (other.gameObject.name == "Resource(Clone)") {
			Debug.Log("Yay!" + other.tag); 
			
			string color = "";
			for(int i = 1; i < other.tag.Length; i++)
			{
				if(char.IsUpper(other.tag[i]))
				{
					color = other.tag.Substring(0, i);
					break;
				}
			}
			
			Debug.Log(color);
			((Agent)agentComponent).GetWMemory().SetFact(color, new WorkingMemoryValue(other.transform.position)); 
			
			WorldState newState = new WorldState();
			newState.setProperty(char.ToLower(color[0]) + color.Substring(1) + "ResourceIsAvailable", new WorldStateValue(true));
			BlackBoard.Instance.SetCurrentWorldstate(agentComponent.GetClan(), newState);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		colliderList.Remove(other);
	}
	
	
	void Update()
	{
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