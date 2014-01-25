using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceObject: MonoBehaviour {
	
	int amount;
	
	void Start()
	{
		amount = 100;
	}
	
	public void UpdateResource(int resourcesTaken)
	{
		amount = amount - resourcesTaken;
		//Debug.Log ("Recource -" + resourcesTaken + "-> " + amount);
	}
}