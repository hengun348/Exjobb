using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building: MonoBehaviour
{
	int health, floors;
	float length;
	
	void Start()
	{
		health = 100;
		floors = 1;
		length = 1.0f;
	}
	
	public void SetFloors(int floors)
	{
		this.floors = floors;
	}
	
	public int GetFloors()
	{
		return floors;
	}
	
}