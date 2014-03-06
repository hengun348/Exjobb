using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeNode {
	
	private string actionName;
	private TreeNode parent;
	private int index; //index to which subtree the node belongs to
	private Agent owner;
	private List<Agent> assistList; //asisters to carry out the action of the node
	private Vector3 position; //position where the action of the node is to be carried out
	
	public TreeNode(Vector3 position = new Vector3(), string actionName = "", TreeNode parent = null, int index = 0) //create a default node
	{
		this.actionName = actionName;
		this.parent = parent;
		assistList = new List<Agent>();
		this.position = position;
		this.index = index;
	}
	
	public void AddAssister(Agent agent) //Add an assister to the node
	{
		assistList.Add(agent);
	}
	
	public TreeNode GetParent()
	{
		return parent;
	}
	
	public void SetParent(TreeNode parent)
	{
		 this.parent = parent;
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
	
	public System.Guid GetOwnerNumber() //return the unique number of the agent that owns the node
	{
		if(owner != null)
		{
			return owner.GetAgentNumber();
		}
		
		return System.Guid.Empty;
	}
	
	public Vector3 GetPosition()
	{
		return position;
	}
	
	public void SetPosition(Vector3 position)
	{
		this.position = position;
	}
	
	public int GetIndex()
	{
		return index;
	}
}