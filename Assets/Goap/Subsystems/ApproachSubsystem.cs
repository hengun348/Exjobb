using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApproachSubsystem: MonoBehaviour{
		
	
	CharacterController controller;
	
	void Start(){

		
		

		controller = gameObject.transform.parent.GetComponent<CharacterController>();

		
	}
	
	void Update(){
		if(((Agent)controller.GetComponent("Agent")).currentAction == "approachAction"){
			Debug.Log("*APPROACHING*");			
			((Agent)controller.GetComponent("Agent")).actionIsDone = true;
			
		}
	}	
}