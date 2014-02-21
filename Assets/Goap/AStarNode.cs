using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode {
	
	private float h;
	private float g;
	private float f;
	private string name;
	private AStarNode parent;
	private float time;
	private WorldState worldState;
	private List<AStarNode> suitableActions; //All the neighbouring nodes

	
	public AStarNode()
	{
		suitableActions = new List<AStarNode>();
	}
	
	public List<AStarNode> GetNeighbours(bool  firstTime)
	{
		List<Action> tempList = new List<Action>();
		WorldState preConditions = new WorldState();
		
		if(firstTime == true)//returns a list of actions suitable for the goal(a string)
		{
			preConditions = worldState;
		}
		else{
			preConditions = ActionManager.Instance.getAction(this.name).preConditions;
		}
		
		//go thru postConditions for this action
		//TODO: gör så det funkar för fler postConditions
		/*foreach(KeyValuePair<string, bool> pair in preConditions.getProperties())
		{
			tempList = ActionManager.Instance.getSuitableActions(pair.Key, pair.Value, preConditions);
		}*/
		
		tempList = ActionManager.Instance.getSuitableActions(preConditions);
		
		//Debug.Log("templist count: " + tempList.Count);
		foreach(Action action in tempList)
		{
			//Debug.Log("templist name: " + action.actionName);
			AStarNode node = new AStarNode();
			node.name = action.GetActionName();
			node.parent = this;
			node.time = action.time;
			
			suitableActions.Add(node);
		}
		return suitableActions;
	}
	
	public void AddNeighbour(AStarNode node)
	{
		suitableActions.Add(node);
	}
	
	public void SetH(float h)
	{
		this.h = h;
	}
	
	public void SetF(float f)
	{
		this.f = f;
	}
	
	public void SetG(float g)
	{
		this.g = g;
	}
	
	public float GetH()
	{
		return h;
	}
	
	public float GetF()
	{
		return f;
	}
	
	public float GetG()
	{
		return g;
	}
	
	public void SetName(string name)
	{
		this.name = name;
	}
	
	public void SetParent(AStarNode parent)
	{
		this.parent = parent;
	}
	
	public void SetTime(float time)
	{
		this.time = time;
	}
	
	public string GetName()
	{
		return name;
	}
	
	public AStarNode GetParent()
	{
		return parent;
	}
	
	public float GetTime()
	{
		return time;
	}
	
	public WorldState GetWorldState()
	{
		return worldState;
	}
	
	public void SetWorldState(WorldState worldState)
	{
		this.worldState = worldState;
	}
}