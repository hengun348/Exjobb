using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingCommander: MonoBehaviour
{
	
	private BlackBoard blackBoard;
	private Planner planner;
	List<AStarNode> plan;
	int goalIndex;
	
	void Start()
	{
		goalIndex = 0;
		blackBoard = BlackBoard.Instance;
		planner = new Planner();
		
		List<WorldState> goals = new List<WorldState>();
		
		
		//goals.Add( new WorldState("redHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("blueHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("yellowHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("yellowHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("yellowHouseIsBuilt", new WorldStateValue(true)));*/
		//goals.Add( new WorldState("orangeFactoryIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("greenFactoryIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("magentaFactoryIsBuilt", new WorldStateValue(true)));
		
		goals.Add( new WorldState("blackTowerIsBuilt", new WorldStateValue(true)));
		goals.Add( new WorldState("blackTowerIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		//goals.Add( new WorldState("purpleHouseIsBuilt", new WorldStateValue(true)));
		
		
		
		Vector3 position = new Vector3();
		
		foreach(WorldState goal in goals)
		{
			goalIndex ++;
			planner.SetGoalWorldState(goal);
			plan = planner.RunAStar((WorldState)blackBoard.GetFact("currentWorldState")[0].GetFactValue());
					
			Debug.Log("STARTPLAN!!!!!!!!: " + plan.Count);
			foreach(AStarNode node in plan)
			{
				Debug.Log("-----" + node.getName());
			}
			

			
				Debug.Log ("Plan count: " + plan.Count);
			//Add the wireframe for each house!!!
			for(int j = plan.Count-1; j > -1; j--)
			{
				Debug.Log ("PlanStep " + plan[j].getName() );
				
				if(plan[j].getName().Substring(0,1) == ("B"))
					{
						Debug.Log ("***********BUILD**********");
						position = new Vector3(Random.Range(-20.0f, 20.0f), 0.5f, Random.Range(-20.0f, 20.0f));
						
						List<WorkingMemoryValue> buildings = BlackBoard.Instance.GetFact("Buildings");
						if(buildings.Count > 0){
							//TODO borde kontrollera fler saker än bara andra hus
							//Debug.Log ("måste kolla positionen på husen");
							position = CheckPosition(position, buildings);
						}
						
						GameObject house = GameObject.CreatePrimitive(PrimitiveType.Cube);
						
						Destroy(house.GetComponent<BoxCollider>());
						house.tag = "wireframe";
						house.AddComponent("wireframe");
						house.transform.position = position;
								
						
						string actionName = plan[j].getName();
						string color = "";
						
						
						Debug.Log ("actionName: " + actionName);
						int firstUpper = -1;
							for (int i = 1; i < actionName.Length; i++) 
							{ 
								if (char.IsUpper(actionName[i])) 
								{
									if(firstUpper == -1)
									{
										firstUpper = i;
									} else {
										color = actionName.Substring(firstUpper, i - firstUpper); 
										break; 
									}
									
								} 
							}
						
						//color = char.ToUpper(color[0]) + color.Substring(1);
						
						Debug.Log ("color: " + color);
						
						//Debug.Log (color);
						
						wireframe aa = (wireframe)house.GetComponent("wireframe");
						aa.lineColor = BlackBoard.Instance.GetColorForObject(color);
						
					}
				
				blackBoard.AddToTaskTree(plan[j], position, goalIndex);
			}
			
			
				
		}
		
		
	}
	
	Vector3 CheckPosition(Vector3 position, List<WorkingMemoryValue> buildings)
	{
		
		foreach(WorkingMemoryValue building in buildings)
		{
			Dictionary<string, WorkingMemoryValue> temp = (Dictionary<string, WorkingMemoryValue>)building.GetFactValue();
			//Debug.Log ("positionen på huset som kollas: " + temp["Position"].factValue);
			//Debug.Log ("Agenten är på väg till: " + moveToPosition);
			//Debug.Log ("Avstånd mellan husposition och moveposition: " + Vector3.Distance((Vector3)temp["Position"].factValue, moveToPosition));
			if(Vector3.Distance((Vector3)temp["Position"].GetFactValue(), position) <= ((float)temp["Length"].GetFactValue() + 2.5f))	
			{
				//don't build here, create new position
				return CheckPosition(new Vector3(Random.Range(-50.0f, 50.0f), 0.5f, Random.Range(-50.0f, 50.0f)), buildings);
			}
		}
		//Debug.Log ("allt är frid och fröjd");
		return position;
	}
}
