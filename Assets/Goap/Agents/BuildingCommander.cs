using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Commanders/BuildingCommander")]

public class BuildingCommander: MonoBehaviour
{
	GameObject buildingGroup;
	BlackBoard blackBoard;
	Planner planner;
	List<AStarNode> plan;
	List<WorldState> goals;
	int goalIndex;
	string clan;
	
	void Awake()
	{
		goalIndex = 0;
		blackBoard = BlackBoard.Instance;
		planner = new Planner();
		goals = new List<WorldState>();
		buildingGroup = new GameObject();
		

	}
	
	void Start()
	{	
		buildingGroup.transform.parent = gameObject.transform.parent.parent;
		buildingGroup.transform.position = gameObject.transform.parent.parent.position;
		buildingGroup.name = "Buildings";
		//blackBoard.GetTaskTree(clan).PrintTree();
		CreateTemple();
		
		//TEMPTEMPTEMPTEMP DEBUGGING ENBART 
		
		
		Building building = (Building)Instantiate(Resources.Load(("Prefabs/Building"),typeof(Building)));
	
		building.transform.position = new Vector3(Random.Range(-50.0f, 50.0f), 0.5f, Random.Range(-50.0f, 50.0f));
		building.transform.parent = GameObject.Find(clan).transform.FindChild("Buildings");
		building.renderer.material.color = BlackBoard.Instance.GetColorForObject(clan.Substring(0, clan.Length-5));
		building.tag = clan.Substring(0, clan.Length-5) + "House";
		building.name = clan.Substring(0, clan.Length-5) + "House";
		
		AstarPath.active.UpdateGraphs (building.collider.bounds);
		
	
		Building building2 = (Building)Instantiate(Resources.Load(("Prefabs/Building"),typeof(Building)));
	
		building2.transform.position = new Vector3(Random.Range(-50.0f, 50.0f), 0.5f, Random.Range(-50.0f, 50.0f));
		building2.transform.parent = GameObject.Find(clan).transform.FindChild("Buildings");
		building2.renderer.material.color = BlackBoard.Instance.GetColorForObject(clan.Substring(0, clan.Length-5));
		building2.tag = clan.Substring(0, clan.Length-5) + "House";
		building2.name = clan.Substring(0, clan.Length-5) + "House";
	}
	
	void CreateTemple()
	{
		//Create the temple and place it in the world
		Temple prefab = (Temple)Instantiate(Resources.Load(("Prefabs/Temple"),typeof(Temple)));
		prefab.transform.parent = buildingGroup.transform;
		prefab.transform.position = buildingGroup.transform.position;
		prefab.SetClan(clan);
		
		
		//To get the correct color for the temple
		string color = "";
		for (int i = 1; i < clan.Length; i++) 
		{ 
			if (char.IsUpper(clan[i])) 
			{
				color = clan.Substring(0, i - 1); 
				break; 
			} 
		}
		
		foreach(Transform child in prefab.transform)
		{
			child.renderer.material.color = BlackBoard.Instance.GetColorForObject(color);
			AstarPath.active.UpdateGraphs (child.collider.bounds);
		}
		
		
		//Create a dictionary and save it to the blackboard
		Dictionary<string, WorkingMemoryValue> templeInfo = new Dictionary<string, WorkingMemoryValue>();
		templeInfo.Add ("Type", new WorkingMemoryValue("Temple"));
		templeInfo.Add ("Position", new WorkingMemoryValue(transform.position));
		templeInfo.Add ("Length", new WorkingMemoryValue(3.0f));
		templeInfo.Add ("Health", new WorkingMemoryValue(1000.0f));
		templeInfo.Add ("Floors", new WorkingMemoryValue(1));
		BlackBoard.Instance.SetFact(clan, "Buildings", new WorkingMemoryValue(templeInfo));
		Debug.Log("Nu är templet byggt");
	}
	
	public void SetClan(string clan)
	{
		
	 	this.clan = clan;
		
	}
	
	Vector3 CheckPosition(Vector3 position, List<WorkingMemoryValue> buildings)
	{
		foreach(WorkingMemoryValue building in buildings)
		{
			Dictionary<string, WorkingMemoryValue> temp = (Dictionary<string, WorkingMemoryValue>)building.GetFactValue();
			if(Vector3.Distance((Vector3)temp["Position"].GetFactValue(), position) <= ((float)temp["Length"].GetFactValue() + 2.5f))	
			{
				//can't build here, create new position
				return CheckPosition(new Vector3(Random.Range(-50.0f, 50.0f), 0.5f, Random.Range(-50.0f, 50.0f)), buildings);
			}
		}
		return position;
	}
	
	public void SetGoal(WorldState buildingResult)
	{
		goals.Clear();
		goals.Add(buildingResult);
		PlaceBuildings();
	}
	
