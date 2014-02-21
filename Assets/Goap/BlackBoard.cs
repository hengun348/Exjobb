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
		bool alreadyKnown = false;
		foreach(WorkingMemoryValue vm in tribeFacts[clan].GetFact(name))
		{
			
			if(vm.GetFactValue().Equals( factValue.GetFactValue()))
			{
				//Already knows about it
				Debug.Log("Klanen vet redan det här!");
				alreadyKnown = true;
				break;
			}
		}
		
		if(!alreadyKnown)
		{	
			tribeFacts[clan].SetFact(name, factValue);
		}

		if(name == "Agents")
		{
			tribeFacts[clan].ChangeNumberAgentsInClan(1);
		}
	}
	
	public List<WorkingMemoryValue> GetFact(string clan, string name)
	{
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
	
	public void SetCurrentWorldstate(string clan,  WorldState ws)
	{
		WorldState currentWorldState = (WorldState)GetFact(clan, "currentWorldState")[0].GetFactValue();
		RemoveFact(clan, "currentWorldState", new WorkingMemoryValue(currentWorldState));
		
		WorldState tempWorldState = new WorldState ();
		
		foreach(KeyValuePair<string, WorldStateValue> newState in ws.getProperties())
		{
		
			foreach (KeyValuePair<string, WorldStateValue> oldState in currentWorldState.getProperties())
			{
				
				if(oldState.Key == newState.Key)
				{
					
					tempWorldState.setProperty(newState.Key, newState.Value);
				} else{
				
					tempWorldState.setProperty(oldState.Key, oldState.Value);
				}
			}
			
		}
		

		SetFact(clan, "currentWorldState", new WorkingMemoryValue(tempWorldState));
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
		
		
		WorldState currentWorldState = new WorldState();

		currentWorldState.setProperty("blueResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("redResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("yellowResourceIsAvailable", new WorldStateValue(false));
		
		currentWorldState.setProperty("orangeResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("greenResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("magentaResourceIsAvailable", new WorldStateValue(false));
		
		
		//SetCurrentWorldstate(clan, currentWorldState);
		SetFact(clan, "currentWorldState", new WorkingMemoryValue(currentWorldState));
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
	
	public void RemoveFact(string clan, string factName, WorkingMemoryValue factValue)
	{
		tribeFacts[clan].RemoveFact(factName, factValue);
	}
}