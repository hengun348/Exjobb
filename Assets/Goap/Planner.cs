using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner{
	
	private WorldState currentWorldState;
	private WorldState goalWorldState;
	private List<Action> plan;
	
	public Planner(){
		Debug.Log("här börjar plannern");
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
			Debug.Log("slutmål: " + startNode.name);
		}
		
		foreach(KeyValuePair<string, object> pair in currentWorldState.properties)
		{
			
			endNode.name = pair.Key;
			Debug.Log("nuvarandemål: " + endNode.name);
		}
		
		AStar star = new AStar();
		
		List<AStarNode> templist = new List<AStarNode>();
		Debug.Log(startNode);
		Debug.Log(endNode);
		Debug.Log(star);
		templist = star.runAStar(startNode, endNode);
		Debug.Log("här är planen klar");
		plan = new List<Action>();
		foreach(AStarNode node in templist)
		{
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