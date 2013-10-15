using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action : MonoBehaviour {
	
	//TODO: behövs det ett namn?
	public List<WorldStateProperty> preConditions;
	public List<WorldStateProperty> postConditions;
	public float cost;
	
	public void setPreCondition (WorldStateProperty condition) {
		preConditions.Add(condition);
	}
	
	public void setPostCondition (WorldStateProperty condition) {
		postConditions.Add(condition);
	}
	
	public void setCost (float cost) {
		this.cost = cost;
	}
	
}