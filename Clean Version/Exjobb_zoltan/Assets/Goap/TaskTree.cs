using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskTree : MonoBehaviour{

	TreeNode root; //the root (top) node
	List<TreeNode> leafs; //A list of the leafs of the tree 
	List<List<TreeNode>> tree; //the actual tasktree
	
	public TaskTree()
	{
		leafs = new List<TreeNode>();
		tree = new List<List<TreeNode>>();
	}
	
	public void AddSubtree(List<TreeNode> plan) //add a new subtree of actions of a plan to the tree
	{	
		
		foreach(TreeNode planStep in plan)
		{
			int levelForNode = 0;
			TreeNode node = planStep;
			
			while(node.GetParent() != null)
			{
				node = node.GetParent();
				levelForNode++;
			}
			
			if(levelForNode > tree.Count-1)
			{
				List<TreeNode> tempList = new List<TreeNode> ();
				tempList.Add(planStep);
				tree.Insert(levelForNode, tempList);
				
			} else {
				List<TreeNode> tempList = tree[levelForNode];
				tree.Remove(tempList);
				tempList.Add(planStep);
				
				tree.Insert(levelForNode, tempList);
			}
			
		}
		
		//add leafs
		foreach(TreeNode node in tree[tree.Count-1])
		{
			if(node.GetIndex() == plan[0].GetIndex())
			{
				leafs.Add(node);
			}
		}

		for(int i = tree.Count-1; i > 0; i--)
		{
			foreach(TreeNode node in tree[i-1])
			{
				if(HasChild(tree[i], node) == false) //has no childs == is a leaf
				{
					leafs.Add(node);
				}
			}
		}
	}
	
	
	public void PrintLeafs() //print the leafs
	{
		
		string temp = "-----------Leafs: ";
		
		foreach(TreeNode node in leafs)
		{
			temp += node.GetActionName() + " "; 	
		}
		
		Debug.Log (temp);
		
	}
	
	public void PrintTree() //print the content of the tree
	{
		Debug.Log("*************Tree**************");
		
		Debug.Log ("------Tree Count: " + tree.Count + "-------");
		
		foreach(List<TreeNode> treelevel in tree )
		{
			
			string currentLevel = "";
			foreach(TreeNode levelNode in treelevel)
			{
				currentLevel += levelNode.GetActionName() + " "; 
			}
			
			Debug.Log(currentLevel);
		}
		
		PrintLeafs();
		
		Debug.Log("*******************************");
		
	}

	
	public string GetActionForAgent(Agent agent) //Traverse the leafs of the tree to find a suitable task for this agent
	{
		
		//Check if the agent already has a node as its own
		foreach(TreeNode leaf in leafs)
		{
			if(leaf.GetOwner() != null && leaf.GetOwner().GetAgentNumber() == agent.GetAgentNumber()) //if agent already owns a node
			{
				return leaf.GetActionName();
			} 
			else if(leaf.GetOwner() != null) { //if an agent already is assisting on a node
				
				if(OkayToAssist(agent, leaf, 2))
				{
					return "AssistingAction";
				}
			}
		}
		
		//not already an assister or owner of node then check if agent can get a new node
		foreach(TreeNode leaf in leafs)
		{
			List<string> agents = ActionManager.Instance.AssistingAgentsToAction(agent.GetAgentType(), leaf.GetActionName());
			
			if(agents.Count > 0 && leaf.GetOwnerNumber() == System.Guid.Empty ) //agent can own the node
			{
				leaf.SetOwner(agent);
				return leaf.GetActionName();
			}
			else if(agents.Count > 1 && leaf.GetOwnerNumber() != System.Guid.Empty && agent.GetAgentType() != leaf.GetOwner().GetAgentType()) //agent can assist on the node
			{
				
				if(OkayToAssist(agent, leaf, 1))
				{
					leaf.GetAssisters().Add(agent);
					return "AssistingAction";
				}
			}
		}
			

		return "IdleAction"; //agent cant to any of the leaf-node-actions 
	}
	
	public TreeNode GetOwnedNode(System.Guid agent) //returns node that agent owns in the leafs
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
				if(assister.GetAgentNumber() == agent)
				{
					return leafs[i];
				}
			}
		}
		
		return new TreeNode(new Vector3(30, 0.5f, 30));
	}
	
	public void RemoveNode(Agent agent) //Removes a node in the leafs that this agent owns and adds its parent to the leafs
	{
		TreeNode ownedNode = GetOwnedNode(agent.GetAgentNumber());
		List<Agent> assisters = ownedNode.GetAssisters();
		leafs.Remove(ownedNode); //remove the node from the leafs
		
		//remove the node from the tree
		int level = -1;
		foreach(List<TreeNode> list in tree)
		{
			level++;
			if(list.Contains(ownedNode))
			{
				list.Remove(ownedNode);
				break;
			}
		}

		if(ownedNode.GetParent() != null) //if node has a parent it might be possible for this agent to take that as its next owned node (more efficient, agents that get resource will also build house)
		{
			List<string> agents = ActionManager.Instance.AssistingAgentsToAction(agent.GetAgentType(), ownedNode.GetParent().GetActionName());
			assisters = ownedNode.GetParent().GetAssisters();
		
		
			if(HasChild(tree[level], ownedNode.GetParent()) == false ) 
			{
				
				int nrAgentToDoAction = agents.FindAll(item => item == agent.GetAgentType()).Count;
				int nrSameColorAssisters = 0;
				
				foreach(Agent assister in assisters)
				{
					if(assister.GetAgentType() == agent.GetAgentType())
					{
						nrSameColorAssisters++;
					}
				}
				
				leafs.Add(ownedNode.GetParent());
				
				
				if(nrAgentToDoAction - nrSameColorAssisters == 1)
				{
				
					ownedNode.GetParent().SetOwner(agent);
				
				} else {
				
					foreach(Agent assister in assisters)
					{
						if(assister.GetAgentType() == agent.GetAgentType())
						{
							ownedNode.GetParent().SetOwner(assister);
							assisters.Remove(assister);
							break;
						}
					}
				}
				
			} else if (HasChild(tree[level], ownedNode.GetParent()) == true)
			{
				if(agents.Contains(agent.GetAgentType()))
				{
					if(OkayToAssist(agent,  ownedNode.GetParent(), 1))
					{
						ownedNode.GetParent().GetAssisters().Add(agent);
					}
				}
			}
		}
	}
	
	public bool HasChild(List<TreeNode> level, TreeNode searchNode) //Checks if searchNode node is a parent to one or more of the nodes at level
	{
		foreach(TreeNode node in level)
		{
			if(node.GetParent() == searchNode)
			{
				return true;
			}
		}	
		return false;
	}
	
	public List<TreeNode> GetLeafs()
	{
		return leafs;
	}
	
	public List<List<TreeNode>> GetTree()
	{
		return tree;
	}
	
	public bool OkayToAssist(Agent agent, TreeNode ownedNode, int mode) //check if its okay of the agent to assist on a node, mode 1 = check based on agentType, mode 2 = check based on agentNumber
	{
		bool okToAssist;
		
		if(mode == 1)
		{
			okToAssist = true;
		} else {
		
			okToAssist = false;
		}
		
		List <Agent> assisters = ownedNode.GetAssisters();
				
			foreach(Agent assister in assisters)
			{
				if(mode == 1 && assister.GetAgentType() == agent.GetAgentType()) 
				{
					okToAssist = false;
				} else if(mode == 2 && assister.GetAgentNumber() == agent.GetAgentNumber()) 
				{
					okToAssist = true;
				}
			}
		
		return okToAssist;
				
	}
	
	public void RemoveAgentFromOwnedNode(Agent agent) //Remove the agent from its owned node in the tree
	{
		TreeNode ownedNode = GetOwnedNode(agent.GetAgentNumber());
		if(ownedNode.GetPosition() != new Vector3(30, 0.5f, 30))
		{
			if(ownedNode.GetOwner().GetAgentNumber() == agent.GetAgentNumber())
			{
				ownedNode.SetOwner(null);
			} else {
			
				ownedNode.GetAssisters().Remove(agent);
			}
		}
	}
	

}