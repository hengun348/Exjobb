using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action {
	
	public WorldState preConditions;
	public WorldState postConditions;
	public float cost;
	
	/*public void setPreCondition (WorldState condition) {
		preConditions.Add(condition);
	}
	
	public void setPostCondition (WorldState condition) {
		postConditions.Add(condition);
	}
	
	public void setCost (float cost) {
		this.cost = cost;
	}
	*/
}