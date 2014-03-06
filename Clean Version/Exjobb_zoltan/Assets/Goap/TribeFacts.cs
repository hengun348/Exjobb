using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TribeFacts: MonoBehaviour {
	
	private Dictionary<string, List<WorkingMemoryValue>> knownFacts; //the facts the clan knows about
	private TaskTree taskTree; 
	private int score;
	private string clanColor; //the orignal color of the clan
	private int populationCap;
	private int agentsInClan;
	private List<string> colorsInClan; //available colors in the clan
	
	public TribeFacts(string clanColor, TaskTree taskTree){
		
		this.clanColor = clanColor;
		this.taskTree = taskTree;
		knownFacts = new Dictionary<string, List<WorkingMemoryValue>>();
		
		colorsInClan = new List<string> ();
		populationCap = 5;
		agentsInClan = 0;
	}
	
	public void SetFact(string name, WorkingMemoryValue factValue) //add a fact to the clan facts
	{
		if(!knownFacts.ContainsKey(name)){ //If this type of fact dont exist in the facts then need to add a new list with the fact of the new fact type, ex. knownFacts["Buildings"]
			List<WorkingMemoryValue> tempList = new List<WorkingMemoryValue>();
			tempList.Add(factValue);
			knownFacts.Add(name, tempList);
		} else { //The fact type already excist so att a new value for it
		
			knownFacts[name].Add(factValue);
		}
	}
	
	public List<WorkingMemoryValue> GetFact(string name) //returns a fact 
	{	
		//If the fact exist return it	
		if(ContainsFact(name))
		{	
			return knownFacts[name]; 
				
		} else { //else return an empty list!
		
			return new List<WorkingMemoryValue>();
		}
		
	}
	
	public bool ContainsFact(string name) //check if a fact is known
	{
		return knownFacts.ContainsKey(name);
	}
	
	public void AddToTaskTree(List<TreeNode> plan) //Add a new subtree with actions to the tasktree for a clan
	{
		taskTree.AddSubtree(plan);
	}
		
	public string GetActionForAgent(Agent agent) //check if any action is avaiable that the agent can do and return it
	{
		return taskTree.GetActionForAgent(agent);	
	}
	
	public TaskTree GetTaskTree() //get the tasktree for the clan
	{
		return taskTree;
	}
	
	public void UpdateScore(string type) //update the score for the clan
	{
		//get color and type
		string typeColor = "";
		string typeName = "";
		for (int i = 1; i <type.Length; i++) 
		{ 
			if (char.IsUpper(type[i])) 
			{ 	
				typeColor = type.Substring(0, i-1); 
				typeName = type.Substring(i, (type.Length -1) - (i-1));
				break; 
			} 
		}
		
		if(typeName == "Citizen") //if a citizen update score accordingly 
		{
			if(typeColor != clanColor) //if other color then clan
			{
				//bonuspoints!
				score += 20;
				
			}
			else
			{
				//normal points
				score += 10;
			}
			
		}
		else if(typeName == "House")
		{
			if(typeColor != clanColor)
			{
				score += 30;
			}
			else
			{
				score += 40;
			}
		}
		else if(typeName == "Factory")
		{
			
			List<string> agents = ActionManager.Instance.AssistingAgentsToAction(clanColor, "Build" + typeColor + "FactoryAction");
			
			if(agents.Count != 0)
			{
				score += 50;
			}
			else
			{
				score += 70;
			}
		}
		else if(typeName == "Black Tower")
		{
			score += 100;
		}
		else if(type == "Created Agent")
		{
			score -= 100;
		}
		else if(typeName == "Resource")
		{
			if(typeColor != clanColor)
			{
				score += 10;
			}
			else
			{
				score += 5;
			}
			
		}
	}
	
	public int GetScore()
	{
		return score;
	}
	
	public void SetScore(int score)
	{
		this.score = score;
	}
	
	public void RemoveAgentFromOwnedNode(Agent agent)
	{
		taskTree.RemoveAgentFromOwnedNode(agent);
	}
	
	public void IncreasePopulationCap()
	{
		populationCap++;
	}
	
	public void ChangeNumberAgentsInClan(int amountChange)
	{
		agentsInClan += amountChange;	
	}
	
	public int GetPopulationCap()
	{
		return populationCap;
	}
	
	public int GetAgentsInClan()
	{
		return agentsInClan;
	}
	
	public void AddColorToClan(string color)
	{
		colorsInClan.Add(color);
		
	}
	
	public void RemoveColorFromClan(string color)
	{
		colorsInClan.Remove(color);
	}
	
	public List<string> GetColorsInClan()
	{
		return colorsInClan;
	}
	
	public void RemoveFact(string factName, WorkingMemoryValue factValue) //remove fact from the facts of the clan
	{
		//tempList is needed for 'collection has changed' if we change directly in list
		List<WorkingMemoryValue> tempList = GetFact(factName);
		foreach(WorkingMemoryValue wm in GetFact(factName))
		{
			if(wm.GetFactValue() == factValue.GetFactValue())
			{
				tempList.Remove(wm);
				break;
			}
			else
			{
				//Do nothing
			}
		}
		if(tempList.Count == 0)
		{
			knownFacts.Remove(factName);
		}
		else
		{
			knownFacts[factName] = tempList;
		}
	}
}