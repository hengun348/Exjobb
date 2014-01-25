using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		buildingGroup.transform.parent = gameObject.transform.parent.transform.parent;
		buildingGroup.name = "Buildings";
		//blackBoard.GetTaskTree(clan).PrintTree();
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
				//Debug.Log ("-----l node.getName() + " med parent " + node.getParent().getName());
				Debug.Log("-----" + node.getName());
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
				} else 
				{	
					for(int k = 0; k < planList.Count; k++)
					{
						if(planList[k].GetActionName() == plan[j].getParent().getName())
						{
							planList.Add(new TreeNode(position, plan[j].getName(), planList[k], goalIndex));
						}
					}
				}
				
				
				//blackBoard.AddToTaskTree(plan[j], position, goalIndex);
			}
			
			blackBoard.AddToTaskTree(clan, planList);	
		}
	}
}
