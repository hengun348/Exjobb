using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetBlueSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;
	Component agentComponent;
	GameObject agentObject;
	bool collected, actionIsDone;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent2");
	}
	
	IEnumerator Start(){
		((AIPath)(agentObject.GetComponent("AIPath"))).canMove = true;
		((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = true;
		
		float xpos = Random.Range(-20.0f, 20.0f);
		float zpos = Random.Range(-20.0f, 20.0f);
		moveToPosition = new Vector3(xpos, 0, zpos);
		
		collected = false;
		actionIsDone = false;
		
		yield return StartCoroutine(FindBlue());
		Destroy(gameObject);
	}
	
	IEnumerator FindBlue(){
		while(actionIsDone == false){	
			if(collected)
			{
				Vector3 housePosition = BlackBoard.Instance.getTaskTree().getOwnedNode(((Agent2)agentComponent).getAgentNumber()).getPosition(); 
				((Agent2)agentComponent).getTarget().transform.position = housePosition;
				((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent2)agentComponent).getTarget().transform;
				
				if((Mathf.Abs(housePosition.x - agentObject.transform.position.x) <= 3) &&  (Mathf.Abs(housePosition.z - agentObject.transform.position.z) <= 3))
				{
					collected = false;
					actionIsDone = true;
					
					((AIPath)(agentObject.GetComponent("AIPath"))).canMove = false;
					((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = false;
				}
			} else 
			{
				if(((Agent2)agentComponent).getWMemory().getFact("Blue").Count > 0)
				{
					moveToPosition = (Vector3)((Agent2)agentComponent.GetComponent("Agent2")).getWMemory().getFact("Blue")[0].getFactValue();
					((Agent2)agentComponent).getTarget().transform.position = moveToPosition;
					((AIPath)(agentComponent.GetComponent("AIPath"))).target = ((Agent2)agentComponent).getTarget().transform;
					
					if((Mathf.Abs(moveToPosition.x - agentObject.transform.position.x) <= 3) &&  (Mathf.Abs(moveToPosition.z - agentObject.transform.position.z) <= 3))
					{
							
						collected = true;
						Debug.Log("*GOT-BLUE*");
					}
				} else
				{
					((Agent2)agentComponent).getTarget().transform.position = moveToPosition;
					((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent2)agentComponent).getTarget().transform;

					if((Mathf.Abs(moveToPosition.x - agentObject.transform.position.x) <= 3) &&  (Mathf.Abs(moveToPosition.z - agentObject.transform.position.z) <= 3))
					{
						moveToPosition = agentObject.transform.TransformDirection(new Vector3(Random.Range(-10.0f, 10.0f), -2.0f, Random.Range(-10.0f, 10.0f)) - agentObject.transform.position);
					}
					
				}
			}
			yield return null;
		}
	}
}