	private void PlaceBuildings()
	{
		Vector3 position = new Vector3();
		
		List<TreeNode> planList = new List<TreeNode>();
		
		foreach(WorldState goal in goals)
		{
			goalIndex ++;
			planner.SetGoalWorldState(goal);
			plan = planner.RunAStar((WorldState)blackBoard.GetFact(clan, "currentWorldState")[0].GetFactValue());
					
			Debug.Log("STARTPLAN!!!!!!!!: " + plan.Count);
			foreach(AStarNode node in plan)
			{
				Debug.Log ("-----l node.getName() " + node.getName());
				//Debug.Log("-----" + node.getName());
			}
			
			
			
			//Debug.Log ("Plan count: " + plan.Count);
			//Add the wireframe for each house!!!
			for(int j = plan.Count-1; j > -1; j--)
			{
				//Debug.Log ("PlanStep " + plan[j].getName() );
				
				if(plan[j].getName().Substring(0,1) == ("B"))
					{
						//Debug.Log ("***********BUILD**********");
						position = new Vector3(Random.Range(gameObject.transform.position.x -10.0f, gameObject.transform.position.x + 10.0f), 0.5f, Random.Range(gameObject.transform.position.z - 10.0f, gameObject.transform.position.z + 10.0f));
						
						List<WorkingMemoryValue> buildings = BlackBoard.Instance.GetFact(clan, "Buildings");
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
						
					
						house.transform.parent = buildingGroup.transform;
								
						
						string actionName = plan[j].getName();
						string color = "";
						
						string type = actionName.Substring(5, actionName.Length-6);
					
						Dictionary<string, WorkingMemoryValue> houseInfo = new Dictionary<string, WorkingMemoryValue>();
						houseInfo.Add ("Type", new WorkingMemoryValue(type));
						houseInfo.Add ("Position", new WorkingMemoryValue(house.transform.position));
						houseInfo.Add ("Length", new WorkingMemoryValue(1.0f));
						//houseInfo.Add ("Width", new WorkingMemoryValue(1.0f));
						houseInfo.Add ("Health", new WorkingMemoryValue(300.0f));
						houseInfo.Add ("Floors", new WorkingMemoryValue(1));
						
						BlackBoard.Instance.SetFact(clan, "Buildings", new WorkingMemoryValue(houseInfo));
	
						
						//Debug.Log ("actionName: " + actionName);
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
						
						//Debug.Log ("color: " + color);
						
						//Debug.Log (color);
						house.name = color + " wireframe";
						wireframe aa = (wireframe)house.GetComponent("wireframe");
						aa.lineColor = BlackBoard.Instance.GetColorForObject(color);
						
					}
				if(j == plan.Count-1)
				{
					planList.Add(new TreeNode(position, plan[j].getName(), null, goalIndex));
					//Debug.Log ("***************** " + plan[j].getName() + " with position: " + position);
				} else 
				{
	
					for(int k = 0; k < planList.Count; k++)
					{
						
						if(planList[k].GetActionName() == plan[j].getParent().getName())
						{
							
							if(plan[j].getName().Substring(0,1) == ("B"))
							{
								//Debug.Log ("***************** " + plan[j].getName() + " with position: " + position);
								planList.Add(new TreeNode(position, plan[j].getName(), planList[k], goalIndex));
							} else 
							{
								planList.Add(new TreeNode(planList[k].GetPosition(), plan[j].getName(), planList[k], goalIndex));
							}
							break;
						}
					}
				}
				
				
				//blackBoard.AddToTaskTree(plan[j], position, goalIndex);
			}
			
			blackBoard.AddToTaskTree(clan, planList);
		}
	}
	
	public void ImmigratingAgentArriving(string color) //New immigrants arriving, add layer and upgrade excisting house
	{
		//find house to upgrade
		//increase layer
		//add layer in right color to it
		
		//Gå igenom alla bostadshus clanen har och om man hittar ett med children mindre än 3 så lägg till ett floor på det huset 
		Debug.Log ("??????????????????????????Immigrant arriving adding floor");
		
	
		if(!blackBoard.GetColorsInClan(clan).Contains(color))
		{
			blackBoard.AddColorToClan(clan, color);
		}
	
		
		foreach(Transform building in GameObject.Find(clan).transform.FindChild("Buildings"))
		{
			string buildingName = building.name.Substring(building.name.Length - 5, 5);
			if(buildingName.Substring(buildingName.Length-5, buildingName.Length) == "House")
			{
				int numberFloors = ((Building)building.GetComponent("Building")).GetFloors();
				
				if(numberFloors < 3) //max floors = 3
				{
					goalIndex++;
					planner.SetGoalWorldState(new WorldState(char.ToLower(color[0]) + color.Substring(1) + "FloorIsBuilt", new WorldStateValue(true)));
					plan = planner.RunAStar((WorldState)blackBoard.GetFact(clan, "currentWorldState")[0].GetFactValue());
					Debug.Log ("***********Plan Size " + plan.Count);
					
					List<TreeNode> planList = new List<TreeNode>();
					Vector3 positionOfHouseToUpgrade = building.position;
					
					Debug.Log("6666666666666666666666 floor ska byggas på " + positionOfHouseToUpgrade + " " + building.name);
					
					planList.Add(new TreeNode(positionOfHouseToUpgrade, plan[1].getName(), null, goalIndex));
					planList.Add(new TreeNode(positionOfHouseToUpgrade, plan[0].getName(), planList[0], goalIndex));
					
					blackBoard.AddToTaskTree(clan, planList);
					
						
								
					
					numberFloors++;
					((Building)building.GetComponent("Building")).SetFloors(numberFloors);
					
					
					break;
				}
			}
		}
	}
}
