using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoutSubsystem: MonoBehaviour{
		
	
	CharacterController controller;
	
	void Start(){

		

		controller = transform.parent.GetComponent<CharacterController>();

		
	}
	
	void Update(){
		if(((Agent)controller.GetComponent("Agent")).currentAction == "scoutAction"){
			Debug.Log("*SCOUTING*");
			((Agent)controller.GetComponent("Agent")).actionIsDone = true;
		}
	}	
}