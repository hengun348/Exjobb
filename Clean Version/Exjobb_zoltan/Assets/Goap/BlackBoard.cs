using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BlackBoard {
	
	private static BlackBoard instance;
	private Dictionary<string, TribeFacts> tribeFacts; //the facts for each of the clans
	private Dictionary<string, Color> colors; //the available colors
	List<string> clans;	//all clans in the world
	
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
		colors["Blue"] = new Color(0, 0, 1.0f); 
		colors["Red"] = new Color(1.0f, 0, 0);
		colors["Yellow"] = Color.yellow;
		colors["Purple"] = new Color(0.63f, 0.13f, 0.94f);
		colors["Orange"] = new Color(1.0f, 0.65f, 0);
		colors["Magenta"] = new Color(0.63f, 0.13f, 0.94f);
		colors["Green"] = new Color(0, 1.0f, 0);
		colors["Black"] = Color.black;
	}
	
	public void SetFact(string clan, string name, WorkingMemoryValue factValue) //set a new fact for a clan
	{
		bool alreadyKnown = false;
		foreach(WorkingMemoryValue vm in tribeFacts[clan].GetFact(name))
		{
			
			if(vm.GetFactValue().Equals( factValue.GetFactValue()))
			{
				//Already knows about it
				alreadyKnown = true;
				break;
			}
		}
		
		if(!alreadyKnown) //if we dont know the fact then add it
		{	
			tribeFacts[clan].SetFact(name, factValue);
		}

		if(name == "Agents") //if the fact is about a new agent increase the fact about number of agents
		{
			tribeFacts[clan].ChangeNumberAgentsInClan(1);
		}
	}
	
	public List<WorkingMemoryValue> GetFact(string clan, string name) //return a fact for a clan
	{
		return tribeFacts[clan].GetFact(name);			
	}
	
	public bool ContainsFact(string clan, string name) //check if clan knows a fact
	{
		return tribeFacts[clan].ContainsFact(name);
	}
	
	public void ChangeNumberAgentsInClan(string clan, int change) //change number of agents in a clan
	{
		tribeFacts[clan].ChangeNumberAgentsInClan(change);
	}
	
	public void AddToTaskTree(string clan, List<TreeNode> plan) //Add a new subtree with actions to the tasktree for a clan
	{
		tribeFacts[clan].AddToTaskTree(plan);
	}
		
	public string GetActionForAgent(string clan, Agent agent) //check if any action is avaiable that the agent can do and return it
	{
		return tribeFacts[clan].GetActionForAgent(agent);
	}
	
	public TaskTree GetTaskTree(string clan) //get the tasktree for a clan
	{
		return tribeFacts[clan].GetTaskTree();
	}
	
	public Color GetColorForObject(string colorName) //get the actual color object for a color string
	{
		return colors[colorName];		 
	}
	
	public void UpdateScore(string clan, string type) //update the score for a clan
	{
		tribeFacts[clan].UpdateScore(type);
	}
	
	public void SetScore(string clan, int score) //set the score for a clan
	{
		tribeFacts[clan].SetScore(score);
	}
	
	public int GetScore(string clan) //get the score for a clan
	{
		return tribeFacts[clan].GetScore();
	}
	
	public void SetCurrentWorldstate(string clan,  WorldState ws) //update the current worldState for a clan
	{
		WorldState currentWorldState = (WorldState)GetFact(clan, "currentWorldState")[0].GetFactValue();
		RemoveFact(clan, "currentWorldState", new WorkingMemoryValue(currentWorldState));
		
		WorldState tempWorldState = new WorldState ();
		
		foreach(KeyValuePair<string, WorldStateValue> newState in ws.GetProperties())
		{
		
			foreach (KeyValuePair<string, WorldStateValue> oldState in currentWorldState.GetProperties())
			{
				
				if(oldState.Key == newState.Key)
				{
					
					tempWorldState.SetProperty(newState.Key, newState.Value);
				} else{
				
					tempWorldState.SetProperty(oldState.Key, oldState.Value);
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
		
		
		clans.Add(clan);
		TribeFacts fact = new TribeFacts(clanColor, new TaskTree());
		tribeFacts.Add(clan, fact);
		
		//default worldstate for the clan
		WorldState currentWorldState = new WorldState();

		currentWorldState.SetProperty("blueResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.SetProperty("redResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.SetProperty("yellowResourceIsAvailable", new WorldStateValue(false));
		
		currentWorldState.SetProperty("orangeResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.SetProperty("greenResourceIsAvailable", new WorldStateValue(false));
		currentWorldState.SetProperty("magentaResourceIsAvailable", new WorldStateValue(false));
		
		SetFact(clan, "currentWorldState", new WorkingMemoryValue(currentWorldState));
		return clan;
	}
	
	public List<string> GetClans()//Returns all available clans 
	{
		return clans;
	}
	
	public void RemoveAgentFromOwnedNode(string clan, Agent agent)
	{
		tribeFacts[clan].RemoveAgentFromOwnedNode(agent);
	}
	
	private static int Compare(int x, int y) //compare to integers
	{
		if (x == y)
            return 0;
        else if (x > y)
            return -1;
        else
            return 1;
	}
	
	public List<string> SortClanScores() //sort the clan list based on score and returns it
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
	
	public void RemoveFact(string clan, string factName, WorkingMemoryValue factValue) //remove a fact for a clan
	{
		tribeFacts[clan].RemoveFact(factName, factValue);
	}
}