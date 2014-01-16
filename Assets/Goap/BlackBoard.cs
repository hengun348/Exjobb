using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackBoard {
	
	private static BlackBoard instance;
	private Dictionary<string, List<WorkingMemoryValue>> knownFacts;
	private TaskTree taskTree;
	private Dictionary<string, Color> colors;
	
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
	
	private BlackBoard(){
	
		knownFacts = new Dictionary<string, List<WorkingMemoryValue>>();
		
		
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
		
		SetFact("currentWorldState", new WorkingMemoryValue(currentWorldState));
		
		
		taskTree = new TaskTree();
		
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
	
	public void SetFact(string name, WorkingMemoryValue factValue)
	{
		if(!knownFacts.ContainsKey(name)){
			List<WorkingMemoryValue> tempList = new List<WorkingMemoryValue>();
			tempList.Add(factValue);
			knownFacts.Add(name, tempList);
		} else {
		
			knownFacts[name].Add(factValue);
		}
	}
	
	public List<WorkingMemoryValue> GetFact(string name)
	{
		
		//Ska vi returnera senaste faktan eller ska vi returnera hela listan med värden, OBS returnerar en tom lista om det inte finns någon fakta!
		
		if(ContainsFact(name))
		{	
				return knownFacts[name]; 
				
		} else {
		
			return new List<WorkingMemoryValue>();
		}
		
	}
	
	public bool ContainsFact(string name)
	{
		return knownFacts.ContainsKey(name);
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
	public void AddToTaskTree(AStarNode planStep, Vector3 position, int treeIndex)
	{
		taskTree.AddSubtree(planStep, position, treeIndex);
	}
		
	public string GetActionForAgent(Agent agent)
	{
		return taskTree.GetActionForAgent(agent);	
	}
	
	public TaskTree GetTaskTree()
	{
		return taskTree;
	}
	
	public Color GetColorForObject(string colorName)
	{
		
		return colors[colorName];		 
	}
}