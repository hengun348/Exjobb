using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sensors: MonoBehaviour {
	
	
	void Start(){
		//add all the sensors
		GameObject ResSens = new GameObject("ResourceSensor");
		ResSens.transform.position = transform.position;
		ResSens.transform.parent = transform;
		ResSens.AddComponent("ResourceSensor");
		
		GameObject EnSens = new GameObject("EnemySensor");
		EnSens.transform.position = transform.position;
		EnSens.transform.parent = transform;
		EnSens.AddComponent("EnemySensor");
	}	
}