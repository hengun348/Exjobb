using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssistingSubsystem : MonoBehaviour{
	
	Vector3 moveToPosition;
	Agent agentComponent;
	GameObject agentObject;
	bool actionIsDone;
	string clan;
	WalkSubsystem walker;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		clan = agentComponent.GetClan();
	}
	
	IEnumerator Start(){
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.getAgentNumber()).GetPosition(); 
		actionIsDone = false;
		
		yield return StartCoroutine(Assist());
		Debug.Log("Nu är subrutinen klar, och assistsubsystem tas bort");
		Destroy(gameObject);
	}
	
	IEnumerator Assist(){
		while(actionIsDone == false){	
			walker.StartWalking(moveToPosition);
			
			if(BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.getAgentNumber()).GetPosition().Equals(new Vector3(30, 0.5f, 30)))
			{
				((AIPath)agentObject.GetComponent("AIPath")).canMove = false;
				((AIPath)agentObject.GetComponent("AIPath")).canSearch = false;
				agentComponent.RemoveEnergy();
				actionIsDone = true;
			}
			yield return null;
		}
	}
}