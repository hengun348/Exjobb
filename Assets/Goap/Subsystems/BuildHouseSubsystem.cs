using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildHouseSubsystem: MonoBehaviour{
		
	
	Vector3 moveDirection, moveToPosition;
	CharacterController controller;
	bool availablePosition;
	
	void Start(){

		controller = transform.parent.GetComponent<CharacterController>();
		availablePosition = true;
		moveToPosition = new Vector3(Random.Range(-50.0f, 50.0f), -2.0f, Random.Range(-50.0f, 50.0f));
	}
	
	void Update(){
		if(((Agent)controller.GetComponent("Agent")).currentAction == "BuildHouseAction"){
			
			//control that the building isn't built on top of another building
			//-----------------------------------------
			//wMemory.setFact("buildHousePosition", new WorkingMemoryValue(new Vector3(Random.Range(0.0f, 10.0f), 0.0f, Random.Range(0.0f, 10.0f))));
			//moveToPosition = (Vector3)((Agent)controller.GetComponent("Agent")).wMemory.getFact("buildHousePosition").factValue;
			
			//loop through list of all buildings and check their position+width/height to make sure that it is an available spot
			List<WorkingMemoryValue> buildings = ((Agent)controller.GetComponent("Agent")).wMemory.getFact("Building");
			
			if(buildings.Count > 0){
				Debug.Log ("måste kolla positionen på husen");
				moveToPosition = checkPosition(buildings, moveToPosition);
			}
			
			//-----------------------------------------
			
			

			//Debug.Log("husposition: " + moveToPosition);
			
			moveDirection = moveToPosition - controller.transform.position;
			// Move the controller
			controller.SimpleMove(moveDirection);
			//Debug.Log ("distance to house: " + Vector3.Distance(moveToPosition, controller.transform.position));
			//if(Vector3.Distance(moveToPosition, controller.transform.position) <= 2.0f)
			if((Mathf.Abs(moveToPosition.x - controller.transform.position.x) <= 2) &&  (Mathf.Abs(moveToPosition.z - controller.transform.position.z) <= 2))
			{
				//--------------------------------------------------------------------
				GameObject house = GameObject.CreatePrimitive(PrimitiveType.Cube);
				house.transform.position = controller.transform.position + moveDirection * 0.5f;
				house.renderer.material.mainTexture = Resources.Load("wall") as Texture;
				house.tag = "House";
				house.name = "House";
				
				//do we need a building-object?
				//house.AddComponent("Building");
				//((Building)house.GetComponent("Building")).setUpBuilding("house", controller.transform.position);
				
				Dictionary<string, WorkingMemoryValue> houseInfo = new Dictionary<string, WorkingMemoryValue>();
				houseInfo.Add ("Type", new WorkingMemoryValue("House"));
				houseInfo.Add ("Position", new WorkingMemoryValue(house.transform.position));
				houseInfo.Add ("Length", new WorkingMemoryValue(1.0f));
				//houseInfo.Add ("Width", new WorkingMemoryValue(1.0f));
				houseInfo.Add ("Health", new WorkingMemoryValue(300.0f));
				
				((Agent)controller.GetComponent("Agent")).wMemory.setFact("Building", new WorkingMemoryValue(houseInfo));
				//-----------------------------------------------------
				
				

				((Agent)controller.GetComponent("Agent")).actionIsDone = true;
				Debug.Log("*HOUSE-IS-BUILT*");
			}
			
		}
	}
	
	Vector3 checkPosition(List<WorkingMemoryValue>buildings, Vector3 moveToPosition)
	{
		foreach(WorkingMemoryValue building in buildings)
		{
			Dictionary<string, WorkingMemoryValue> temp = (Dictionary<string, WorkingMemoryValue>)building.factValue;
			//Debug.Log ("positionen på huset som kollas: " + temp["Position"].factValue);
			//Debug.Log ("Agenten är på väg till: " + moveToPosition);
			//Debug.Log ("Avstånd mellan husposition och moveposition: " + Vector3.Distance((Vector3)temp["Position"].factValue, moveToPosition));
			if(Vector3.Distance((Vector3)temp["Position"].factValue, moveToPosition) <= ((float)temp["Length"].factValue + 2.5f))	
			{
				Debug.Log ("Nu blir det en krock här");
				//don't build here
				return checkPosition(buildings, new Vector3(Random.Range(-50.0f, 50.0f), -2.0f, Random.Range(-50.0f, 50.0f)));
			}
		}
		//Debug.Log ("allt är frid och fröjd");
		return moveToPosition;
	}
}