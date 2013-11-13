using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Subsystems: MonoBehaviour {
	
	
	void Start(){
	
		gameObject.AddComponent("ScoutSubsystem");
		gameObject.AddComponent("ApproachSubsystem");
		gameObject.AddComponent("DetonateBombSubsystem");
		gameObject.AddComponent("GetWoodSubsystem");
		gameObject.AddComponent("GetStoneSubsystem");
		gameObject.AddComponent("BuildHouseSubsystem");
		gameObject.AddComponent("BuildPyramidSubsystem");
		
	}
	

	
}