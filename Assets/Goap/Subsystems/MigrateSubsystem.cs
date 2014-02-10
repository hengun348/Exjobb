using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MigrateSubsystem : MonoBehaviour{
	
	Vector3 moveToPosition;
	GameObject agentObject;
	bool actionIsDone;
	WalkSubsystem walker;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
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
				((AIPath)agentObject.GetComponent("AIPath")).canMove = false;
				((AIPath)agentObject.GetComponent("AIPath")).canSearch = false;
				//agentComponent.RemoveEnergy();
				actionIsDone = true;
			}
			yield return null;
		}
	}
}