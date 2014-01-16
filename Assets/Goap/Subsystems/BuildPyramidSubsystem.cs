using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildPyramidSubsystem: MonoBehaviour{
		
	
	public float jumpHeight;
	public float gravity;
	Vector3 moveDirection, moveToPosition;
	CharacterController controller;
	float start;
	bool firstTime;
	
	void Start(){

		

		controller = transform.parent.GetComponent<CharacterController>();
		jumpHeight = 1.0f;
		gravity = 5.0f;
		
		firstTime=false;
		
	}
	
	void Update(){
		
		
		
		if(((Agent)controller.GetComponent("Agent")).currentAction == "AgentBuildPyramidAction"){
			
			if(firstTime == false){
			
				start = controller.transform.position.x;
				firstTime = true;
				
				moveToPosition = (Vector3)((Agent)controller.GetComponent("Agent")).wMemory.getFact("buildPyramidPosition")[0].getFactValue();//new Vector3(jumpHeight,0,0);
				
				
			}
			
			
			moveDirection = moveToPosition - controller.transform.position;
			// Move the controller
			controller.SimpleMove(moveDirection ); //* Time.deltaTime
			
			
			//if((Mathf.Abs(moveToPosition.x - controller.transform.position.x) <= 1) && (Mathf.Abs(moveToPosition.y - controller.transform.position.y) <= 1) &&  (Mathf.Abs(moveToPosition.z - controller.transform.position.z) <= 1))
			if((Mathf.Abs(moveToPosition.x - controller.transform.position.x) <= 2) &&  (Mathf.Abs(moveToPosition.z - controller.transform.position.z) <= 2))
			{
				GameObject pyramid = GameObject.CreatePrimitive(PrimitiveType.Capsule);
				pyramid.transform.position = controller.transform.position + moveDirection * 0.5f;

				((Agent)controller.GetComponent("Agent")).actionIsDone = true;
				Debug.Log("*PYRAMID-IS-BUILT*");
			}
				

		}
	}
}