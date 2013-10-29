using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action {
	
	public WorldState preConditions;
	public WorldState postConditions;
	public float cost;
	public string actionName;
	protected List<string> agentTypes;
	
	public List<string> getAgentTypes()
	{
		return agentTypes;
	}
	
	public bool containsPreCondition(string condition, bool val)
	{
		Dictionary<string, bool> preConditionsList = preConditions.getProperties();	
		
		foreach(KeyValuePair<string, bool> pair in preConditionsList)
		{
			if(pair.Key == condition && pair.Value.Equals( val))
			{
				return true;
			}
		}
		return false;
	}
	
	public bool containsPostCondition(string condition, bool val)
	{
		Dictionary<string, bool> postConditionsList = postConditions.getProperties();	
		
		foreach(KeyValuePair<string, bool> pair in postConditionsList)
		{
			if(pair.Key == condition && pair.Value.Equals( val))
			{
				return true;
			}
		}
		return false;
	}
}