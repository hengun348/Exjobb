using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldStateProperty {
	
	public string propertyName;
	public WorldStateValue propertyValue;
	
	public WorldStateProperty(string name, WorldStateValue stateValue)
	{
		
		propertyName = name;
		propertyValue = stateValue;
		
	}
	
}