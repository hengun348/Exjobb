using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeNode {
	
	private string actionName;
	private TreeNode parent;
	private int index;
	private Agent owner;
	private List<Agent> assistList;
	private Vector3 position;
	
	public TreeNode(Vector3 position, string actionName = "", TreeNode parent = null, int index = 0)
	{
		this.actionName = actionName;
		this.parent = parent;
		assistList = new List<Agent>();
		this.position = position;
		this.index = index;
	}
	
	public void AddAssister(Agent agent) //Alternativt skicka in en lista
	{
		assistList.Add(agent);
	}
	
	public TreeNode GetParent()
	{
		return parent;
	}
	
	public List<Agent> GetAssisters()
	{
		return assistList;
	}
	
	public string GetActionName()
	{
		
		return actionName;
	}
	
	public void SetOwner(Agent owner)
	{
		this.owner = owner;
	}
	
	public Agent GetOwner()
	{
		return owner;
	}
	
	public System.Guid GetOwnerNumber()
	{
		if(owner != null)
		{
			return owner.getAgentNumber();
		}
		
		return System.Guid.Empty;
	}
	
	public Vector3 GetPosition()
	{
		return position;
	}
	
	public int GetIndex()
	{
		return index;
	}
}