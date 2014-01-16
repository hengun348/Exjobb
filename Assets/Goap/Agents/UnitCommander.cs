using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCommander: MonoBehaviour
{
	GameObject agentGroup;
	
	void Awake()
	{
		
		agentGroup = new GameObject();	
		agentGroup.name = "Agents";
		agentGroup.transform.position = new Vector3(0, 1, 0);
		for(int i = 0; i < 2; i++)
		{
			GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			child.transform.position = new Vector3(i*2, 1, i*2);
			child.transform.parent = agentGroup.transform;
			child.tag = child.name = "Blue";
			child.AddComponent("Agent");
			child.AddComponent<CharacterController>();
			child.AddComponent<CapsuleCollider>().height = 3;
			child.GetComponent<CapsuleCollider>().radius = 1;
			Destroy(child.GetComponent<SphereCollider>());
			child.AddComponent<Rigidbody>().isKinematic = true;
			child.GetComponent<Rigidbody>().useGravity = true;
			child.AddComponent("DynamicGridObstacle");
			child.AddComponent<GraphUpdateScene>().updatePhysics = true;
			child.layer = 9;
			
			BlackBoard.Instance.SetFact("Agents", new WorkingMemoryValue(child));
		}
		
		
		for(int i = 0; i < 2; i++)
		{
			GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			child.transform.position = new Vector3(i*2, 1, i*2);
			child.transform.parent = agentGroup.transform;
			child.tag = child.name = "Red";
			child.AddComponent("Agent");
			child.AddComponent<CharacterController>();
			child.AddComponent<CapsuleCollider>().height = 3;
			child.GetComponent<CapsuleCollider>().radius = 1;
			Destroy(child.GetComponent<SphereCollider>());
			child.AddComponent<Rigidbody>().isKinematic = true;
			child.GetComponent<Rigidbody>().useGravity = true;
			child.AddComponent("DynamicGridObstacle");
			child.AddComponent<GraphUpdateScene>().updatePhysics = true;
			child.layer = 9;
			
			BlackBoard.Instance.SetFact("Agents", new WorkingMemoryValue(child));
		}
		
		
		for(int i = 0; i < 2; i++)
		{
			GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			child.transform.position = new Vector3(i*2, 1, i*2);
			child.transform.parent = agentGroup.transform;
			child.tag = child.name = "Yellow";
			child.AddComponent("Agent");
			child.AddComponent<CharacterController>();
			child.AddComponent<CapsuleCollider>().height = 3;
			child.GetComponent<CapsuleCollider>().radius = 1;
			Destroy(child.GetComponent<SphereCollider>());
			child.AddComponent<Rigidbody>().isKinematic = true;
			child.GetComponent<Rigidbody>().useGravity = true;
			child.AddComponent("DynamicGridObstacle");
			child.AddComponent<GraphUpdateScene>().updatePhysics = true;
			child.layer = 9;
			
			BlackBoard.Instance.SetFact("Agents", new WorkingMemoryValue(child));
		}
	}
	
	void Start()
	{	
	}
	
	void Update()
	{
		
		//Uppdatera agenternas positioner i gridgraphen
		foreach(WorkingMemoryValue val in BlackBoard.Instance.GetFact("Agents"))
		{
			
			AstarPath.active.UpdateGraphs (((GameObject)val.GetFactValue()).collider.bounds);
		}
		
	}
}
