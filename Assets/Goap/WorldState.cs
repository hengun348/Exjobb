using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState {
	
	public Dictionary<string, WorldStateProperty> properties;
	
	public Dictionary<string, WorldStateProperty> getProperties()
	{
		return properties;
	}
	
	public void setProperty(string name, WorldStateProperty property)
	{
		properties.Add(name, property);
	}
	
	public WorldStateProperty getProperty(string name)
	{
		return properties[name];
	}
	
	public void removeProperty(string name)
	{
		properties.Remove(name);
	}
	
}