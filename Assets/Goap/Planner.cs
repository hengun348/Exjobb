using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner{
	
	private WorldState currentWorldState;
	private WorldState goalWorldState;
	private List<AStarNode> plan;
	private string currentAgent;
	
	public List<AStarNode> runAStar(string agentType){
		
		currentAgent = agentType;
		
		
		currentWorldState = new WorldState();
		//currentWorldState.setProperty("enemyVisible", false);
		currentWorldState.setProperty("armedWithGun", true);
		/*currentWorldState.setProperty("weaponLoaded", false);
		currentWorldState.setProperty("enemyLinedUp", false);*/
		currentWorldState.setProperty("enemyAlive", true);
		currentWorldState.setProperty("armedWithBomb", false);
		//currentWorldState.setProperty("nearEnemy", false);
		currentWorldState.setProperty("agentAlive", true);
		
		goalWorldState = new WorldState();
		goalWorldState.setProperty("enemyAlive", false);
		//goalWorldState.setProperty("agentAlive", true); //------ HADE GLÖMT DENNA HAHA! BUMMER!
		
		AStarNode startNode = new AStarNode();
		AStarNode endNode = new AStarNode();
		
		startNode.worldState = goalWorldState;
		endNode.worldState = currentWorldState;
		
		AStar star = new AStar();
		

		ActionManager.Instance.currentAgent = currentAgent;
		plan = star.runAStar(startNode, endNode);
		
		//används under utveklingsfasen
		Debug.Log("HÄR ÄR PLANEN!!!!!!!!: " + plan.Count);
		foreach(AStarNode node in plan)
		{
			Debug.Log(node.name);
		}
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