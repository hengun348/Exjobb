using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetStoneSubsystem: MonoBehaviour{
	
	Vector3 moveToPosition;

	Component agentComponent;
	GameObject agentObject;
	CharacterController agentController;
	
	
	void Awake(){
	
		agentObject = gameObject.transform.parent.gameObject;
		
		agentController = transform.parent.GetComponent<CharacterController>();
		
		
		agentComponent = agentController.GetComponent("Agent");
		
	}
	
	void Start(){

		
			//should be an exploring-action instead
			moveToPosition = new Vector3(Random.Range(-20.0f, 20.0f), -2.0f, Random.Range(-20.0f, 20.0f));
	
	}
	
	void Update(){
		
		
		//if(((SpecialAgent)agentController.GetComponent(agentController.tag)).currentAction == "GetStoneAction"){
		if(((Agent)agentComponent).currentAction == "GetStoneAction") {
			
			((AIPath)(agentController.GetComponent("AIPath"))).canMove = true;
			
			if(((Agent)agentComponent).wMemory.getFact("Stone").Count > 0)
			{
			//ta reda på positionen för stenen
	
					moveToPosition = (Vector3)((Agent)agentObject.GetComponent("Agent")).wMemory.getFact("Stone")[0].factValue;
					((Agent)agentComponent).target.transform.position = moveToPosition;
					((AIPath)(agentController.GetComponent("AIPath"))).target = ((Agent)agentComponent).target.transform;
					((AIPath)(agentController.GetComponent("AIPath"))).canSearch = true;
					
					//moveDirection = moveToPosition - agentObject.transform.position;
				
				if(Vector2.Distance(moveToPosition, agentController.transform.position) <= 1.0f)
				{
						
					//((SpecialAgent)agentComponent).wMemory.removeFact("stonePosition");
					((Agent)agentComponent).actionIsDone = true;
					((AIPath)(agentController.GetComponent("AIPath"))).canMove = false;
					Debug.Log("*GOT-STONE*");
				}
			} else
			{
				if(Vector2.Distance(moveToPosition, agentController.transform.position) <= 1.0f)
				{
					moveToPosition = agentController.transform.TransformDirection(new Vector3(Random.Range(-10.0f, 10.0f), -2.0f, Random.Range(-10.0f, 10.0f)) - agentController.transform.position);
				}
				((Agent)agentComponent).target.transform.position = moveToPosition;
				((AIPath)(agentController.GetComponent("AIPath"))).target = ((Agent)agentComponent).target.transform;
				((AIPath)(agentController.GetComponent("AIPath"))).canSearch = true;
			}
		}
	}
}