using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BlackBoard {
	
	private static BlackBoard instance;
	private Dictionary<string, TribeFacts> tribeFacts;
	private Dictionary<string, Color> colors;
	List<string> clans;
	
	public static BlackBoard Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new BlackBoard();
			}
			return instance;
		}
		
	}
	
	private BlackBoard()
	{
		clans = new List<string>();
		tribeFacts = new Dictionary<string, TribeFacts>();

		
		colors = new Dictionary<string, Color>(); 
		colors["Blue"] = new Color(0, 0, 1.0f); // and so on 
		colors["Red"] = new Color(1.0f, 0, 0);
		colors["Yellow"] = Color.yellow;
		colors["Purple"] = new Color(0.63f, 0.13f, 0.94f);
		colors["Orange"] = new Color(1.0f, 0.65f, 0);
		colors["Magenta"] = new Color(0.63f, 0.13f, 0.94f);
		colors["Green"] = new Color(0, 1.0f, 0);
		colors["Black"] = Color.black;
		
	}
	
	public void SetFact(string clan, string name, WorkingMemoryValue factValue)
	{
		//Debug.Log ("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!SÄTTER FAKTA!!!!!");
		tribeFacts[clan].SetFact(name, factValue);
		
		if(name == "Agents")
		{
			tribeFacts[clan].ChangeNumberAgentsInClan(1);
		}
	}
	
	public List<WorkingMemoryValue> GetFact(string clan, string name)
	{
		//Debug.Log ("******************CLAN" + clan);
		return tribeFacts[clan].GetFact(name);			
	}
	
	public bool ContainsFact(string clan, string name)
	{
		return tribeFacts[clan].ContainsFact(name);
	}
	
	public void ChangeNumberAgentsInClan(string clan, int change)
	{
		tribeFacts[clan].ChangeNumberAgentsInClan(change);
	}
	
	/*public bool agentInHelpList(System.Guid agentNumber){
		
		List<WorkingMemoryValue> helpLists = BlackBoard.Instance.getFact("HelpList");
				
				bool inHelpList= false;
				
				foreach(WorkingMemoryValue helpList in helpLists)
				{
					Dictionary<string, WorkingMemoryValue> temp = (Dictionary<string, WorkingMemoryValue>)helpList.getFactValue();
					if(temp["Owner"].getFactValue().Equals(agentNumber))
					{
						inHelpList = true;
						break;
					}
					
				}
				
		return inHelpList;
		
	}
	*/
	//public void AddToTaskTree(AStarNode planStep, Vector3 position, int treeIndex)
	public void AddToTaskTree(string clan, List<TreeNode> plan)
	{
		tribeFacts[clan].AddToTaskTree(plan);
	}
		
	public string GetActionForAgent(string clan, Agent agent)
	{
		return tribeFacts[clan].GetActionForAgent(agent);
	}
	
	public TaskTree GetTaskTree(string clan)
	{
		return tribeFacts[clan].GetTaskTree();
	}
	
	public Color GetColorForObject(string colorName)
	{
		return colors[colorName];		 
	}
	
	public void UpdateScore(string clan, string type)
	{
		tribeFacts[clan].UpdateScore(clan, type);
	}
	
	public void SetScore(string clan, int score)
	{
		tribeFacts[clan].SetScore(score);
	}
	
	public int GetScore(string clan)
	{
		//Debug.Log ("******************CLAN" + clan);
		return tribeFacts[clan].GetScore();
	}
	
	public void SetCurrentWorldstate()
	{
		//TODO fixa så att man itne behöver lägga in allt för hand
		WorldState currentWorldState = new WorldState();
		currentWorldState.setProperty("enemyVisible", new WorldStateValue(false));
		currentWorldState.setProperty("armedWithGun", new WorldStateValue(true));
		currentWorldState.setProperty("weaponLoaded", new WorldStateValue(false));
		//currentWorldState.setProperty("enemyLinedUp", new WorldStateValue(false));
		//currentWorldState.setProperty("enemyAlive", new WorldStateValue(true));
		currentWorldState.setProperty("armedWithBomb", new WorldStateValue(true));
		//currentWorldState.setProperty("nearEnemy", new WorldStateValue(false));
		currentWorldState.setProperty("agentAlive", new WorldStateValue(true));
		
		currentWorldState.setProperty("blueResourceIsAvailable", new WorldStateValue(true));
		currentWorldState.setProperty("redResourceIsAvailable", new WorldStateValue(true));
		currentWorldState.setProperty("yellowResourceIsAvailable", new WorldStateValue(true));
		
		currentWorldState.setProperty("orangeResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("greenResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("magentaResourceIsAvailable", new WorldStateValue(false));
		
		/*currentWorldState.setProperty("redResourceIsCollected", new WorldStateValue(false));
		currentWorldState.setProperty("blueResourceIsCollected", new WorldStateValue(false));
		currentWorldState.setProperty("yellowResourceIsCollected", new WorldStateValue(false));*/
		
		foreach(string clan in clans)
		{
			SetFact(clan, "currentWorldState", new WorkingMemoryValue(currentWorldState));
		}
	}
	
	//Returns a unique clan name for the supremecommander
	public string GetClan()
	{
		
		string clan = "";
		string clanColor = "";
		
		foreach(KeyValuePair<string, Color> color in colors)
		{
			if(!clans.Contains(color.Key + " Clan"))
			{
				clan = color.Key + " Clan";
				clanColor = color.Key;
				break;
			}
		}	
		
		//clan = "Red Clan";
		clans.Add(clan);
		TribeFacts fact = new TribeFacts(clanColor, new TaskTree());
		tribeFacts.Add(clan, fact);
		SetCurrentWorldstate();
		return clan;
	}
	
	//Returns all available clans 
	public List<string> GetClans()
	{
		return clans;
	}
	
	public void RemoveAgentFromOwnedNode(string clan, Agent agent)
	{
		tribeFacts[clan].RemoveAgentFromOwnedNode(agent);
	}
	
	private static int Compare(int x, int y)
	{
		if (x == y)
            return 0;
        else if (x > y)
            return -1;
        else
            return 1;
	}
	
	public List<string> SortClanScores()
	{
		
		List<int> scores = new List<int>();
		
		List<string> returnClans = new List<string>();
		
		foreach(string clan in clans)
		{
			scores.Add (BlackBoard.Instance.GetScore(clan));
		}
		
		scores.Sort(Compare);
		
		foreach(int score in scores)
		{
			foreach(string clan in clans)
			{
				if(score == BlackBoard.Instance.GetScore(clan))
				{
					returnClans.Add(clan);	
				}
			}
		}
		
		return returnClans;
		
	}
	
	public void IncreasePopulationCap(string clan)
	{
		tribeFacts[clan].IncreasePopulationCap();
	}	
	
	public int GetPopulationCap(string clan)
	{
		return tribeFacts[clan].GetPopulationCap();
	}
	
	public int GetAgentsInClan(string clan)
	{
		return tribeFacts[clan].GetAgentsInClan();
	}
	
	public void AddColorToClan(string clan, string color)
	{
		tribeFacts[clan].AddColorToClan(color);
		
	}
	
	public void RemoveColorFromClan(string clan, string color)
	{
		tribeFacts[clan].RemoveColorFromClan(color);
	}
	
	public List<string> GetColorsInClan(string clan)
	{
		return tribeFacts[clan].GetColorsInClan();
	}
}