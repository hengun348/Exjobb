using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskTree : MonoBehaviour {

	TreeNode root;
	List<TreeNode> leafs; //A list of the leafs of the tree 
	
	public TaskTree()
	{
		root = new TreeNode(new Vector3(0 , 30, 0)); //Dummy node
		leafs = new List<TreeNode>();
	}
	
	public void AddSubtree(AStarNode planStep, Vector3 position, int treeIndex) //adds a new plan to the tree as a subtree to the root node
	{
		
		if(planStep.getParent().getName() == null)
		{
			leafs.Add(new TreeNode(position, planStep.getName(), root, treeIndex)); //Add first node in tree to the root
		} else {	
			int a = leafs.Count;
			for(int i = 0; i<a; i++)
			{
				if( planStep.getParent().getName() == leafs[i].GetActionName() && leafs[i].GetIndex() == treeIndex)
				{
					leafs.Add(new TreeNode(position, planStep.getName(), leafs[i], treeIndex));
					leafs.Remove(leafs[i]);
						
				}else if (planStep.getParent().getName() == leafs[i].GetParent().GetActionName() && leafs[i].GetParent().GetIndex() == treeIndex)
				{
					leafs.Add(new TreeNode(position, planStep.getName(), leafs[i].GetParent(), treeIndex));
				}
			}
		}
		PrintTree();
	}
	
	public void PrintTree()
	{
		string level = "Leafs: ";
		foreach(TreeNode node in leafs)
		{
			level += node.GetActionName() + " ";
			
		}
		Debug.Log ("**LÖÖÖÖÖÖV" + level);	
	}

	
	public string GetActionForAgent(Agent agent) //Traverse the leafs of the tree to find a suitable task for this agent
	{
		foreach(TreeNode leaf in leafs)
		{
			List<string> agents = ActionManager.Instance.AgentsThatDoAction(agent.getAgentType(), leaf.GetActionName());
			
			if(agents.Count > 0 && leaf.GetOwnerNumber() == System.Guid.Empty )
			{
				leaf.SetOwner(agent);
				return leaf.GetActionName();
			}
			else if(agents.Count > 1 && leaf.GetOwnerNumber() != System.Guid.Empty && agent.getAgentType() != leaf.GetOwner().getAgentType())
			{
				bool okToAssist = true;
				List<Agent> assisters = leaf.GetAssisters();
				foreach(Agent assister in assisters)
				{
					if(assister.getAgentType() == agent.getAgentType()) 
					{
						okToAssist = false;
					}
				}
				
				if(okToAssist)
				{
					leaf.GetAssisters().Add(agent);
					//Debug.Log("AGNETSTHATDOACVTION " + agents[0] + " " + agents[1] + " for action " + leaf.GetActionName());
					return "AssistingAction";
				}
			}
		}
			

		return "Idle"; //empty string means no match, agent cant to any of the leaf-node-actions 
	}
	
	//returns node that agent owns in the leafs
	public TreeNode GetOwnedNode(System.Guid agent)
	{
		for(int i = 0; i < leafs.Count; i++)
		{
			if(leafs[i].GetOwnerNumber().Equals(agent))
			{
				return leafs[i];
			}
		}
		
		for(int i = 0; i < leafs.Count; i++)
		{
			List<Agent> assisters = leafs[i].GetAssisters();
			foreach (Agent assister in assisters)
			{
				if(assister.getAgentNumber() == agent)
				{
					return leafs[i];
				}
			}
		}
		
		return new TreeNode(new Vector3(30, 0.5f, 30));
	}
	
	//Removes a node in the leafs that this agent owns and adds its parent 
	public void RemoveNode(System.Guid agent)
	{
		TreeNode ownedNode = GetOwnedNode(agent);
		List<Agent> assisters = ownedNode.GetAssisters();
		foreach(Agent assister in assisters)
		{
			assister.GetPlan()[0] = "Idle";
			Destroy(assister.transform.FindChild("Subsystem").gameObject);
		}
		leafs.Remove(ownedNode);
		if(ownedNode.GetParent() != null && ownedNode.GetParent() != root && CheckForChildren(ownedNode.GetParent()) == false)
		{
			leafs.Add(ownedNode.GetParent());
			PrintTree();
		}	
	}
	
	//Checks if this node is a parent to one or more of the leafs
	public bool CheckForChildren(TreeNode node)
	{
		foreach(TreeNode leaf in leafs)
		{
			if(leaf.GetParent().Equals(node))
			{
				return true;
			}
		}
		return false;
	}
	
}