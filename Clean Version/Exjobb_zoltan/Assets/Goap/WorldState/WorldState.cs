using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState {
	
	private Dictionary<string, WorldStateValue> properties; //contains the properties belonging to this worldstate
	
	public WorldState() //create a empty worldstate
	{
		properties = new Dictionary<string, WorldStateValue>();
	}
	
	public WorldState(string name, WorldStateValue stateValue) //create a worldstate with one property
	{
		properties = new Dictionary<string, WorldStateValue>();
		properties.Add(name, stateValue);
	}
	
	public Dictionary<string, WorldStateValue> GetProperties() //get the properties of the worldstate
	{
		return properties;
	}
	
	public void SetProperty(string name, WorldStateValue stateValue) //add a property to the worldstate
	{
		if(!properties.ContainsKey(name)){ //if not contains the property, add it
			
			properties.Add(name, stateValue);
			
		} else { //if it contains it change its value
			
			properties[name] = stateValue;
		}		
	}
	
	public WorldStateValue GetValue(string name) //returns the corresponding worldstatevalue
	{
		return properties[name];
	}
	
	public void RemoveProperty(string name) //remove a property from the worldstate
	{
		properties.Remove(name);
	}
		
	public bool Contains(WorldState ws) //check if the worldstate contains the same properties as the worldstate ws (can still contain other as well)
	{
				
		Dictionary<string, WorldStateValue> wsProperties = ws.GetProperties();
		
		foreach(KeyValuePair<string, WorldStateValue> pair in wsProperties)
		{
			if(properties.ContainsKey(pair.Key)) //if both contains the same property key
			{
				
				if(properties[pair.Key].propertyValues["bool"].Equals(pair.Value.propertyValues["bool"])) //if they both also have the same value
				{
					continue;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		return true;
	}
	
	public bool ContainsAmount() //check if the worldstate contains a property with amount
	{ 
		
		foreach(KeyValuePair<string, WorldStateValue> pair in properties){
			if((int)pair.Value.propertyValues["amount"] > 1){
				
					return true;
				}
			}
		return false;
	}
}