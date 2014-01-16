using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartScript: MonoBehaviour
{
	
	private BlackBoard blackBoard;
	private Planner planner;
	List<AStarNode> plan;
	
	void Start(){
		
		blackBoard = BlackBoard.Instance;
		planner = new Planner();
		
		List<WorldState> goals = new List<WorldState>();
		
		//goals.Add( new WorldState("redHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("blueHouseIsBuilt", new WorldStateValue(true)));
		goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		
		
		
		
		
		foreach(WorldState goal in goals)
		{
			planner.goalWorldState = goal;
			plan = planner.runAStar((WorldState)blackBoard.getFact("currentWorldState")[0].getFactValue());
					
			Debug.Log("STARTPLAN!!!!!!!!: " + plan.Count);
			foreach(AStarNode node in plan)
			{
				Debug.Log("-----" + node.name);
			}
			
			Vector3 position = new Vector3(Random.Range(-20.0f, 20.0f), 0, Random.Range(-20.0f, 20.0f));
			
			List<WorkingMemoryValue> buildings = BlackBoard.Instance.getFact("Buildings");
			if(buildings.Count > 0){
				//TODO borde kontrollera fler saker än bara andra hus
				//Debug.Log ("måste kolla positionen på husen");
				position = checkPosition(position, buildings);
			}

			blackBoard.addToTaskTree(plan, position);
		}
	}
	
	Vector3 checkPosition(Vector3 position, List<WorkingMemoryValue> buildings)
	{
		
		foreach(WorkingMemoryValue building in buildings)
		{
			Dictionary<string, WorkingMemoryValue> temp = (Dictionary<string, WorkingMemoryValue>)building.getFactValue();
			//Debug.Log ("positionen på huset som kollas: " + temp["Position"].factValue);
			//Debug.Log ("Agenten är på väg till: " + moveToPosition);
			//Debug.Log ("Avstånd mellan husposition och moveposition: " + Vector3.Distance((Vector3)temp["Position"].factValue, moveToPosition));
			if(Vector3.Distance((Vector3)temp["Position"].getFactValue(), position) <= ((float)temp["Length"].getFactValue() + 2.5f))	
			{
				//don't build here, create new position
				return checkPosition(new Vector3(Random.Range(-50.0f, 50.0f), 0, Random.Range(-50.0f, 50.0f)), buildings);
			}
		}
		//Debug.Log ("allt är frid och fröjd");
		return position;
	}
}