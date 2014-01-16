using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssistingSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;
	Component agentComponent;
	GameObject agentObject;
	bool actionIsDone;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent");
	}
	
	IEnumerator Start(){
		((AIPath)(agentObject.GetComponent("AIPath"))).canMove = true;
		((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = true;
		moveToPosition = BlackBoard.Instance.GetTaskTree().GetOwnedNode(((Agent)agentComponent).getAgentNumber()).GetPosition(); 
		actionIsDone = false;
		
		yield return StartCoroutine(Assist());
		Debug.Log("Nu är subrutinen klar, och assistsubsystem tas bort");
		Destroy(gameObject);
	}
	
	IEnumerator Assist(){
		while(actionIsDone == false){	

			((Agent)agentComponent).getTarget().transform.position = moveToPosition;
			((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent)agentComponent).getTarget().transform;
			
			
			//if((Mathf.Abs(moveToPosition.x - agentObject.transform.position.x) <= 2) &&  (Mathf.Abs(moveToPosition.z - agentObject.transform.position.z) <= 2)) //Need to check if there are actions not done in the helplist which this agent have post, if it is then wait with current action!
			if(Vector3.Distance(moveToPosition, agentObject.transform.position) <= 2)
			{
				
				((AIPath)(agentObject.GetComponent("AIPath"))).canMove = false;
				((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = false;
			}
			
			if(BlackBoard.Instance.GetTaskTree().GetOwnedNode(((Agent)agentComponent).getAgentNumber()).GetPosition().Equals(new Vector3(30, 0.5f, 30)))
			{
				((Agent)agentComponent).RemoveEnergy();
				actionIsDone = true;
			}
			yield return null;
		}
	}
}