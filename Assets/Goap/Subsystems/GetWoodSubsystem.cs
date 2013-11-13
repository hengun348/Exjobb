using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetWoodSubsystem: MonoBehaviour{
		
	

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
		float xpos = Random.Range(-20.0f, 20.0f);
		float zpos = Random.Range(-20.0f, 20.0f);
		moveToPosition = new Vector3(xpos, Terrain.activeTerrain.SampleHeight((Vector3)((Agent)agentComponent).target.transform.position), zpos);
		
	}
	
	void Update(){
		
		if(((Agent)agentComponent).currentAction == "GetWoodAction"){
			
			((AIPath)(agentController.GetComponent("AIPath"))).canMove = true;
			
			if(((Agent)agentComponent).wMemory.getFact("Wood").Count > 0){
			
				moveToPosition = (Vector3)((Agent)agentObject.GetComponent("Agent")).wMemory.getFact("Wood")[0].factValue;
				((Agent)agentComponent).target.transform.position = moveToPosition;
				((AIPath)(agentController.GetComponent("AIPath"))).target = ((Agent)agentComponent).target.transform;
				((AIPath)(agentController.GetComponent("AIPath"))).canSearch = true;
				
				//if(Vector2.Distance(moveToPosition, agentController.transform.position) <= 1.0f)
				if((Mathf.Abs(moveToPosition.x - agentController.transform.position.x) <= 1) &&  (Mathf.Abs(moveToPosition.z - agentController.transform.position.z) <= 1))
				{	
					//((SpecialAgent)agentComponent).wMemory.removeFact("stonePosition");
					((Agent)agentComponent).actionIsDone = true;
					((AIPath)(agentController.GetComponent("AIPath"))).canMove = false;
					Debug.Log("*GOT-WOOD*");
				}
			}else{
				//if(Vector2.Distance(moveToPosition, agentController.transform.position) <= 1.0f)
				if((Mathf.Abs(moveToPosition.x - agentController.transform.position.x) <= 1) &&  (Mathf.Abs(moveToPosition.z - agentController.transform.position.z) <= 1))
				{
					float xpos = Random.Range(-20.0f, 20.0f);
					float zpos = Random.Range(-20.0f, 20.0f);
					moveToPosition = new Vector3(xpos, Terrain.activeTerrain.SampleHeight((Vector3)((Agent)agentComponent).target.transform.position), zpos);
					//moveToPosition = agentController.transform.TransformDirection(new Vector3(Random.Range(-10.0f, 10.0f), -2.0f, Random.Range(-10.0f, 10.0f)) - agentController.transform.position);
				}
				((Agent)agentComponent).target.transform.position = moveToPosition;
				((AIPath)(agentController.GetComponent("AIPath"))).target = ((Agent)agentComponent).target.transform;
				((AIPath)(agentController.GetComponent("AIPath"))).canSearch = true;
			}
		}
	}
}