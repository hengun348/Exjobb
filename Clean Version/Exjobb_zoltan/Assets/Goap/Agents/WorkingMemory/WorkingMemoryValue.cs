using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkingMemoryValue {
	
	private object factValue; //The value of a working memory, can be of any type hence its an object
	
	public WorkingMemoryValue(object factValue){
	
		this.factValue = factValue;
	}
	
	public object GetFactValue()
	{
		return factValue;
		
	}

}