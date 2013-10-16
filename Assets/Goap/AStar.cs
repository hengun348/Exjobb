using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {
	
	public List<AStarNode> openList;
	public List<AStarNode> closedList;
	public List<AStarNode> pathList;
	
	public List<AStarNode> runAStar(AStarNode startNode, AStarNode endNode)
	{
		openList.Add(startNode);
		pathList.Add(startNode);
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
			}
			currentNode = openList[lowInd];
 
			if(currentNode == endNode)
			{
				return pathList;
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
				
				float g_score = currentNode.g + 1.0f; //distance between nodes
				bool gScoreIsBest = false;
				
				if(!openList.Contains(currentNeighbour) )
				{
					gScoreIsBest = true;
					//currentNeighbour.h = heuristic_cost(currentNeighbour, endNode);
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
		return new List<AStarNode>();
	}
}