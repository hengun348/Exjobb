using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkingMemoryValue {
	
	private object factValue;
	
	public WorkingMemoryValue(object factValue){
	
		this.factValue = factValue;
		
	}
	
	public object GetFactValue()
	{
		return factValue;
		
	}

}