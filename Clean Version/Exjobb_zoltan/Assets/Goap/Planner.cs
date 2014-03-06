using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner{
	
	private WorldState goalWorldState; //the goal of the planner
	
	public void SetGoalWorldState(WorldState goalWorldState) //set the goal worldstate
	{
		this.goalWorldState = goalWorldState;
	}
	
	public List<AStarNode> RunAStar(WorldState currentWorldState){ //get a plan to get from goal to current state
		
		AStarNode startNode = new AStarNode();
		AStarNode endNode = new AStarNode();
		
		//Debug!!!!
		foreach(KeyValuePair<string, WorldStateValue> pair in goalWorldState.GetProperties())
		{
			startNode.SetName(pair.Key);
		}
		
		
		startNode.SetWorldState(goalWorldState); //startnode is the goal
		endNode.SetWorldState(currentWorldState); //end node is the current WorldState
		
		AStar star = new AStar();
		
		List<AStarNode> plan = star.Run(startNode, endNode); //run the AStar to get a plan

		return plan;
	}
}