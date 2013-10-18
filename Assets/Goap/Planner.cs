using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner{
	
	private WorldState currentWorldState;
	private WorldState goalWorldState;
	private List<Action> plan;
	
	public Planner(){
		
		currentWorldState = new WorldState();
		currentWorldState.setProperty("needToJump", true);
		goalWorldState = new WorldState();
		goalWorldState.setProperty("hasJumped", true);
		
		AStarNode startNode = new AStarNode();
		AStarNode endNode = new AStarNode();
		
		//TODO: hur funkar det med flera mål?
		foreach(KeyValuePair<string, object> pair in goalWorldState.properties)
		{
			
			startNode.name = pair.Key;
			
		}
		
		foreach(KeyValuePair<string, object> pair in currentWorldState.properties)
		{
			
			endNode.name = pair.Key;

		}
		
		AStar star = new AStar();
		
		List<AStarNode> templist = new List<AStarNode>();
		templist = star.runAStar(startNode, endNode);
		Debug.Log("här är planen klar");
		plan = new List<Action>();
		foreach(AStarNode node in templist)
		{
			Debug.Log(node.action.actionName);
			Debug.Log(ActionManager.Instance.getAction(node.action.actionName).actionName);
			//TODO: dubbelkolla att returnerande action faktiskt finns
			plan.Add(ActionManager.Instance.getAction(node.action.actionName));
			
		}
		Debug.Log("här ska planen skrivas ut," + plan.Count);
		foreach(Action action in plan)
		{
			Debug.Log(action.actionName);
		}
		
	}
	
}