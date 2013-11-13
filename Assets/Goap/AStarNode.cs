using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode {
	
	public float h { get; set; }
	public float g { get; set; }
	public float f { get; set; }
	public string name { get; set; }
	public AStarNode parent {get; set;}
	public float time {get; set; }
	
	public WorldState worldState {get; set;}

	//public Action action { get; set; }
	public List<AStarNode> suitableActions; //All the neighbouring nodes
	
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
}