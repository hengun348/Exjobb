using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action {
	
	public WorldState preConditions = new WorldState();
	public WorldState postConditions = new WorldState();
	public float cost; //The cost of an action
	public float time; //The time it takes to do the action
	protected string actionName;
	protected List<string> agentTypes;
	
	public List<string> GetAgentTypes() //Returns the agents that can do an action
	{
		return agentTypes;
	}
	
	public bool ContainsPreCondition(string condition, bool val) //Check if the action contains a precondition 
	{
		
		Dictionary<string, WorldStateValue> preConditionsList = preConditions.GetProperties();	
		
		foreach(KeyValuePair<string, WorldStateValue> pair in preConditionsList)
		{
			if(pair.Key == condition && pair.Value.propertyValues["bool"].Equals( val))
			{
				return true;
			}
			
		}
		return false;
	}
	
	public bool ContainsPostCondition(string condition, bool val) //Check if the action contains a postcondition 
	{
		Dictionary<string, WorldStateValue> postConditionsList = postConditions.GetProperties();	
		
		foreach(KeyValuePair<string, WorldStateValue> pair in postConditionsList)
		{
			if(pair.Key == condition && pair.Value.propertyValues["bool"].Equals( val))
			{
				return true;
			}
		}
		return false;
	}
	
	public string GetActionName() //Returns the name of the action
	{
		return actionName;
	}

}