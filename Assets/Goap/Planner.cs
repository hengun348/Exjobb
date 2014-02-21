using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner{
	
	
	private WorldState goalWorldState;
	//private string currentAgent;
	
	public void SetGoalWorldState(WorldState goalWorldState)
	{
		this.goalWorldState = goalWorldState;
	}
	
	public List<AStarNode> RunAStar(WorldState currentWorldState){
		
		
		AStarNode startNode = new AStarNode();
		AStarNode endNode = new AStarNode();
		
		startNode.SetWorldState(goalWorldState);
		endNode.SetWorldState(currentWorldState);
		
		AStar star = new AStar();
			
		
		List<AStarNode> plan = star.Run(startNode, endNode);
	
		
		//används under utvecklingsfasen
		/*Debug.Log("HÄR ÄR PLANEN!!!!!!!!: " + plan.Count);
		foreach(AStarNode node in plan)
		{
			Debug.Log(node.name);
		}*/
		//----------------------------
		
		/*foreach(AStarNode node in plan)
		{
			//TODO: dubbelkolla att returnerande action faktiskt finns
			blackBoard.setCurrentAction(node.name);
			
		}*/
		//blackBoard.setCurrentAction("");
		
		
		return plan;
	}
}