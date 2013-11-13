using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building: MonoBehaviour
{
	
	private Vector3 position;
	private float length;
	private float width;
	private float health;
	private string houseType;
	
	void Awake(){
	
		//position = new Vector3();
		
		
	}
	
	public void setUpBuilding(string houseType, Vector3 position){
	
		this.houseType = houseType;
		this.position = position;
	}
	
	public Vector3 getPosition(){
	
		return position;
		
	}
	
	public float getLength(){
	
		return length;
		
	}
	
	public float getWidth(){
	
		return width;
		
	}
	
	void Start(){
	
		
		
		
	}
	
	void Update(){
		

	}
	
}