using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;
	Agent agentComponent;
	GameObject agentObject;
	bool collected, actionIsDone;
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
		moveToPosition = new Vector3(Random.Range(agentObject.transform.position.x - 20.0f, agentObject.transform.position.x + 20.0f), 0, Random.Range(agentObject.transform.position.z - 20.0f, agentObject.transform.position.z + 20.0f));

		collected = false;
		actionIsDone = false;
		yield return StartCoroutine(FindResource());
		Destroy(gameObject);
	}
	
	IEnumerator FindResource(){
		while(actionIsDone == false){	
			if(collected)
			{
				Vector3 housePosition = BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.getAgentNumber()).GetPosition(); 
				walker.StartWalking(housePosition);
				
				if(walker.hasArrived == true)
				{
					collected = false;
					actionIsDone = true;
					Debug.Log("*DELIVERED-" + facts[1] + "*");
					agentComponent.SetSkillpoints("collect");
					agentComponent.RemoveEnergy();
				}
			} else 
			{
				//TODO: Denna information borde plockas ut från buildingposition
				if(agentComponent.GetWMemory().GetFact(facts[1]).Count > 0)
				{
					//TODO: choose the nearest source to go to
					moveToPosition = (Vector3)agentComponent.GetWMemory().GetFact(facts[1])[0].GetFactValue();
					walker.StartWalking(moveToPosition);
					
					if(walker.hasArrived == true)
					{
						collected = true;
						Debug.Log("*GOT-" + facts[1] + "*");
					}
					
				} else
				{
					walker.StartWalking(moveToPosition);
					
					if(walker.hasArrived == true)
					{
						moveToPosition = new Vector3(Random.Range(agentObject.transform.position.x - 10.0f, agentObject.transform.position.x + 10.0f), 1.0f, Random.Range(agentObject.transform.position.z -10.0f, agentObject.transform.position.z + 10.0f));
						walker.StartWalking(moveToPosition);
					}
				}
			}
			yield return null;
		}
	}
}