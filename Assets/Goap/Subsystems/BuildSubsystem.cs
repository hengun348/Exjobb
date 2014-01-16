using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildSubsystem: MonoBehaviour{
		
	Vector3 moveToPosition;
	Component agentComponent;
	GameObject agentObject;
	bool actionIsDone;
	List<string> facts;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent");
		facts = ((Agent)agentComponent).GetSubsystemFacts();
	}
	
	IEnumerator Start(){
		((AIPath)(agentObject.GetComponent("AIPath"))).canMove = true;
		((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = true;
		moveToPosition = BlackBoard.Instance.GetTaskTree().GetOwnedNode(((Agent)agentComponent).getAgentNumber()).GetPosition(); 
		actionIsDone = false;
		yield return StartCoroutine(Build());
		Debug.Log("Nu är subrutinen klar, och byggsubsystem tas bort");
		Destroy(gameObject);
	}
	
	IEnumerator Build(){
		while(actionIsDone == false){	
			
			((Agent)agentComponent).getTarget().transform.position = moveToPosition;
			((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent)agentComponent).getTarget().transform;
			
			//Debug.Log ("**DISTANCE_BUILDER****" + Vector3.Distance(moveToPosition, agentObject.transform.position));
			//if((Mathf.Abs(moveToPosition.x - agentObject.transform.position.x) <= 2) && (Mathf.Abs(moveToPosition.z - agentObject.transform.position.z) <= 2)) //Need to check if there are actions not done in the helplist which this agent have post, if it is then wait with current action!
			if(Vector3.Distance(moveToPosition, agentObject.transform.position) <= 2)
			{
				((AIPath)(agentObject.GetComponent("AIPath"))).canMove = false;
				((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = false;
				
				bool assistersAreNearby = false;
				bool temp = false;
				List<Agent> assisters =  BlackBoard.Instance.GetTaskTree().GetOwnedNode(((Agent)agentComponent).getAgentNumber()).GetAssisters();
				foreach(Agent assister in assisters)
				{
					//if((Mathf.Abs(moveToPosition.x - assister.transform.position.x) >= 2.0f) || (Mathf.Abs(moveToPosition.z - assister.transform.position.z) >= 2.0f))
					if(Vector3.Distance(moveToPosition, assister.transform.position) >= 2)
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
					
					GameObject house = GameObject.CreatePrimitive(PrimitiveType.Cube);
					house.layer = 9;
					AstarPath.active.UpdateGraphs (house.collider.bounds);

					house.AddComponent<Rigidbody>().useGravity = false;
					house.GetComponent<Rigidbody>().isKinematic = true;
					house.transform.position = moveToPosition;//agentObject.transform.position + (moveDirection * 1.0f); //should be houseposition
					
					//add fact of new recources location if facory is built
					if(facts[1] == "Green" || facts[1] == "Orange" || facts[1] == "Magenta")
					{
						((Agent)agentComponent).GetWMemory().SetFact(facts[1], new WorkingMemoryValue(house.transform.position));
						//BlackBoard.Instance.SetFact(facts[1], new WorkingMemoryValue(house.transform.position));
						
					}
					
					house.renderer.material.color = BlackBoard.Instance.GetColorForObject(facts[1]);
					house.tag = facts[1] + facts[2];
					house.name = facts[1] + facts[2];
					
					Dictionary<string, WorkingMemoryValue> houseInfo = new Dictionary<string, WorkingMemoryValue>();
					houseInfo.Add ("Type", new WorkingMemoryValue(facts[1] + facts[2]));
					houseInfo.Add ("Position", new WorkingMemoryValue(house.transform.position));
					houseInfo.Add ("Length", new WorkingMemoryValue(1.0f));
					//houseInfo.Add ("Width", new WorkingMemoryValue(1.0f));
					houseInfo.Add ("Health", new WorkingMemoryValue(300.0f));
					
					((Agent)agentObject.GetComponent("Agent")).GetWMemory().SetFact("Buildings", new WorkingMemoryValue(houseInfo));
					//-----------------------------------------------------
	
					
					
					((Agent)agentComponent).SetSkillpoints("build");
					((Agent)agentComponent).RemoveEnergy();
					
					Debug.Log("*" + facts[1] + facts[2] + "-IS-BUILT*");
					actionIsDone = true;
				}
			}
			yield return null;
		}
	}
}