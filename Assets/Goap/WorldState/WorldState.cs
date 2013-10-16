using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState {
	
	
	//TODO: kontrollera hur worldstate igentligen ska vara uppbyggt(se setProperty tex)
	public Dictionary<string, WorldStateProperty> properties;
	
	public Dictionary<string, WorldStateProperty> getProperties()
	{
		return properties;
	}
	
	public void setProperty(string name, WorldStateValue stateValue)
	{
		
		properties.Add(name, new WorldStateProperty(name, stateValue));
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