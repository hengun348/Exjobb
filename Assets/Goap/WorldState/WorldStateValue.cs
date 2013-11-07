using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldStateValue {
	
	//public object propertyValue;
	public Dictionary<string, object> propertyValues;
	
	public WorldStateValue(object objValue, object objValue2)
	{
		propertyValues = new Dictionary<string, object>();
		
		this.propertyValues.Add("bool", objValue);
		this.propertyValues.Add("amount", objValue2);
		//this.propertyValue = objValue;
		
	}
	
	public WorldStateValue(object objValue)
	{
		propertyValues = new Dictionary<string, object>();
		
		this.propertyValues.Add("bool", objValue);
		this.propertyValues.Add("amount", 1);
		//this.propertyValue = objValue;
		
	}
	
	public void setProperty(string objString, object objValue)
	{
		this.propertyValues.Add(objString, objValue);
	}

}