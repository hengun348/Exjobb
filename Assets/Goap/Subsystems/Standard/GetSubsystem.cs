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
	AudioClip[] getSounds;
	//AudioSource getSound;
	
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
		Debug.Log ("Försöker hitta, i wm, information om: " + facts[1]);
		moveToPosition = (Vector3)agentComponent.GetWMemory().GetFact(facts[1])[0].GetFactValue();
	
		
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
					Debug.Log ("!!!!!!!!!!!!!!!!!!!!!!!!!!! Ska lämna vid " + housePosition);
				walker.StartWalking(housePosition);
				
				if(walker.hasArrived == true)
				{
					Debug.Log("*DELIVERED-" + facts[1] + "*");
					
					AudioSource.PlayClipAtPoint(getSounds[1], housePosition, 0.5f);
					BlackBoard.Instance.GetTaskTree(clan).RemoveNode(agentComponent);
				
					collected = false;
					actionIsDone = true;
					
					agentComponent.SetSkillpoints("collect");
					agentComponent.RemoveEnergy();
				}
			} else 
			{
				walker.StartWalking(moveToPosition);
				
				if(walker.hasArrived == true)
				{
					List<Collider> tempList = ((ResourceSensor)agentObject.transform.FindChild("Sensors").FindChild("ResourceSensor").GetComponent("ResourceSensor")).colliderList;
					bool resourceIsHere = false;
					foreach(Collider collider in tempList)
					{
						if(collider.gameObject.transform.position == moveToPosition)
						{
							resourceIsHere = true;
						}
					}
					
					if (!resourceIsHere)
					{
						Debug.Log("HÄR SKULLE DET VARA EN RESOURCE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
						agentComponent.GetWMemory().RemoveFact(facts[1], new WorkingMemoryValue(moveToPosition));
						
						if(agentComponent.GetWMemory().GetFact(facts[1]).Count == 0)
						{
							//No resources of that color exist, so update the worldstate
							WorldState newState = new WorldState();
							newState.setProperty(char.ToLower(facts[1][0]) + facts[1].Substring(1) + "ResourceIsAvailable", new WorldStateValue(false));

							//Report the new worldstate to the blackboard
							BlackBoard.Instance.SetCurrentWorldstate(agentComponent.GetClan(), newState);
							
							//Create a new plan with the new information
							WorldState tempState = new WorldState();
							tempState.setProperty(char.ToLower(facts[1][0]) + facts[1].Substring(1) + "ResourceIsAvailable", new WorldStateValue(true));
							agentComponent.CreateNewPlan((WorldState)BlackBoard.Instance.GetFact(agentComponent.GetClan(), "currentWorldState")[0].GetFactValue(), tempState);
						}
						else{
							//There are more resources, choose one of them
							//TODO: choose the nearest source to go to
							moveToPosition = (Vector3)agentComponent.GetWMemory().GetFact(facts[1])[0].GetFactValue();
						}
					}
					else
					{
						AudioSource.PlayClipAtPoint(getSounds[0], moveToPosition, 0.75f);
						yield return new WaitForSeconds((100-agentComponent.GetSkill("collectSkillpoints"))/20);
						
						Debug.Log("*GOT-" + facts[1] + "*");
					
					
						foreach(Transform resourceItem in GameObject.Find("Resources").transform) //TODO: Use sensor instead of loop?
						{
							if(resourceItem.position == moveToPosition)	
							{
								//TODO: Implement collectskill instead of 1, check if available resources is takeable (amount left is more or equal to what the agent wants?)
								((ResourceObject)resourceItem.GetComponent("ResourceObject")).UpdateResource(clan, 1/* *collectSkill */);
							}
						}
						collected = true;
					}
				}
			}
			yield return null;
		}
	}
}