using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MigrateSubsystem : MonoBehaviour{
	
	Vector3 moveToPosition;
	GameObject agentObject;
	bool actionIsDone;
	WalkSubsystem walker;
	Agent agentComponent;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
	}
	
	IEnumerator Start(){
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = transform.parent.parent.parent.parent.FindChild("Commanders").position; // new Vector3(0,0,0);//BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.getAgentNumber()).GetPosition(); 
		actionIsDone = false;
		yield return StartCoroutine(Migrate());
		Debug.Log("Nu är subrutinen klar, och migratesubsystem tas bort");
		Destroy(gameObject);
	}
	
	IEnumerator Migrate(){
		while(actionIsDone == false){	
			walker.StartWalking(moveToPosition);
			
			if(walker.hasArrived == true)
			{
				//agentComponent.RemoveEnergy();
				actionIsDone = true;
			}
			yield return null;
		}
		
		BlackBoard.Instance.GetTaskTree(agentComponent.GetClan()).RemoveNode(agentComponent);
	}
}