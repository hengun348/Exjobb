using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode {
	
	public float h { get; set; }
	public float g { get; set; }
	public float f { get; set; }
	public string name { get; set; }
	public Action action { get; set; }
	public List<AStarNode> suitableActions; //All the neighbouring nodes
	
	public AStarNode()
	{
		suitableActions = new List<AStarNode>();
	}
	
	public List<AStarNode> getNeighbours()
	{
		
		//returns a list of actions suitable for the goal(a string)
		List<Action> tempList = ActionManager.Instance.getSuitableActions(this.name);
		
		foreach(Action action in tempList)
		{
			
			AStarNode node = new AStarNode();
			node.name = action.actionName;
			suitableActions.Add(node);
			
		}
		
		return suitableActions;
		
	}
	
	public void addNeighbour(AStarNode node)
	{
		
		suitableActions.Add(node);
		
	}

}