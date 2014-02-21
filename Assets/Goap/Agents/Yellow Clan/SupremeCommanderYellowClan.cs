using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Commanders/SupremeCommanderYellowClan")]

public class SupremeCommanderYellowClan: MonoBehaviour
{
	public float globalEnergy;
	string clan;
	UnitCommanderYellowClan unitCommander;
	BuildingCommanderYellowClan buildingCommander;
	
	void Awake()
	{
	}
	
	void Start()
	{
		//create UnitCommander
		unitCommander = (UnitCommanderYellowClan)Instantiate(Resources.Load(("Prefabs/UnitCommanderYellowClan"), typeof(UnitCommanderYellowClan)));
		unitCommander.transform.position = transform.parent.position;
		unitCommander.transform.parent = transform.parent;
		unitCommander.SetClan(clan);
		
		//create BuildingCommander
		buildingCommander = (BuildingCommanderYellowClan)Instantiate(Resources.Load(("Prefabs/BuildingCommanderYellowClan"), typeof(BuildingCommanderYellowClan)));
		buildingCommander.transform.position = transform.parent.position;
		buildingCommander.transform.parent = transform.parent;
		buildingCommander.SetClan(clan);
	}
	
	void Update()
	{
		
		CalculateScore();
		
		//Add new agents
		int slump = Random.Range(0, 100);
		//Debug.Log(BlackBoard.Instance.GetAgentsInClan(clan) + " " + BlackBoard.Instance.GetPopulationCap(clan) + " " + BlackBoard.Instance.GetScore(clan) + " " + slump);
		/*if( (BlackBoard.Instance.GetAgentsInClan(clan) < BlackBoard.Instance.GetPopulationCap(clan)) && (BlackBoard.Instance.GetScore(clan) > 100) && (slump < 80) )
		{
			BlackBoard.Instance.UpdateScore(clan, "Created Agent");
			//TODO: fix so that the supremecommander can more units of same color that is currently in the tribe
			StartCoroutine(unitCommander.AddAgent(clan.Substring(0, clan.Length-5)));
		}*/
		
		
		//Create new buildings
		if(BlackBoard.Instance.GetTaskTree(clan).GetLeafs().Count == 0 && unitCommander.GetAgents().Count > 0)
		{

			AddNewGoals();
		}
		else if(BlackBoard.Instance.GetTaskTree(clan).GetTree().Count > 0)
		{
			bool okToAdd = true;
		
			foreach(TreeNode node in BlackBoard.Instance.GetTaskTree(clan).GetTree()[0])
			{	
				if(node.GetActionName().Substring(node.GetActionName().Length-11) != "FloorAction")
				{
					okToAdd = false;
				}
			}
			if(okToAdd == true)
			{
				AddNewGoals();
			}
		}
	}
	
	private float CalculateScore()
	{
		//TODO, call the function in the blackboard instead of calculating here....
		globalEnergy = 0;
		foreach(Agent agent in unitCommander.GetAgents())
		{
			globalEnergy += agent.GetEnergy();
		}
		globalEnergy = globalEnergy/unitCommander.GetAgents().Count;
		
		return globalEnergy;
	}	
	
	public void AddNewGoals()
	{
		bool okToAdd = false;
		Debug.Log ("BWAHAHAHHAHAH");
	
			Debug.Log ("Nu borde det slumpas mer tasks");
			//loopa igenom actions, välj ut en och posta dess postcondition
			WorldState goal = ActionManager.Instance.GetGoal(Random.Range(0, ActionManager.Instance.NumberOfActions() - 1));
			Dictionary<string, WorldStateValue> dict = goal.getProperties();
			string building = "";
			
			foreach(KeyValuePair<string, WorldStateValue> pair in dict)
			{
				building = pair.Key;
			}
			
			Action action = ActionManager.Instance.getSuitableActions(goal)[0];
			string actionName = action.GetActionName().Substring(0, 5); //Build action?
			
			if(actionName == "Build")
			{
				string type = "";
				
				int firstUpper = 0;
				
				for(int i = 0; i<building.Length-1; i++)
				{
					if(char.IsUpper(building[i]))
					{
						type = building.Substring(i);
						
						if(firstUpper == 0)
						{
							firstUpper = i;
						} else {
						
							type = building.Substring(firstUpper, i-firstUpper);
							break;
						}
					}
				}
				
				if( type != "Floor")
				{
				
					List<string> colorsInClan = BlackBoard.Instance.GetColorsInClan(clan);		
					//Check if action requires several agents then it should check if we got both those agentcolors
					
					bool haveAllAgents = true;
					
					foreach ( List<string> listOfColors in ActionManager.Instance.AgentsThatDoAction(action.GetActionName()))
					{
						
						
						foreach(string color in listOfColors)
						{
							if(!colorsInClan.Contains(color))
							{
								haveAllAgents = false;
							}
						}
						
						if(haveAllAgents)
						{
							break;
						}
					}
					
					
					if(haveAllAgents == true) //We can only add new buildings as goals
					{
						Debug.Log("Och vi har alla agenter!!");
						/*string color = "";
						for(int i = 0; i < building.Length; i++)
						{
							if(char.IsUpper(building[i]))
							{
								color = building.Substring(0, i);
								break;
							}
						}
						color = char.ToUpper(color[0]) + color.Substring(1);
						*/
							Debug.Log ("NEW GOAL!!!! " + building);
							buildingCommander.SetGoal(goal);
							okToAdd = true;
					}
				
				}
		
			}
		
		
		//buildingCommander.SetGoal(new WorldState("blackTowerIsBuilt", new WorldStateValue(true)));
		//buildingCommander.SetGoal(new WorldState("magentaFactoryIsBuilt", new WorldStateValue(true)));
		//buildingCommander.SetGoal(new WorldState("redHouseIsBuilt", new WorldStateValue(true)));
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;
	}
	
	public void ImmigratingAgentArriving(Agent immigrant) //New immigrants arriving, upgrade excisting house
	{
		//Tell the commander of the reciving clan that new agents are arriving
		buildingCommander.ImmigratingAgentArriving(immigrant);
	}
	
	public bool HaveABreak()
	{
		int slump = Random.Range(0,100);
		if(slump < 10)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
