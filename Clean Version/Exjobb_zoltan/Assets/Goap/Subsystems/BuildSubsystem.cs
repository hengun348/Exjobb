using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildSubsystem: MonoBehaviour{
		
	Vector3 moveToPosition;
	Agent agentComponent;
	GameObject agentObject;
	bool actionIsDone;
	List<string> facts; //contains the type of building and its color
	string clan;
	WalkSubsystem walker;
	AudioClip buildingSounds; //the sound to be played when building
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		facts = agentComponent.GetSubsystemFacts();
		clan = agentComponent.GetClan();
	}
	
	IEnumerator Start(){
		
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.GetAgentNumber()).GetPosition(); 
		
		actionIsDone = false;

		yield return StartCoroutine(Build());
		Debug.Log("Nu är subrutinen klar, och byggsubsystem tas bort");

		Destroy(gameObject);
	}
	
	IEnumerator Build(){
		while(actionIsDone == false){	
			
			walker.StartWalking(moveToPosition);
			
			if(walker.hasArrived == true) //agent has arrived at the position
			{
				
				//check if all assisters are here too
				bool assistersAreNearby = false;
				bool temp = false;
				List<Agent> assisters =  BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.GetAgentNumber()).GetAssisters();
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
				
				if(assistersAreNearby)//all helpers are nearby so we can build the building
				{
					
					buildingSounds = (AudioClip)Resources.Load ("SFX/industry005"); 
					AudioSource.PlayClipAtPoint(buildingSounds, moveToPosition, 0.5f); //play building sound at center of building 
					
					yield return new WaitForSeconds((100-agentComponent.GetSkill("buildSkillpoints"))/10); //wait this amount of time until the buidling is completed (based on skill)
					
					
					//remove the wireframe for the building
					GameObject[] tiles = GameObject.FindGameObjectsWithTag("wireframe");
     
				    foreach (GameObject tile in tiles)
				    {
				    	if(Vector3.Distance(tile.transform.position, moveToPosition) < 0.01f)
						{
								Destroy(tile);
						}
				    }
					
					//add the prefab for the bulding in place of the wireframe
					GameObject building = (GameObject)Instantiate(Resources.Load(("Prefabs/Building"),typeof(GameObject)));	
					building.transform.position = moveToPosition;
					building.transform.parent = GameObject.Find(clan).transform.FindChild("Buildings");
					building.renderer.material.color = BlackBoard.Instance.GetColorForObject(facts[1]);
					building.tag = facts[1] + facts[2];
					building.name = facts[1] + facts[2];
					
					AstarPath.active.UpdateGraphs (building.collider.bounds); //update the gridgraph with the position of the building
					
					
					//if a factory was built add new resource location
					if(facts[1] == "Green" || facts[1] == "Orange" || facts[1] == "Magenta" || facts[1] == "Black")
					{
						//add new resource location to the working memory
						agentComponent.GetWMemory().SetFact(facts[1], new WorkingMemoryValue(building.transform.position));
						
						WorldState newState = new WorldState();
						newState.SetProperty(char.ToLower(facts[1][0]) + facts[1].Substring(1) + "ResourceIsAvailable", new WorldStateValue(true));

						//Report the new worldstate (new resource is available) to the blackboard
						BlackBoard.Instance.SetCurrentWorldstate(agentComponent.GetClan(), newState);
						
					}
					
					//update the score for the clan
					BlackBoard.Instance.UpdateScore(clan, facts[1] + " " + facts[2]);

					//update the skill for the agent					
					agentComponent.SetSkillpoints("build");
					
					Debug.Log("*" + facts[1] + facts[2] + "-IS-BUILT*");
					BlackBoard.Instance.GetTaskTree(clan).RemoveNode(agentComponent); //action is done so remove this node from the tasktree
					
					BlackBoard.Instance.IncreasePopulationCap(clan); //increase the population cap for the clan
					
					actionIsDone = true;
				}
			}
			yield return null;
		}
	}
}