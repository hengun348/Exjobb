using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoutSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;
	Agent agentComponent;
	GameObject agentObject;
	List<string> facts;
	string clan;
	WalkSubsystem walker;
	
	void Awake(){
		agentObject = gameObject.transform.parent.gameObject;
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		clan = agentComponent.GetClan();
		facts = agentComponent.GetSubsystemFacts();
	}
	
	IEnumerator Start(){
		walker = (WalkSubsystem)gameObject.GetComponent("WalkSubsystem");
		moveToPosition = GeneratePosition();
		
		yield return StartCoroutine(FindResource());
		Destroy(gameObject);
	}
	
	IEnumerator FindResource(){
		Debug.Log ("Letar efter:  " + facts[1]);
		while(BlackBoard.Instance.GetFact(clan, facts[1]).Count == 0 ){	
			walker.StartWalking(moveToPosition);
			
			if(walker.hasArrived == true)
			{
				moveToPosition = GeneratePosition();
				
				
				walker.StartWalking(moveToPosition);
			}
			
			
			yield return null;
		}
		
		BlackBoard.Instance.GetTaskTree(clan).RemoveNode(agentComponent);
		
		
	}
	
	Vector3 GeneratePosition()
	{
		Vector3 position =  new Vector3(Random.Range(agentObject.transform.position.x - 10.0f, agentObject.transform.position.x + 10.0f), 1.0f, Random.Range(agentObject.transform.position.z -10.0f, agentObject.transform.position.z + 10.0f));;
		/*if(Mathf.Abs(position.x) > AstarPath.active.astarData.gridGraph.size.x/4 || Mathf.Abs(position.z) > AstarPath.active.astarData.gridGraph.size.y/4)
		{
			return GeneratePosition();
		}*/
		
		return position;
	}
}