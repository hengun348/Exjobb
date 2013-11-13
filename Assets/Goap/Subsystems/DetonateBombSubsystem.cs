using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetonateBombSubsystem: MonoBehaviour{
		
	
	public float jumpHeight;
	public float gravity;
	Vector3 moveDirection;
	CharacterController controller;
	
	void Start(){

		

		controller = transform.parent.GetComponent<CharacterController>();
		jumpHeight = 1.0f;
		gravity = 5.0f;
		
	}
	
	void Update(){
		
		if(((Agent)controller.GetComponent("Agent")).currentAction == "detonateBombAction"){
		
			Debug.Log("*BOMBING*");
			Debug.Log("time: " + Time.deltaTime);
		
			moveDirection = new Vector3(0,jumpHeight,0);
			Debug.Log("moveDirection: " + moveDirection);
			// Apply gravity
			moveDirection.y -= gravity * Time.deltaTime;
			
			// Move the controller
			controller.Move(moveDirection * Time.deltaTime);
			
			Debug.Log("controller.transform.position.y: " + controller.transform.position.y);
			if(controller.transform.position.y >= 10)
			{

				((Agent)controller.GetComponent("Agent")).actionIsDone = true;
				
			}
		}
	}
}