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
	
	public List<AStarNode> getNeighbours(bool  firstTime)
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
			node.name = action.actionName;
			node.parent = this;
			node.time = action.time;
			
			suitableActions.Add(node);
		}
		return suitableActions;
	}
	
	public void addNeighbour(AStarNode node)
	{
		suitableActions.Add(node);
	}
	
	public void setH(float h)
	{
		this.h = h;
	}
	
	public void setF(float f)
	{
		this.f = f;
	}
	
	public void setG(float g)
	{
		this.g = g;
	}
	
	public float getH()
	{
		return h;
	}
	
	public float getF()
	{
		return f;
	}
	
	public float getG()
	{
		return g;
	}
	
	public void setName(string name)
	{
		this.name = name;
	}
	
	public void setParent(AStarNode parent)
	{
		this.parent = parent;
	}
	
	public void setTime(float time)
	{
		this.time = time;
	}
	
	public string getName()
	{
		return name;
	}
	
	public AStarNode getParent()
	{
		return parent;
	}
	
	public float getTime()
	{
		return time;
	}
	
	public WorldState getWorldState()
	{
		return worldState;
	}
	
	public void setWorldState(WorldState worldState)
	{
		this.worldState = worldState;
	}
}