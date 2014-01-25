using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkingMemory {
	private Dictionary<string, List<WorkingMemoryValue>> knownFacts;
	private BlackBoard blackBoard;
	private string clan;
	
	public WorkingMemory(){
	
		knownFacts = new Dictionary<string, List<WorkingMemoryValue>>();
		blackBoard = BlackBoard.Instance;
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
		
		//Check if it is a globaly important fact that everyone needs to know about, then send it to the blackboard
		if(name == "Red" || name == "Blue" || name == "Yellow" || name == "Buildings" || name == "Orange"){
			blackBoard.SetFact(clan, name, factValue);
		}
	}
	
	public List<WorkingMemoryValue> GetFact(string name)
	{
		//If the fact is not in WorkingMemory the check if it is in the blackboard 		
		if(ContainsFact(name))
		{
			return knownFacts[name]; 
		} else {
			//check in blackboard
			List<WorkingMemoryValue> temp = blackBoard.GetFact(clan, name);
			return temp; //OBS! returnerar bara första värdet ur listan! 
		}
	}
	
	public bool ContainsFact(string name)
	{
		return knownFacts.ContainsKey(name);
	}
	
	public void RemoveFact(string name)
	{
		knownFacts.Remove(name);
	}
	
	public void PrintWorkingMemory()
	{
		Debug.Log("My memory contains: ");
		foreach(KeyValuePair<string, List<WorkingMemoryValue>> fact in knownFacts)
		{
			Debug.Log(fact.Key);
		}
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;
	}
}