using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner{
	
	
	public WorldState goalWorldState;
	private List<AStarNode> plan;
	//private string currentAgent;
	
	
	public List<AStarNode> runAStar(WorldState currentWorldState){
		
		
		
		
		AStarNode startNode = new AStarNode();
		AStarNode endNode = new AStarNode();
		
		startNode.worldState = goalWorldState;
		endNode.worldState = currentWorldState;
		
		AStar star = new AStar();
		

		
		plan = star.run(startNode, endNode);
		
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