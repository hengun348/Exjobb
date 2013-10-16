using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkingMemory {
	
	public Dictionary<string, WorkingMemoryFact> knownFacts;
	
	public void setFact(string name, WorkingMemoryFact factValue)
	{
		knownFacts.Add(name, factValue);
	}
	
	public WorkingMemoryFact getFact(string name)
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