using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode {
	
	private float h; //the estimated movement cost to move from that given node to the final destination
	private float g; //the movement cost to move from the starting point A to a given node
	private float f; //the sum of g and h
	private string name; 
	private AStarNode parent; //the nodes parent
	private float time;
	private WorldState worldState;
	private List<AStarNode> suitableActions; //All the neighbouring nodes

	
	public AStarNode()
	{
		suitableActions = new List<AStarNode>();
	}
	
	public List<AStarNode> GetNeighbours(bool  firstTime) //get the neighbours of this node
	{
		List<Action> tempList = new List<Action>();
		WorldState preConditions = new WorldState();
		
		if(firstTime == true)//if first node in the AStar then it will be a postCondition 
		{
			preConditions = worldState;
		}
		else{ //else it will be an action so get the preconditions of that action
			
			preConditions = ActionManager.Instance.GetAction(this.name).preConditions;
		}
		
		tempList = ActionManager.Instance.GetSuitableActions(preConditions);
		
		foreach(Action action in tempList)
		{
			AStarNode node = new AStarNode();
			node.name = action.GetActionName();
			node.parent = this;
			node.time = action.time;
			
			suitableActions.Add(node);
		}
		return suitableActions;
	}
	
	public void AddNeighbour(AStarNode node) //add a neighbour to the node 
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
	
	public List<AStarNode> GetChildren(List<AStarNode> planNodes) //Returns the children of the node
	{
		List<AStarNode> children = new List<AStarNode>();
		
		foreach(AStarNode node in planNodes)
		{
			if(node.GetParent() == this)
			{
				children.Add(node);
			}
		}	
		
		return children;
	}
}