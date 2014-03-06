using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssistingSubsystem : MonoBehaviour{
	
	Vector3 moveToPosition; //the position to move to where the action should be carried out
	Agent agentComponent; //the agent script
	GameObject agentObject; //the agent gameObject
	bool actionIsDone; //is the action done or not?
	string clan;
	WalkSubsystem walker; //enables the agent to walk
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		clan = agentComponent.GetClan();
	}
	
	IEnumerator Start(){

		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.GetAgentNumber()).GetPosition(); //get the position from the agents owned node in the tasktree
		actionIsDone = false;
		
		yield return StartCoroutine(Assist()); //start a process that runs parallel to the other processes and wait on this line until the process is done
		Debug.Log("Nu är subrutinen klar, och assistsubsystem tas bort");
		
		Destroy(gameObject); //remove this subsystem
	}
	
	IEnumerator Assist(){
		while(actionIsDone == false){ 	
			walker.StartWalking(moveToPosition); //walk to position
			
			if(BlackBoard.Instance.GetTaskTree(clan).GetOwnedNode(agentComponent.GetAgentNumber()).GetPosition().Equals(new Vector3(30, 0.5f, 30))) //if done with action (no owned node) the we are done with the action
			{
				((AIPath)agentObject.GetComponent("AIPath")).canMove = false; //agent cant move
				((AIPath)agentObject.GetComponent("AIPath")).canSearch = false; //agent cant search
				actionIsDone = true;
			}
			yield return null;
		}
	}
}