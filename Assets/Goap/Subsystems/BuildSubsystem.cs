using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildSubsystem: MonoBehaviour{
		
	Vector3 moveToPosition;
	Agent agentComponent;
	GameObject agentObject;
	bool actionIsDone;
	List<string> facts;
	string clan;
	WalkSubsystem walker;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		facts = agentComponent.GetSubsystemFacts();
		clan = agentComponent.GetClan();
	}
	
	IEnumerator Start(){
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.getAgentNumber()).GetPosition(); 
		actionIsDone = false;
		yield return StartCoroutine(Build());
		Debug.Log("Nu är subrutinen klar, och byggsubsystem tas bort");
		Destroy(gameObject);
	}
	
	IEnumerator Build(){
		while(actionIsDone == false){	
			
			walker.StartWalking(moveToPosition);
			
			if(walker.hasArrived == true)
			{
				bool assistersAreNearby = false;
				bool temp = false;
				List<Agent> assisters =  BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.getAgentNumber()).GetAssisters();
				foreach(Agent assister in assisters)
				{
					if(Vector3.Distance(moveToPosition, assister.transform.position) >= 3.0f)
					{
						temp = true;
					}
				}
				
				if(temp == false)
				{
					assistersAreNearby = true;
				}
				
				if(assistersAreNearby)//all helpers are nearby
				{
					GameObject[] tiles = GameObject.FindGameObjectsWithTag("wireframe");
     
				    foreach (GameObject tile in tiles)
				    {
				    	if(tile.transform.position == moveToPosition)
							Destroy(tile);
				    }
					
					GameObject house = GameObject.CreatePrimitive(PrimitiveType.Cube); //TODO: Create prefab
					house.layer = 9;
					house.AddComponent<Rigidbody>().useGravity = false;
					house.GetComponent<Rigidbody>().isKinematic = true;
					house.transform.position = moveToPosition;
					house.renderer.material.color = BlackBoard.Instance.GetColorForObject(facts[1]);
					house.tag = facts[1] + facts[2];
					house.name = facts[1] + facts[2];
					AstarPath.active.UpdateGraphs (house.collider.bounds);
					
					
					
					//if(facts[1] == "Green" || facts[1] == "Orange" || facts[1] == "Magenta")
					//{
						//TODO: ta bort denna och hämta information ifrån buildings sen i get-subsystem
						agentComponent.GetWMemory().SetFact(facts[1], new WorkingMemoryValue(house.transform.position));
						BlackBoard.Instance.UpdateScore(clan, facts[1] + " " + facts[2]);
					//}
					//*************************************************************************
					
					
					agentComponent.SetSkillpoints("build");
					agentComponent.RemoveEnergy();
					
					Debug.Log("*" + facts[1] + facts[2] + "-IS-BUILT*");
					actionIsDone = true;
				}
			}
			yield return null;
		}
	}
}