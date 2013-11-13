using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackBoard {
	
	private static BlackBoard instance;
	private Dictionary<string, List<WorkingMemoryValue>> knownFacts;
	
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
	
	public BlackBoard(){
	
		knownFacts = new Dictionary<string, List<WorkingMemoryValue>>();
		
	}
	
	public void setFact(string name, WorkingMemoryValue factValue)
	{
		if(!knownFacts.ContainsKey(name)){
			List<WorkingMemoryValue> tempList = new List<WorkingMemoryValue>();
			tempList.Add(factValue);
			knownFacts.Add(name, tempList);
		} else {
		
			knownFacts[name].Add(factValue);
		}
	}
	
	public List<WorkingMemoryValue> getFact(string name)
	{
		
		//Ska vi returnera senaste faktan eller ska vi returnera hela listan med värden, OBS returnerar en tom lista om det inte finns någon fakta!
		
		if(containsFact(name))
		{	
				return knownFacts[name]; 
				
		} else {
		
			return new List<WorkingMemoryValue>();
		}
		
	}
	
	public bool containsFact(string name)
	{
		return knownFacts.ContainsKey(name);
	}
		
	void Start(){
	
	}
	
	
	void Update(){
	
	
		
	}
	
}