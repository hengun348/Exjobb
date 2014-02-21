using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TribeFacts: MonoBehaviour {
	
	private Dictionary<string, List<WorkingMemoryValue>> knownFacts;
	private TaskTree taskTree;
	private int score;
	private string clanColor;
	private int populationCap;
	private int agentsInClan;
	private List<string> colorsInClan;
	
	public TribeFacts(string clanColor, TaskTree taskTree){
		
		this.clanColor = clanColor;
		this.taskTree = taskTree;
		knownFacts = new Dictionary<string, List<WorkingMemoryValue>>();
		
		colorsInClan = new List<string> ();
		populationCap = 5;
		agentsInClan = 0;// ((UnitCommander)GameObject.Find(clanColor + " Clan").transform.FindChild("Commanders").FindChild("UnitCommander(Clone)")).GetAgents().Count;
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
	//public void AddToTaskTree(AStarNode planStep, Vector3 position, int treeIndex)
	public void AddToTaskTree(List<TreeNode> plan)
	{
		taskTree.AddSubtree(plan);
	}
		
	public string GetActionForAgent(Agent agent)
	{
		return taskTree.GetActionForAgent(agent);	
	}
	
	public TaskTree GetTaskTree()
	{
		return taskTree;
	}
	
	public void UpdateScore(string clan, string type)
	{
		//get color of type
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
		
		if(typeName == "Citizen")
		{
			if(typeColor != clanColor)
			{
				//bonuspoäng!
				score += 20;
				
			}
			else
			{
				//vanlig poängscore
				score += 10;
			}
			
		}
		else if(typeName == "House")
		{
			if(typeColor != clanColor)
			{
				//bonuspoäng!
				score += 30;
				
			}
			else
			{
				//vanlig poängscore
				score += 40;
			}
		}
		else if(typeName == "Factory")
		{
			
			List<string> agents = ActionManager.Instance.AssistingAgentsToAction(clanColor, "Build" + typeColor + "FactoryAction");
			
			if(agents.Count != 0)
			{
				//bonuspoäng!
				score += 50;
				
			}
			else
			{
				//vanlig poängscore
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
	
	public void RemoveFact(string factName, WorkingMemoryValue factValue)
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