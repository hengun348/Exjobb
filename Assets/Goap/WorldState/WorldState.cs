using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState {
	
	//TODO: kontrollera hur worldstate igentligen ska vara uppbyggt(se setProperty tex)
	
	//string = propertyName, bool = propertyValue
	private Dictionary<string, bool> properties; 
	
	public WorldState()
	{
		properties = new Dictionary<string, bool>();
	}
	
	public Dictionary<string, bool> getProperties()
	{
		return properties;
	}
	
	public void setProperty(string name, bool stateValue)
	{
		properties.Add(name, stateValue);
	}
	
	public bool getValue(string name)
	{
		//returns the value (an bool)
		return properties[name];
	}
	
	public void removeProperty(string name)
	{
		properties.Remove(name);
	}
	
	public bool contains(WorldState ws)
	{
		Dictionary<string, bool> wsProperties = ws.getProperties();
		
		foreach(KeyValuePair<string, bool> pair in properties)
		{
			if(ws.getProperties().ContainsKey(pair.Key))
			{
				if(wsProperties[pair.Key].Equals(pair.Value))
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