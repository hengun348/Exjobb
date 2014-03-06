using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoutSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;
	Agent agentComponent;
	GameObject agentObject;
	List<string> facts; //contains what color to scout for
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
		Debug.Log ("Scouting for: " + facts[1]);
		while(BlackBoard.Instance.GetFact(clan, facts[1]).Count == 0 ){	//continue to scout for resource as long as no one has found one
			walker.StartWalking(moveToPosition);
			
			if(walker.hasArrived == true) //has arrived at new searchlocation need a new one
			{
				moveToPosition = GeneratePosition();
				
				walker.StartWalking(moveToPosition);
			}
			
			
			yield return null;
		}
		
		BlackBoard.Instance.GetTaskTree(clan).RemoveNode(agentComponent);
	}
	
	Vector3 GeneratePosition() //generate a new position to walk to for scouting
	{
		Vector3 position =  new Vector3(Random.Range(agentObject.transform.position.x - 10.0f, agentObject.transform.position.x + 10.0f), 1.0f, Random.Range(agentObject.transform.position.z -10.0f, agentObject.transform.position.z + 10.0f));;
		
		//make sure to stay inside the map
		if(Mathf.Abs(position.x) > AstarPath.active.astarData.gridGraph.size.x/4 || Mathf.Abs(position.z) > AstarPath.active.astarData.gridGraph.size.y/4)
		{
			return GeneratePosition();
		}
		
		return position;
	}
}