using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildPurpleHouseSubsystem: MonoBehaviour{
		
	Vector3 moveDirection, moveToPosition;
	Component agentComponent;
	GameObject agentObject;
	CharacterController agentController;
	bool actionIsDone;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent2");
	}
	
	IEnumerator Start(){
		((AIPath)(agentObject.GetComponent("AIPath"))).canMove = true;
		((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = true;
		moveToPosition = BlackBoard.Instance.getTaskTree().getOwnedNode(((Agent2)agentComponent).getAgentNumber()).getPosition(); 
		actionIsDone = false;
		
		yield return StartCoroutine(BuildPurple());
		Debug.Log("Nu är subrutinen klar, och byggpurplesubsystem tas bort");
		Destroy(gameObject);
	}
	
	IEnumerator BuildPurple(){
		while(actionIsDone == false){	
			
			((Agent2)agentComponent).getTarget().transform.position = moveToPosition;
			((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent2)agentComponent).getTarget().transform;
			
			
			if((Mathf.Abs(moveToPosition.x - agentObject.transform.position.x) <= 2) && (Mathf.Abs(moveToPosition.z - agentObject.transform.position.z) <= 2)) //Need to check if there are actions not done in the helplist which this agent have post, if it is then wait with current action!
			{
				bool assistersAreNearby = true;
				List<Agent2> assisters =  BlackBoard.Instance.getTaskTree().getOwnedNode(((Agent2)agentComponent).getAgentNumber()).getAssisters();
				foreach(Agent2 assister in assisters)
				{
					if((Mathf.Abs(moveToPosition.x - assister.transform.position.x) >= 2.0f) || (Mathf.Abs(moveToPosition.z - assister.transform.position.z) >= 2.0f))
					{
						assistersAreNearby = false;
					}
				}
				
				if(assistersAreNearby)//all helpers are nearby
				{
					GameObject house = GameObject.CreatePrimitive(PrimitiveType.Cube);
				
					house.AddComponent<Rigidbody>().useGravity = false;
					house.GetComponent<Rigidbody>().isKinematic = true;
					house.transform.position = agentObject.transform.position + (moveDirection * 1.0f); //should be houseposition
					house.renderer.material.color = new Color(0.63f, 0.13f, 0.94f);
					house.tag = "PurpleHouse";
					house.name = "PurpleHouse";
					
					Dictionary<string, WorkingMemoryValue> houseInfo = new Dictionary<string, WorkingMemoryValue>();
					houseInfo.Add ("Type", new WorkingMemoryValue("PurpleHouse"));
					houseInfo.Add ("Position", new WorkingMemoryValue(house.transform.position));
					houseInfo.Add ("Length", new WorkingMemoryValue(1.0f));
					//houseInfo.Add ("Width", new WorkingMemoryValue(1.0f));
					houseInfo.Add ("Health", new WorkingMemoryValue(300.0f));
					
					((Agent2)agentObject.GetComponent("Agent2")).getWMemory().setFact("Buildings", new WorkingMemoryValue(houseInfo));
					//-----------------------------------------------------
	
					((AIPath)(agentObject.GetComponent("AIPath"))).canMove = false;
					((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = false;
					
					Debug.Log("*PURPLEHOUSE-IS-BUILT*");
					actionIsDone = true;
				}
			}
			yield return null;
		}
	}
}