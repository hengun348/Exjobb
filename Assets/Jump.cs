using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Jump : MonoBehaviour {
	
	CharacterController controller;
	public float jumpHeight = 8.0f;
	public float gravity = 10.0f;
	Vector3 moveDirection;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
		moveDirection = new Vector3(0,jumpHeight,0);
	}
	
	void Update()
	{
		
		
		// Apply gravity
		moveDirection.y -= gravity * Time.deltaTime;
		
		// Move the controller
		controller.Move(moveDirection * Time.deltaTime);
		
		//hahah
	}
	
	void Execute()
	{
		
	}
	
}
