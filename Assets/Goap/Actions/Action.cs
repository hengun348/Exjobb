using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action {
	
	public WorldState preConditions;
	public WorldState postConditions;
	public float cost;
	public float time {get; set; }
	public string actionName;
	protected List<string> agentTypes;
	
	public List<string> getAgentTypes()
	{
		return agentTypes;
	}
	
	public bool containsPreCondition(string condition, bool val)
	{
		
		Dictionary<string, WorldStateValue> preConditionsList = preConditions.getProperties();	
		
		foreach(KeyValuePair<string, WorldStateValue> pair in preConditionsList)
		{
			if(pair.Key == condition && pair.Value.propertyValues["bool"].Equals( val))
			{
				return true;
			}
			
		}
		return false;
	}
	
	public bool containsPostCondition(string condition, bool val)
	{
		Dictionary<string, WorldStateValue> postConditionsList = postConditions.getProperties();	
		
		foreach(KeyValuePair<string, WorldStateValue> pair in postConditionsList)
		{
			if(pair.Key == condition && pair.Value.propertyValues["bool"].Equals( val))
			{
				return true;
			}
		}
		return false;
	}
}