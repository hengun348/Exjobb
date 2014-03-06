using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldStateValue {
	
	public Dictionary<string, object> propertyValues;
	
	public WorldStateValue(object objValue, object objValue2) //create a new worldstate with posibility of a boolean and a amount, ex. blueHouseIsBuilt = true requires 2 resources
	{
		propertyValues = new Dictionary<string, object>();
		
		this.propertyValues.Add("bool", objValue);
		this.propertyValues.Add("amount", objValue2);
	}
	
	public WorldStateValue(object objValue) //create a new worldstate with the default amount of 1
	{
		propertyValues = new Dictionary<string, object>();
		
		this.propertyValues.Add("bool", objValue);
		this.propertyValues.Add("amount", 1);
		
	}
	
	public void SetProperty(string objString, object objValue) //add a property to a worldstatevalue
	{
		this.propertyValues.Add(objString, objValue);
	}

}