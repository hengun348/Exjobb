using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[AddComponentMenu("Commanders/BuildingCommander")]

public class BuildingCommander: MonoBehaviour
{
	GameObject buildingGroup; //the group in a clan which all buildings are placed under
	Planner planner;
	List<AStarNode> plan; //The plan with the actions to build a buidling
	List<WorldState> goals; //the goals the buildingCommander receives 
	int goalIndex; //used to seperate the subtrees, containting each goal and the actions to reach it, in the tasktree
	string clan; //The clan the commander belongs to
	List<TreeNode> planList; //List to be sent into the TaskTree
	
	void Awake()
	{
		goalIndex = 0;
		planner = new Planner();
		goals = new List<WorldState>();
		buildingGroup = new GameObject();
		planList = new List<TreeNode>();
	}
	
	void Start()
	{	
		buildingGroup.transform.parent = gameObject.transform.parent.parent;
		//buildingGroup.transform.position = gameObject.transform.parent.parent.position;
		buildingGroup.name = "Buildings";		
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;	
	}
	
	Vector3 CheckPosition(Vector3 position, List<WorkingMemoryValue> buildings) //Check if a position is available 
	{
		
		//TODO borde kontrollera fler saker än bara andra hus
		foreach(WorkingMemoryValue building in buildings)
		{
			
			Dictionary<string, WorkingMemoryValue> temp = (Dictionary<string, WorkingMemoryValue>)building.GetFactValue();
			if(Vector3.Distance((Vector3)temp["Position"].GetFactValue(), position) <= ((float)temp["Length"].GetFactValue() + 2.5f))	
			{
				//can't build here, create new position
				return CheckPosition(new Vector3(Random.Range(-50.0f, 50.0f), 0.5f, Random.Range(-50.0f, 50.0f)), buildings);
			}
		}
		
		//Check so that the house is built inside the grid area
		if(Mathf.Abs(position.x) > AstarPath.active.astarData.gridGraph.size.x/5 || Mathf.Abs(position.z) > AstarPath.active.astarData.gridGraph.size.y/5)
		{
			return CheckPosition(new Vector3(Random.Range(-50.0f, 50.0f), 0.5f, Random.Range(-50.0f, 50.0f)), buildings);
		}
		
		
		return position;
	}
	
	public void SetGoal(WorldState buildingResult) //adds a new goal for the buildingcommander
	{
		goals.Clear();
		goals.Add(buildingResult);
		PlaceBuildings();
	}
	
	private void PlaceBuildings()
	{
		Vector3 position = new Vector3();
		
		foreach(WorldState goal in goals) //goes thru all the goals
		{
			goalIndex ++;
			planner.SetGoalWorldState(goal);
			plan = planner.RunAStar((WorldState)BlackBoard.Instance.GetFact(clan, "currentWorldState")[0].GetFactValue());
			
			Debug.Log("***************STARTPLAN!!!!!!!!: " + plan.Count);
			
			
			
			foreach(AStarNode node in plan)
			{
				
				Debug.Log ("-----l " + node.GetName());
			}
			
			Debug.Log("***************");
		
			ConvertNodes(plan[plan.Count-1], null);
			
			for(int j = 0; j < planList.Count; j++)
			{
				if(planList[j].GetActionName().Substring(0,1) == ("B")) //its a build action so need new position for house
				{
					
					position = new Vector3(Random.Range(gameObject.transform.position.x -10.0f, gameObject.transform.position.x + 10.0f), 0.5f, Random.Range(gameObject.transform.position.z - 10.0f, gameObject.transform.position.z + 10.0f)); //new random position of building
					
					List<WorkingMemoryValue> buildings = BlackBoard.Instance.GetFact(clan, "Buildings"); //get all the buildings of the clan

					position = CheckPosition(position, buildings); //check if position is available if not a new will be given
					
					string actionName = planList[j].GetActionName();
					string colorAndType = actionName.Substring(5, actionName.Length-11);
					
					GameObject house = (GameObject)Instantiate(Resources.Load(("Prefabs/Building"),typeof(GameObject)));	
					
					
					//Add a wireframe for each house
					Destroy(house.GetComponent<BoxCollider>());
					house.tag = "wireframe";
					house.gameObject.AddComponent("wireframe");
					house.transform.parent = buildingGroup.transform;
					house.transform.position = position;
					
					//add information about the house to the blackboard
					Dictionary<string, WorkingMemoryValue> houseInfo = new Dictionary<string, WorkingMemoryValue>();
					houseInfo.Add ("Type", new WorkingMemoryValue(colorAndType));
					houseInfo.Add ("Position", new WorkingMemoryValue(position));
					houseInfo.Add ("Length", new WorkingMemoryValue(1.0f));
					houseInfo.Add ("Width", new WorkingMemoryValue(1.0f));
					houseInfo.Add ("Health", new WorkingMemoryValue(300.0f));
					houseInfo.Add ("Floors", new WorkingMemoryValue(1));
					
					BlackBoard.Instance.SetFact(clan, "Buildings", new WorkingMemoryValue(houseInfo));
					
					string color = "";
					
					//get the color of the building 
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

					house.name = color + " wireframe";
					wireframe aa = (wireframe)house.GetComponent("wireframe");
					aa.lineColor = BlackBoard.Instance.GetColorForObject(color);
				
					planList[j].SetPosition(position);
					
				} else { //its a get action so need position of the parent node (position of the building)
					
					planList[j].SetPosition(planList[j].GetParent().GetPosition());
				
				}
	
			}
			
			BlackBoard.Instance.AddToTaskTree(clan, planList); //set up the new plan in the tasktree
			planList.Clear();
		}
	}
	
	public void ConvertNodes(AStarNode node, TreeNode parent2) //Converts AStarNodes to TreeNodes
	{
		TreeNode parent = new TreeNode();
		parent = new TreeNode(new Vector3(), node.GetName(), parent2, goalIndex);
		planList.Add(parent);
	
		foreach(AStarNode child in node.GetChildren(plan))
		{
			ConvertNodes(child, parent);
		}
	
	}
}
