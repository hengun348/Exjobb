using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode : MonoBehaviour {
	
	public float h;
	public float g;
	public float f;
	
	public List<AStarNode> neighbours;
	
	public List<AStarNode> getNeighbours()
	{
		return neighbours;
	}
	
	public void addNeighbour(AStarNode node)
	{
		neighbours.Add(node);
	}
}