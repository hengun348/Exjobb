using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkingMemory {
	private Dictionary<string, List<WorkingMemoryValue>> knownFacts; //All the facts the agent knows about
	private BlackBoard blackBoard;
	private string clan; //The clan the agent belongs to
	
	public WorkingMemory(){
	
		knownFacts = new Dictionary<string, List<WorkingMemoryValue>>();
		blackBoard = BlackBoard.Instance;
	}
	
	public void SetFact(string name, WorkingMemoryValue factValue)  //Adds a new fact to the workingmemory of the agent
	{
		if(!knownFacts.ContainsKey(name)){ //If this type of fact dont exist in workingmemory then need to add a new list with the fact of the new fact type, ex. knownFacts["Buildings"]
			List<WorkingMemoryValue> tempList = new List<WorkingMemoryValue>();
			tempList.Add(factValue);
			knownFacts.Add(name, tempList);
		} else { //The fact type already excist 
			
			//Check if we already have the same value in the memory
			bool alreadyKnown = false;
			
			foreach(WorkingMemoryValue vm in knownFacts[name])
			{
				if(vm.GetFactValue().Equals(factValue.GetFactValue()))
				{
					//Already knows about it
					alreadyKnown = true;
					break;
				}
			}
			
			if(!alreadyKnown) //If we dont have it then add it
			{	
				knownFacts[name].Add(factValue);
			}

		}
		
		//Check if it is a globaly important fact that everyone needs to know about, then send it to the blackboard
		//if(name == "Red" || name == "Blue" || name == "Yellow" || name == "Buildings" || name == "Orange" || name == "Green" || name == "Magenta")
		//{
			//Also add the fact to the global blackboard for the clan so everyone knows about it
			blackBoard.SetFact(clan, name, factValue);
		//}
	}
	
	public List<WorkingMemoryValue> GetFact(string name) //Returns a fact from the workingmemory
	{
		//If the fact is in the workingmemory return it	
		if(ContainsFact(name))
		{
			return knownFacts[name]; 
			
		} else { //if not in WorkingMemory the check if it is in the blackboard 	
			
			List<WorkingMemoryValue> temp = blackBoard.GetFact(clan, name);
			return temp; //OBS! returnerar bara första värdet ur listan! 
		}
	}
	
	public bool ContainsFact(string name)
	{
		return knownFacts.ContainsKey(name);
	}
	
	public void RemoveFact(string factName, WorkingMemoryValue factValue) //Removes a fact from the workingmemory
	{
		//tempList is needed for 'collection has changed' error if we change directly in list
		List<WorkingMemoryValue> tempList = GetFact(factName);
		foreach(WorkingMemoryValue wm in GetFact(factName))
		{
			if(wm.GetFactValue().Equals(factValue.GetFactValue()))
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
		
		blackBoard.RemoveFact(clan, factName, factValue); //also remove the fact from the blackboard
		
	}
	
	public void PrintWorkingMemory() //Prints the workingmemory fact names and how many values each fact has 
	{
		Debug.Log("My memory contains: ");
		foreach(KeyValuePair<string, List<WorkingMemoryValue>> fact in knownFacts)
		{
			Debug.Log(fact.Key + knownFacts[fact.Key].Count);
		}
	}
	
	public void SetClan(string clan) //set the clan
	{
	 	this.clan = clan;
	}
}