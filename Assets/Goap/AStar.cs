﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {
	
	public List<AStarNode> openList;
	public List<AStarNode> closedList;
	public List<AStarNode> pathList;
	
	public AStar()
	{
		openList = new List<AStarNode>();
		closedList = new List<AStarNode>();
		pathList = new List<AStarNode>();
	}
	
	public List<AStarNode> runAStar(AStarNode startNode, AStarNode endNode)
	{
		AStarNode currentNode = startNode;
		
		//Specialfall för första noden eftersom den INTE är något action utan en postCondition, räknar med att den aldrig kommer va mål!!!
		List<AStarNode> neighbourList = currentNode.getNeighbours(true); //ger lista med actions som har startNodes preCondition 	
		foreach(AStarNode node in neighbourList)
		{
			node.f = ActionManager.Instance.getAction(node.name).cost + 1.0f; //TODO: sätta slumptal istället för +1?
			node.parent = startNode;
			openList.Add(node); //Lägg till grannarna för start i openList
		}
	
		//Ta ut nod med lägsta f ur openList
		while(openList.Count > 0)
		{
			int lowInd = 0;
			for(int i = 0; i < openList.Count; i++) 
			{
				if(openList[i].f < openList[lowInd].f) 
				{ 
					lowInd = i; 
				}
			}
			currentNode = openList[lowInd];
			openList.Remove(currentNode);
			closedList.Add(currentNode);
			
			Debug.Log("currentNode är nu: " + currentNode.name);
			
			//Check if we have reached the target 
			if(ActionManager.Instance.getAction(currentNode.name).preConditions.contains(endNode.worldState))
			{
				return createPath(currentNode, startNode);
			}
			
			if(ActionManager.Instance.getAction(currentNode.name).preConditions.getProperties().Count > 1)
			{
				List<List<AStarNode>> lists = new List<List<AStarNode>>();
				
				foreach(KeyValuePair<string, bool> pair in ActionManager.Instance.getAction(currentNode.name).preConditions.getProperties())
				{
					AStar astar2 = new AStar();
					AStarNode tempNode = new AStarNode();
					
					WorldState tempWorldState = new WorldState();
					tempWorldState.setProperty(pair.Key, pair.Value);
					tempNode.worldState = tempWorldState;
					
					List<AStarNode> tempList = new List<AStarNode>();
					tempList.AddRange(astar2.runAStar(tempNode, endNode));
					
					if(tempList.Count > 0)
					{
						lists.Add(tempList);
					}
				}
				
				lists.Sort((a, b) => ActionManager.Instance.getAction(a[0].name).cost.CompareTo(ActionManager.Instance.getAction(b[0].name).cost));
				
				foreach(List<AStarNode> list in lists)
				{
					pathList.AddRange(list);
				}
				return createPath(currentNode, startNode);
			}
			
			neighbourList = currentNode.getNeighbours(false);
			Debug.Log ("antal grannar: " + neighbourList.Count);
			for(int i = 0; i < neighbourList.Count; i++)
			{
				AStarNode currentNeighbour = neighbourList[i];
				if(closedList.Contains(currentNeighbour))
				{
					//not a valid node
					continue;
				}
				
				if(!openList.Contains(currentNeighbour))
				{
					Debug.Log("namnet på grannen: " + currentNeighbour.name);
					Debug.Log("kontroll om currentneighbour inte finns i openlist: " + !openList.Contains(currentNeighbour));
					//gScoreIsBest = true;
					currentNeighbour.h = heuristic_cost(currentNeighbour, endNode);
					currentNeighbour.g = currentNode.g + 1.0f;
					currentNeighbour.f = currentNeighbour.g + currentNeighbour.h;
					openList.Add(currentNeighbour);
				}	
			}
		}
		Debug.Log ("det blev fel");
		return new List<AStarNode>();
	}
	
	public float heuristic_cost(AStarNode currentNeighbour, AStarNode endNode){
		// See list of heuristics: http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
        /*var d1 = Math.abs (pos1.x - pos0.x);
        var d2 = Math.abs (pos1.y - pos0.y);
        return d1 + d2;*/
		return ActionManager.Instance.getAction(currentNeighbour.name).cost;
	}
	
	public List<AStarNode> createPath(AStarNode currentNode, AStarNode endNode){
		while(!(ActionManager.Instance.getAction(currentNode.name).postConditions.contains(endNode.worldState)))
		{
			if(Object.ReferenceEquals(currentNode.parent.name, null)){
				break;
			}
			else
			{
				pathList.Add(currentNode);
				currentNode = currentNode.parent;
			}
		}
		pathList.Add(currentNode);

		return pathList;
	}
}