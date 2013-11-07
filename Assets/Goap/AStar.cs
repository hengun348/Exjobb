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
	
	public List<AStarNode> run(AStarNode startNode, AStarNode endNode)
	{
		AStarNode currentNode = startNode;
		Debug.Log("ADFSDFSDSDF");
		//Specialfall för första noden eftersom den INTE är något action utan en postCondition, räknar med att den aldrig kommer va mål!!!
		List<AStarNode> neighbourList = currentNode.getNeighbours(true); //ger lista med actions som har startNodes preCondition 	
		foreach(AStarNode node in neighbourList)
		{
			Debug.Log("AAAAAAAA : " + neighbourList.Count);
			node.f = ActionManager.Instance.getAction(node.name).cost + heuristic_cost(node, endNode);
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
			
			bool containsPreWithAmount = false;
			
			if( ActionManager.Instance.getAction(currentNode.name).preConditions.getProperties().Count == 1){
				foreach(KeyValuePair<string, WorldStateValue> pair in ActionManager.Instance.getAction(currentNode.name).preConditions.getProperties()){
				
						if((int)pair.Value.propertyValues["amount"] > 1){
						
							containsPreWithAmount = true;
						}
				
				}
			}
				
			//if multiple preconditions
			if(ActionManager.Instance.getAction(currentNode.name).preConditions.getProperties().Count > 1 || containsPreWithAmount)
			{
				List<List<AStarNode>> lists = new List<List<AStarNode>>();
				
				foreach(KeyValuePair<string, WorldStateValue> pair in ActionManager.Instance.getAction(currentNode.name).preConditions.getProperties())
				{
					Debug.Log("GDDDDDDDDDDDDDDDDDDDDDDDDDDDDD" + pair.Value.propertyValues["amount"]);
					for(int j = 0; j < (int)pair.Value.propertyValues["amount"]; j++)
					{
						Debug.Log ("IIIIIIIIIIIIII: " + j);
						AStar astar2 = new AStar();
						AStarNode tempNode = new AStarNode();
						
						WorldState tempWorldState = new WorldState();
						tempWorldState.setProperty(pair.Key, pair.Value);
						tempNode.worldState = tempWorldState;
						
						List<AStarNode> tempList = new List<AStarNode>();
						tempList.AddRange(astar2.run(tempNode, endNode));
						
						if(tempList.Count > 0)
						{
							lists.Add(tempList);
						}
						else 
						{
							if(!tempWorldState.contains(endNode.worldState))
							{
								return new List<AStarNode>();
							}
						}
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

					currentNeighbour.h = heuristic_cost(currentNeighbour, endNode);
					currentNeighbour.g = currentNode.g + currentNeighbour.time; 
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
		float cost = ActionManager.Instance.getAction(currentNeighbour.name).cost + Random.Range(-3, 3); //could it be dangerous to use negative values?
		
		return cost;
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