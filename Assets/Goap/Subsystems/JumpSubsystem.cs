using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class JumpSubsystem {
	
	CharacterController controller;
	public float jumpHeight = 8.0f;
	public float gravity = 20.0f;
	Vector3 moveDirection;
	
	BlackBoard blackBoard;
	
	public JumpSubsystem(BlackBoard blackBoard){
		
		this.blackBoard = blackBoard;
		
	}
	
	
	public bool Update(CharacterController controller)
	{
		
		/*controller = GetComponent<CharacterController>();
		moveDirection = new Vector3(0,jumpHeight,0);
		
		// Apply gravity
		moveDirection.y -= gravity * Time.deltaTime;
		
		// Move the controller
		controller.Move(moveDirection * Time.deltaTime);*/
		
		/*if(BlackBoard.Instance().getCurrentAction() == "jumpAction"){
		
			Debug.Log("*JUMPING*");
		
		}*/
		return true;
		
	}
	
}
