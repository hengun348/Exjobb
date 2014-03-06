using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {
	
	private List<AStarNode> openList; //list of nodes to be considered
	private List<AStarNode> closedList; //list of nodes that you don’t need to look at again for now
	private List<AStarNode> pathList; //contains the full path from startNode to endNode
	
	public AStar()
	{
		openList = new List<AStarNode>();
		closedList = new List<AStarNode>();
		pathList = new List<AStarNode>();
	}
	
	public List<AStarNode> Run(AStarNode startNode, AStarNode endNode) //the AStar algorithm
	{
		AStarNode currentNode = startNode;
		
		//special case for the first node since it is NOT an action but a postCondition. We are counting on that it will never be a goal!
		List<AStarNode> neighbourList = currentNode.GetNeighbours(true); //gives a list with actions that has startNodes preCondition
		
		foreach(AStarNode node in neighbourList) //for each of these neighbours 
		{
			node.SetF(ActionManager.Instance.GetAction(node.GetName()).cost + HeuristicCost(node, endNode)); //calculate F of the neigbour
			node.SetParent(startNode); //Set the startNode as the neighbours parent
			openList.Add(node); //Add the neighbours of the startNode to the openList
		}

		//As long as we still have nodes in the openList
		while(openList.Count > 0)
		{
			//Take out the node with the lowest value for F in the openList
			int lowInd = 0;
			for(int i = 0; i < openList.Count; i++) 
			{
				if(openList[i].GetF() < openList[lowInd].GetF()) 
				{ 
					lowInd = i; 
				}
			}
			currentNode = openList[lowInd];
			openList.Remove(currentNode);
			closedList.Add(currentNode);
			
			
			
			//Check if we have reached the target 
			if(endNode.GetWorldState().Contains(ActionManager.Instance.GetAction(currentNode.GetName()).preConditions))
			{
				return CreatePath(currentNode, startNode);
			}
			
			
			//if action with preCondition with amount or several preConditions
			
			WorldState preConditions =  ActionManager.Instance.GetAction(currentNode.GetName()).preConditions;
			
			if(preConditions.ContainsAmount() || preConditions.GetProperties().Count > 1)
			{
				List<List<AStarNode>> lists = new List<List<AStarNode>>();
				
				//for each of the preConditions of the currentNodes action we need to start new AStars
				foreach(KeyValuePair<string, WorldStateValue> pair in preConditions.GetProperties())
				{
					//if an action contains a preCondition that has amount then we need to run the algorithm several times
					for(int j = 0; j < (int)pair.Value.propertyValues["amount"]; j++)
					{
						
						AStar astar2 = new AStar();
						
						AStarNode tempNode = new AStarNode();
						
						
						WorldState tempWorldState = new WorldState();
						tempWorldState.SetProperty(pair.Key, pair.Value);
						tempNode.SetWorldState(tempWorldState);
						
						List<AStarNode> tempList = new List<AStarNode>();
						
						tempList.AddRange(astar2.Run(tempNode, endNode)); //run the AStar for the preCondition
						
						tempList[tempList.Count-1].SetParent(currentNode); //set parent to action before fork
						
						if(tempList.Count > 0)
						{
							lists.Add(tempList); //add new plan to main plan
						}
						else 
						{
							if(!tempWorldState.Contains(endNode.GetWorldState()))
							{
								return new List<AStarNode>(); //unreachable goal for the algorithm, cant find goal
							}
						}
					}
				}
				
				//Sort by cost to make the plan prioritize actions with small costs
				lists.Sort((a, b) => ActionManager.Instance.GetAction(a[0].GetName()).cost.CompareTo(ActionManager.Instance.GetAction(b[0].GetName()).cost));
				
				foreach(List<AStarNode> list in lists)
				{
					pathList.AddRange(list); //add all branches to final path
				}
				
				return CreatePath(currentNode, startNode);
			}
			
			neighbourList = currentNode.GetNeighbours(false); //get neighbouring nodes
			
			for(int i = 0; i < neighbourList.Count; i++)
			{
				
				AStarNode currentNeighbour = neighbourList[i];
				if(closedList.Contains(currentNeighbour)) //We have already looked at this node
				{
					//not a valid node
					continue;
				}
				
				if(!openList.Contains(currentNeighbour)) //it can be an interesting node
				{
					currentNeighbour.SetH(HeuristicCost(currentNeighbour, endNode));
					currentNeighbour.SetG(currentNode.GetG() + currentNeighbour.GetTime()); 
					currentNeighbour.SetF(currentNeighbour.GetG() + currentNeighbour.GetH());
					openList.Add(currentNeighbour); //add it to the openList
				}	
			}
			
		}

		return new List<AStarNode>();
	}
	
	public float HeuristicCost(AStarNode currentNeighbour, AStarNode endNode){ //calculate H
		// See list of heuristics: http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
        /*var d1 = Math.abs (pos1.x - pos0.x);
        var d2 = Math.abs (pos1.y - pos0.y);
        return d1 + d2;*/
		float cost = ActionManager.Instance.GetAction(currentNeighbour.GetName()).cost + Random.Range(-3, 3); //could it be dangerous to use negative values?
		
		return cost;
	}
	
	public List<AStarNode> CreatePath(AStarNode currentNode, AStarNode endNode){ //create the full path from the startNode to the endNode

		while(!(ActionManager.Instance.GetAction(currentNode.GetName()).postConditions.Contains(endNode.GetWorldState())))
		{
			if(Object.ReferenceEquals(currentNode.GetParent().GetName(), null)){
				break;
			}
			else
			{
				pathList.Add(currentNode);
				currentNode = currentNode.GetParent();
			}
		}
		pathList.Add(currentNode);

		return pathList;
	}
}