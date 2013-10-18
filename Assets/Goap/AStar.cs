using UnityEngine;
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
		openList.Add(startNode);
		//pathList.Add(startNode);
		AStarNode currentNode = startNode;
		
		currentNode.g = 0.0f;
		currentNode.h = 0.0f;
		currentNode.f = 0.0f;
		while(openList.Count > 0)
		{
			int lowInd = 0;
			for(int i = 0; i < openList.Count; i++) 
			{
				if(openList[i].f < openList[lowInd].f) 
				{ 
					lowInd = i; 
				}
				Debug.Log ("lägsta index: " + lowInd);
			}
			currentNode = openList[lowInd];
 
			Debug.Log("currentNode.name: " + currentNode.name);
			Debug.Log("endNode.name: " + endNode.name);
			Debug.Log(currentNode.action);
			if(currentNode.action != null)
			{
				if(currentNode.action.containsPreCondition(endNode.name))
				{
					Debug.Log("Nu är vi i slutet1");
					return pathList;
				}
			}else{
				if(currentNode.name == endNode.name)
				{
					Debug.Log("Nu är vi i slutet2");
					return pathList;
				}
			}
			
			openList.Remove(currentNode);
			closedList.Add(currentNode);
			
			List<AStarNode> neighbourList = currentNode.getNeighbours();
			
			for(int i = 0; i < neighbourList.Count; i++)
			{
				AStarNode currentNeighbour = neighbourList[i];
				if(closedList.Contains(currentNeighbour))
				{
					//not a valid node
					continue;
				}
				Debug.Log ("antal grannar: " + neighbourList.Count);
				Debug.Log("currentnode: " + currentNode.name);
				float g_score = currentNode.g + 1.0f; //distance between nodes
				bool gScoreIsBest = false;
				
				if(!openList.Contains(currentNeighbour) )
				{
					Debug.Log("kontroll om currentneighbour inte finns i openlist: " + !openList.Contains(currentNeighbour));
					gScoreIsBest = true;
					currentNeighbour.h = heuristic_cost(currentNeighbour, endNode);
					openList.Add(currentNeighbour);
				}
				else if(g_score < currentNeighbour.g)
				{
					gScoreIsBest = true;
				}
				
				if(gScoreIsBest)
				{
					pathList.Add (currentNeighbour);
					currentNeighbour.g = g_score;
					currentNeighbour.f = currentNeighbour.g + currentNeighbour.h;
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
		return currentNeighbour.action.cost;
		
	}
	
}