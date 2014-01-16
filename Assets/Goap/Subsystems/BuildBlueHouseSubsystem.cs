using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildBlueHouseSubsystem: MonoBehaviour{
		
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
		
		yield return StartCoroutine(BuildBlue());
		Debug.Log("Nu är subrutinen klar, och byggblåsubsystem tas bort");
		Destroy(gameObject);
	}
	
	IEnumerator BuildBlue(){
		while(actionIsDone == false){	
			
			((Agent2)agentComponent).getTarget().transform.position = moveToPosition;
			((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent2)agentComponent).getTarget().transform;
			
			
			if((Mathf.Abs(moveToPosition.x - agentObject.transform.position.x) <= 2) &&  (Mathf.Abs(moveToPosition.z - agentObject.transform.position.z) <= 2)) //Need to check if there are actions not done in the helplist which this agent have post, if it is then wait with current action!
			{
				GameObject house = GameObject.CreatePrimitive(PrimitiveType.Cube);
			
			
				house.AddComponent<Rigidbody>().useGravity = false;
				house.GetComponent<Rigidbody>().isKinematic = true;
				house.transform.position = agentObject.transform.position + (moveDirection * 1.0f); //should be houseposition
				house.renderer.material.color = Color.blue;
				house.tag = "BlueHouse";
				house.name = "BlueHouse";
				
				Dictionary<string, WorkingMemoryValue> houseInfo = new Dictionary<string, WorkingMemoryValue>();
				houseInfo.Add ("Type", new WorkingMemoryValue("BlueHouse"));
				houseInfo.Add ("Position", new WorkingMemoryValue(house.transform.position));
				houseInfo.Add ("Length", new WorkingMemoryValue(1.0f));
				//houseInfo.Add ("Width", new WorkingMemoryValue(1.0f));
				houseInfo.Add ("Health", new WorkingMemoryValue(300.0f));
				
				((Agent2)agentObject.GetComponent("Agent2")).getWMemory().setFact("Buildings", new WorkingMemoryValue(houseInfo));
				//-----------------------------------------------------

				((AIPath)(agentObject.GetComponent("AIPath"))).canMove = false;
				((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = false;
				
				Debug.Log("*BLUEHOUSE-IS-BUILT*");
				actionIsDone = true;
			}
			yield return null;
		}
	}
}