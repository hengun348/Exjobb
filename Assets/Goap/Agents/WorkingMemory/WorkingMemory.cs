using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkingMemory {
	
	public Dictionary<string, WorkingMemoryValue> knownFacts;
	
	public WorkingMemory(){
	
		knownFacts = new Dictionary<string, WorkingMemoryValue>();
		
	}
	
	public void setFact(string name, WorkingMemoryValue factValue)
	{
		if(!knownFacts.ContainsKey(name)){
			knownFacts.Add(name, factValue);
		} else {
		
			knownFacts[name] = factValue;
		}
	}
	
	public WorkingMemoryValue getFact(string name)
	{
		return knownFacts[name]; 
	}
	
	public bool containsFact(string name)
	{
		return knownFacts.ContainsKey(name);
	}
	
	public void removeFact(string name)
	{
		knownFacts.Remove(name);
	}

}