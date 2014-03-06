using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;
	Agent agentComponent;
	GameObject agentObject;
	bool collected, actionIsDone; //collected is if the resource has been collected
	List<string> facts; //contains the color of the fact to be collected
	string clan;
	WalkSubsystem walker;
	AudioClip[] getSounds; //the sounds for harvesting resources and deliver resources
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		facts = agentComponent.GetSubsystemFacts();
		clan = agentComponent.GetClan();

		getSounds = new AudioClip[2];
		getSounds[0] = (AudioClip)Resources.Load ("SFX/misc040");
		getSounds[1] = (AudioClip)Resources.Load ("SFX/misc002");
		
	}
	
	IEnumerator Start(){
		
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		//TODO: choose the nearest source to go to
		moveToPosition = (Vector3)agentComponent.GetWMemory().GetFact(facts[1])[0].GetFactValue();
	
		collected = false;
		actionIsDone = false;
		yield return StartCoroutine(FindResource());
		
		Destroy(gameObject);
	}
	
	IEnumerator FindResource(){
		while(actionIsDone == false){	
			if(collected) //if the resource is collected
			{
				Vector3 housePosition = BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.GetAgentNumber()).GetPosition(); //get the position to deliver it to
				walker.StartWalking(housePosition);
				
				if(walker.hasArrived == true) //has arived at building position
				{
					Debug.Log("*DELIVERED-" + facts[1] + "*");
					
					AudioSource.PlayClipAtPoint(getSounds[1], housePosition, 0.5f); //play sound for resource is delivered 
					BlackBoard.Instance.GetTaskTree(clan).RemoveNode(agentComponent); //remove owned node frmo taskTree
					
					collected = false;
					actionIsDone = true;
					
					agentComponent.SetSkillpoints("collect"); //get better at collecting

				}
			} else //its not collected, agent need to go get it at resource location
			{
				walker.StartWalking(moveToPosition);
				
				if(walker.hasArrived == true)
				{
					
					//check if the resource is still att the position said in the blackboard
					List<Collider> tempList = ((ResourceSensor)agentObject.transform.FindChild("Sensors").FindChild("ResourceSensor").GetComponent("ResourceSensor")).colliderList;
					bool resourceIsHere = false;
					foreach(Collider collider in tempList)
					{
						if(collider.gameObject.transform.position == moveToPosition)
						{	
							resourceIsHere = true;
						}
					}
					
					
					if (!resourceIsHere ) //If resource is not here (no amount left) or if its empty (before it has been taken away)
					{
						Debug.Log("HÄR SKULLE DET VARA EN RESOURCE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
						agentComponent.GetWMemory().RemoveFact(facts[1], new WorkingMemoryValue(moveToPosition)); //remove it from the workingMemory
						
						if(agentComponent.GetWMemory().GetFact(facts[1]).Count == 0)
						{
							//No resources of that color exist, so update the worldstate
							WorldState newState = new WorldState();
							newState.SetProperty(char.ToLower(facts[1][0]) + facts[1].Substring(1) + "ResourceIsAvailable", new WorldStateValue(false));

							//Report the new worldstate to the blackboard
							BlackBoard.Instance.SetCurrentWorldstate(agentComponent.GetClan(), newState);
							
							//Create a new plan with the new information
							WorldState tempState = new WorldState();
							tempState.SetProperty(char.ToLower(facts[1][0]) + facts[1].Substring(1) + "ResourceIsAvailable", new WorldStateValue(true));
							agentComponent.CreateNewPlan((WorldState)BlackBoard.Instance.GetFact(agentComponent.GetClan(), "currentWorldState")[0].GetFactValue(), tempState);
						}
						else{
							//There are more resources, choose one of them
							//TODO: choose the nearest source to go to
							moveToPosition = (Vector3)agentComponent.GetWMemory().GetFact(facts[1])[0].GetFactValue();
						}
					}
					else //harvesting resource
					{
						AudioSource.PlayClipAtPoint(getSounds[0], moveToPosition, 0.75f); //play harvesting sound
							
						yield return new WaitForSeconds((100-agentComponent.GetSkill("collectSkillpoints"))/20); //wait a time to harvest depending on the skill
						
						Debug.Log("*GOT-" + facts[1] + "*");
						collected = true;
					}
				}
			}
			
			yield return null;
		}
	}
}