using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState {
	
	//TODO: kontrollera hur worldstate igentligen ska vara uppbyggt(se setProperty tex)
	
	//string = propertyName, object = propertyValue
	public Dictionary<string, object> properties; 
	
	public WorldState()
	{
		properties = new Dictionary<string, object>();
	}
	
	public Dictionary<string, object> getProperties()
	{
		
		return properties;
		
	}
	
	public void setProperty(string name, object stateValue)
	{
		
		properties.Add(name, stateValue);
		
	}
	
	public object getValue(string name)
	{
		
		//returns the value (an object)
		return properties[name];
		
	}
	
	public void removeProperty(string name)
	{
		
		properties.Remove(name);
		
	}
	
}