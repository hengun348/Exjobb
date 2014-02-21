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
	AudioClip buildingSounds;
	
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
					Building building = (Building)Instantiate(Resources.Load(("Prefabs/Building"),typeof(Building)));
					
					buildingSounds = (AudioClip)Resources.Load ("SFX/industry005");
					AudioSource.PlayClipAtPoint(buildingSounds, building.transform.position, 0.5f);
					
					yield return new WaitForSeconds((100-agentComponent.GetSkill("buildSkillpoints"))/10);
					
					GameObject[] tiles = GameObject.FindGameObjectsWithTag("wireframe");
     
				    foreach (GameObject tile in tiles)
				    {
				    	if(tile.transform.position == moveToPosition)
							Destroy(tile);
				    }
					
					
				
					
					
					
					if(facts[2] != "Floor")//Build house
					{
					
						building.transform.position = moveToPosition;
						building.transform.parent = GameObject.Find(clan).transform.FindChild("Buildings");
						building.renderer.material.color = BlackBoard.Instance.GetColorForObject(facts[1]);
						building.tag = facts[1] + facts[2];
						building.name = facts[1] + facts[2];
						
						AstarPath.active.UpdateGraphs (building.collider.bounds);
			
						
						//if a factory was built add new resource location
						if(facts[1] == "Green" || facts[1] == "Orange" || facts[1] == "Magenta")
						{
							//TODO: ta bort denna och hämta information ifrån buildings sen i get-subsystem
							agentComponent.GetWMemory().SetFact(facts[1], new WorkingMemoryValue(building.transform.position));
							
						}
						
						BlackBoard.Instance.UpdateScore(clan, facts[1] + " " + facts[2]);
						
					} else { //adding a floor to an excisting house
						
						foreach(Transform buildingItem in GameObject.Find(clan).transform.FindChild("Buildings")) //TODO: Use sensor instead of loop?
						{
							Debug.Log ("88888888888888888 " + buildingItem.position + " == " + moveToPosition);
							
							
							
							if(buildingItem.position == moveToPosition)	
							{
								int numberFloors = buildingItem.childCount + 1;
								building.transform.position = moveToPosition + new Vector3(0, 1.0f, 0) * numberFloors;
								building.transform.parent = buildingItem;
								building.renderer.material.color = BlackBoard.Instance.GetColorForObject(facts[1]);
								building.tag = "HouseFloor";
								building.name = facts[1] + "Floor";
								
								break;
							}
							
						}
						Debug.Log("NU ANROPAR VI SUPREMELEADERN");
						if(clan == "Blue Clan")
						{
							((SupremeCommanderBlueClan)GameObject.Find(clan).transform.FindChild("Commanders").FindChild("SupremeCommanderBlueClan(Clone)").GetComponent("SupremeCommanderBlueClan")).AddNewGoals();
						
						} else if(clan == "Red Clan")
						{
							((SupremeCommanderRedClan)GameObject.Find(clan).transform.FindChild("Commanders").FindChild("SupremeCommanderRedClan(Clone)").GetComponent("SupremeCommanderRedClan")).AddNewGoals();
						
						} else if(clan == "Yellow Clan")
						{
							((SupremeCommanderYellowClan)GameObject.Find(clan).transform.FindChild("Commanders").FindChild("SupremeCommanderYellowClan(Clone)").GetComponent("SupremeCommanderYellowClan")).AddNewGoals();
						}
					}
					
					
					agentComponent.SetSkillpoints("build");
					agentComponent.RemoveEnergy();
					
					Debug.Log("*" + facts[1] + facts[2] + "-IS-BUILT*");
					BlackBoard.Instance.GetTaskTree(clan).RemoveNode(agentComponent);
					
					BlackBoard.Instance.IncreasePopulationCap(clan);
					
					actionIsDone = true;
				}
			}
			yield return null;
		}
	}
}