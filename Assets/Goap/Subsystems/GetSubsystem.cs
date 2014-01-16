using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;
	Component agentComponent;
	GameObject agentObject;
	bool collected, actionIsDone;
	List<string> facts;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = agentObject.GetComponent("Agent");
		facts = ((Agent)agentComponent).GetSubsystemFacts();
		
	}
	
	
	IEnumerator Start(){
		((AIPath)(agentObject.GetComponent("AIPath"))).canMove = true;
		((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = true;
		
		float xpos = Random.Range(-20.0f, 20.0f);
		float zpos = Random.Range(-20.0f, 20.0f);
		moveToPosition = new Vector3(xpos, 0, zpos);
		
		collected = false;
		actionIsDone = false;
		yield return StartCoroutine(FindResource());
		Destroy(gameObject);
	}
	
	IEnumerator FindResource(){
		while(actionIsDone == false){	
			if(collected)
			{
				Vector3 housePosition = BlackBoard.Instance.GetTaskTree().GetOwnedNode(((Agent)agentComponent).getAgentNumber()).GetPosition(); 
				((Agent)agentComponent).getTarget().transform.position = housePosition;
				((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent)agentComponent).getTarget().transform;
				
				if((Mathf.Abs(housePosition.x - agentObject.transform.position.x) <= 3) &&  (Mathf.Abs(housePosition.z - agentObject.transform.position.z) <= 3))
				{
					collected = false;
					actionIsDone = true;
					
					Debug.Log("*DELIVERED-" + facts[1] + "*");
					((Agent)agentComponent).SetSkillpoints("collect");
					((Agent)agentComponent).RemoveEnergy();
					((AIPath)(agentObject.GetComponent("AIPath"))).canMove = false;
					((AIPath)(agentObject.GetComponent("AIPath"))).canSearch = false;
				}
			} else 
			{
				//((Agent)agentComponent.GetComponent("Agent")).GetWMemory().PrintWorkingMemory();
				//Debug.Log(((Agent)agentComponent).GetWMemory().GetFact(facts[1])[1].GetFactValue());
				if(((Agent)agentComponent).GetWMemory().GetFact(facts[1]).Count > 0)
				{
					
					//TODO: choose the nearest source to go to
					//Eftersom GetFact returnar en ny tom(eftersom värdet inte finns), så går de alltid in här. FIX!!
					moveToPosition = (Vector3)((Agent)agentComponent.GetComponent("Agent")).GetWMemory().GetFact(facts[1])[0].GetFactValue();
					
					((Agent)agentComponent).getTarget().transform.position = moveToPosition;
					((AIPath)(agentComponent.GetComponent("AIPath"))).target = ((Agent)agentComponent).getTarget().transform;
					
					if((Mathf.Abs(moveToPosition.x - agentObject.transform.position.x) <= 3) &&  (Mathf.Abs(moveToPosition.z - agentObject.transform.position.z) <= 3))
					{
							
						collected = true;
						Debug.Log("*GOT-" + facts[1] + "*");
					}
				} else
				{
					((Agent)agentComponent).getTarget().transform.position = moveToPosition;
					((AIPath)(agentObject.GetComponent("AIPath"))).target = ((Agent)agentComponent).getTarget().transform;

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