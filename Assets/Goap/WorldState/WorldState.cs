using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState {
	
	//TODO: kontrollera hur worldstate igentligen ska vara uppbyggt(se setProperty tex)
	
	//string = propertyName, bool = propertyValue
	private Dictionary<string, WorldStateValue> properties; 
	
	public WorldState()
	{
		properties = new Dictionary<string, WorldStateValue>();
	}
	
	public Dictionary<string, WorldStateValue> getProperties()
	{
		return properties;
	}
	
	public void setProperty(string name, WorldStateValue stateValue)
	{
		properties.Add(name, stateValue);
	}
	
	public WorldStateValue getValue(string name)
	{
		//returns the corresponding worldstatevalue
		return properties[name];
	}
	
	public void removeProperty(string name)
	{
		properties.Remove(name);
	}
	
	public bool contains(WorldState ws)
	{
		Dictionary<string, WorldStateValue> wsProperties = ws.getProperties();
		
		foreach(KeyValuePair<string, WorldStateValue> pair in properties)
		{
			if(ws.getProperties().ContainsKey(pair.Key))
			{
				
				if(wsProperties[pair.Key].propertyValues["bool"].Equals(pair.Value.propertyValues["bool"]))
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
}