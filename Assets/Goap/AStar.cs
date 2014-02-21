using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {
	
	private List<AStarNode> openList;
	private List<AStarNode> closedList;
	private List<AStarNode> pathList;
	
	public AStar()
	{
		openList = new List<AStarNode>();
		closedList = new List<AStarNode>();
		pathList = new List<AStarNode>();
	}
	
	public List<AStarNode> Run(AStarNode startNode, AStarNode endNode)
	{
		AStarNode currentNode = startNode;
		//Debug.Log("************ASTAR RUNNING************");
		//Specialfall för första noden eftersom den INTE är något action utan en postCondition, räknar med att den aldrig kommer va mål!!!
		List<AStarNode> neighbourList = currentNode.GetNeighbours(true); //ger lista med actions som har startNodes preCondition 	
		
		foreach(AStarNode node in neighbourList)
		{
			node.SetF(ActionManager.Instance.getAction(node.GetName()).cost + HeuristicCost(node, endNode));
			node.SetParent(startNode);
			openList.Add(node); //Lägg till grannarna för start i openList
		}

		//Ta ut nod med lägsta f ur openList
		while(openList.Count > 0)
		{
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
			
			//Debug.Log("currentNode är nu: " + currentNode.getName());
			
			//Check if we have reached the target 
			if(ActionManager.Instance.getAction(currentNode.GetName()).preConditions.contains(endNode.GetWorldState()))
			{
				return CreatePath(currentNode, startNode);
			}
			
			List<List<AStarNode>> lists = new List<List<AStarNode>>();
			
			foreach(KeyValuePair<string, WorldStateValue> pair in ActionManager.Instance.getAction(currentNode.GetName()).preConditions.getProperties())
			{
				//Debug.Log("GDDDDDDDDDDDDDDDDDDDDDDDDDDDDD" + pair.Value.propertyValues["amount"]);
				for(int j = 0; j < (int)pair.Value.propertyValues["amount"]; j++)
				{
					//Debug.Log ("IIIIIIIIIIIIII: " + j);
					AStar astar2 = new AStar();
					AStarNode tempNode = new AStarNode();
					
					
					WorldState tempWorldState = new WorldState();
					tempWorldState.setProperty(pair.Key, pair.Value);
					tempNode.SetWorldState(tempWorldState);
					
					List<AStarNode> tempList = new List<AStarNode>();
					
					tempList.AddRange(astar2.Run(tempNode, endNode));
					
					tempList[tempList.Count-1].SetParent(currentNode); //nyligen tillagd 
					
					//Debug.Log(".................." + tempList[0].getName() + " får parent " + currentNode.getName());
					
					if(tempList.Count > 0)
					{
						lists.Add(tempList);
					}
					else 
					{
						if(!tempWorldState.contains(endNode.GetWorldState()))
						{
							return new List<AStarNode>();
						}
					}
				}
			}
			
			//Sort by cost to make the plan prioritize actions with small costs
			lists.Sort((a, b) => ActionManager.Instance.getAction(a[0].GetName()).cost.CompareTo(ActionManager.Instance.getAction(b[0].GetName()).cost));
			
			foreach(List<AStarNode> list in lists)
			{
				pathList.AddRange(list);
			}
			return CreatePath(currentNode, startNode);
			
			neighbourList = currentNode.GetNeighbours(false);
			//Debug.Log ("antal grannar: " + neighbourList.Count);
			for(int i = 0; i < neighbourList.Count; i++)
			{
				//Debug.Log ("***********CurretnNode: " + currentNode.getName() + " neigbour " + neighbourList[i].getName());
				AStarNode currentNeighbour = neighbourList[i];
				if(closedList.Contains(currentNeighbour))
				{
					//not a valid node
					continue;
				}
				
				if(!openList.Contains(currentNeighbour))
				{
					//Debug.Log("namnet på grannen: " + currentNeighbour.getName());
					//Debug.Log("kontroll om currentneighbour inte finns i openlist: " + !openList.Contains(currentNeighbour));

					currentNeighbour.SetH(HeuristicCost(currentNeighbour, endNode));
					currentNeighbour.SetG(currentNode.GetG() + currentNeighbour.GetTime()); 
					currentNeighbour.SetF(currentNeighbour.GetG() + currentNeighbour.GetH());
					openList.Add(currentNeighbour);
				}	
			}
		}
		//Debug.Log ("det blev fel");
		return new List<AStarNode>();
	}
	
	public float HeuristicCost(AStarNode currentNeighbour, AStarNode endNode){
		// See list of heuristics: http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
        /*var d1 = Math.abs (pos1.x - pos0.x);
        var d2 = Math.abs (pos1.y - pos0.y);
        return d1 + d2;*/
		float cost = ActionManager.Instance.getAction(currentNeighbour.GetName()).cost + Random.Range(-3, 3); //could it be dangerous to use negative values?
		
		return cost;
	}
	
	public List<AStarNode> CreatePath(AStarNode currentNode, AStarNode endNode){
		
		while(!(ActionManager.Instance.getAction(currentNode.GetName()).postConditions.contains(endNode.GetWorldState())))
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