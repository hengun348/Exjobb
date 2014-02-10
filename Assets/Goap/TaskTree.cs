using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskTree : MonoBehaviour{

	TreeNode root;
	List<TreeNode> leafs; //A list of the leafs of the tree 
	List<List<TreeNode>> tree;
	
	public TaskTree()
	{
		leafs = new List<TreeNode>();
		tree = new List<List<TreeNode>>();
	}
	
	//public void AddSubtree(AStarNode planStep, Vector3 position, int treeIndex) //adds a new plan to the tree as a subtree to the root node
	public void AddSubtree(List<TreeNode> plan)
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
			//if(planStep.GetParent() != null)
			//Debug.Log("lägger till " + planStep.GetActionName() + " som har parent: " + planStep.GetParent().GetActionName());
			
		}
		
		Debug.Log("-------- Lägger till löv----------");
		
		//add leafs
		foreach(TreeNode node in tree[tree.Count-1])
		{
			if(node.GetIndex() == plan[0].GetIndex())
			{
				Debug.Log("+1 ett löv");
				leafs.Add(node);
			}
		}

		for(int i = tree.Count-1; i > 0; i--)
		{
			foreach(TreeNode node in tree[i-1])
			{
				//Debug.Log ("node in tree-1: " + node.GetActionName());
				//Debug.Log(HasChild(tree[i], node));
				if(HasChild(tree[i], node) == false) //has no childs == is a leaf
				{
					Debug.Log("+1 ett löv");
					leafs.Add(node);
				}
			}
		}
		//PrintLeafs();
		
		PrintTree();
	}
	
	
	public void PrintLeafs()
	{
		
		string temp = "-----------LÖV: ";
		
		foreach(TreeNode node in leafs)
		{
			temp += node.GetActionName() + " "/*+ " owner = " + node.GetOwner().getAgentType() + " assister = "*/; 	
			
			/*foreach(Agent assister in node.GetAssisters())
			{
				temp += "" + assister.getAgentType();
			}*/
			
			
		}
		Debug.Log (temp);
		
		
		
	}
	
	public void PrintTree()
	{
		
		
		/*List<TreeNode> tempList = new List<TreeNode> ();
		tempList.Add (new TreeNode(new Vector3(),"level1", root, 1));
		tree.Insert(0, tempList);
		
		tempList = new List<TreeNode> ();
		tempList.Add (new TreeNode(new Vector3(),"level2", root, 1));
		tempList.Add (new TreeNode(new Vector3(),"level2", root, 1));
		tree.Insert(1, tempList);
		
		tempList = new List<TreeNode> ();
		tempList.Add (new TreeNode(new Vector3(),"level3", root, 1));
		tempList.Add (new TreeNode(new Vector3(),"level3", root, 1));
		tempList.Add (new TreeNode(new Vector3(),"level3", root, 1));
		tree.Insert(2, tempList);*/
		
		//PrintLevel(leafs);
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
			if(leaf.GetOwner() != null && leaf.GetOwner().getAgentNumber() == agent.getAgentNumber()) //if agent already owns a node
			{
				return leaf.GetActionName();
			} 
			else if(leaf.GetOwner() != null) { //if an agent already is assisting on a node
				
				
				
				if(OkayToAssist(agent, leaf, 2))
				{
					//Debug.Log("AGNETSTHATDOACVTION " + agents[0] + " " + agents[1] + " for action " + leaf.GetActionName());
					return "AssistingAction";
				}
			}
		}

		foreach(TreeNode leaf in leafs)
		{
			List<string> agents = ActionManager.Instance.AssistingAgentsToAction(agent.getAgentType(), leaf.GetActionName());
			
			if(agents.Count > 0 && leaf.GetOwnerNumber() == System.Guid.Empty )
			{
				leaf.SetOwner(agent);
				return leaf.GetActionName();
			}
			else if(agents.Count > 1 && leaf.GetOwnerNumber() != System.Guid.Empty && agent.getAgentType() != leaf.GetOwner().getAgentType())
			{
				
				if(OkayToAssist(agent, leaf, 1))
				{
					leaf.GetAssisters().Add(agent);
					//Debug.Log("AGNETSTHATDOACVTION " + agents[0] + " " + agents[1] + " for action " + leaf.GetActionName());
					return "AssistingAction";
				}
			}
		}
			

		return "IdleAction"; //empty string means no match, agent cant to any of the leaf-node-actions 
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
	public void RemoveNode(Agent agent)
	{
		
		TreeNode ownedNode = GetOwnedNode(agent.getAgentNumber());
		List<Agent> assisters = ownedNode.GetAssisters();
		leafs.Remove(ownedNode);
		
		
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
		
		//Debug.Log (agent.getAgentType());
		//Debug.Log(ownedNode.GetParent().GetActionName());
		if(ownedNode.GetParent() != null)
		{
			List<string> agents = ActionManager.Instance.AssistingAgentsToAction(agent.getAgentType(), ownedNode.GetParent().GetActionName());
			assisters = ownedNode.GetParent().GetAssisters();
		
		
			if(HasChild(tree[level], ownedNode.GetParent()) == false )
			{
				
				int nrAgentToDoAction = agents.FindAll(item => item == agent.getAgentType()).Count;
				int nrSameColorAssisters = 0;
				
				foreach(Agent assister in assisters)
				{
					if(assister.getAgentType() == agent.getAgentType())
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
						if(assister.getAgentType() == agent.getAgentType())
						{
							ownedNode.GetParent().SetOwner(assister);
							assisters.Remove(assister);
							break;
						}
					}
				}
				
				//PrintLeafs();
				
			} else if (HasChild(tree[level], ownedNode.GetParent()) == true)
			{
				if(agents.Contains(agent.getAgentType()))
				{
					if(OkayToAssist(agent,  ownedNode.GetParent(), 1))
					{
						ownedNode.GetParent().GetAssisters().Add(agent);
					}
				}
			}
		}
		
		Debug.Log ("!!!!!!!!!!!Remove node!!!!!!!!!!!!!");
		
		//PrintTree();
	}
	
	//Checks if this node is a parent to one or more of the leafs
	public bool HasChild(List<TreeNode> level, TreeNode searchNode)
	{
		foreach(TreeNode node in level)
		{
			//Debug.Log ("node tree: " + node.GetActionName() + "(" + node.GetParent().GetActionName() + ")");
			if(node.GetParent() == searchNode /*&& node.GetParent().GetIndex() == searchNode.GetIndex()*/)
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
	
	public bool OkayToAssist(Agent agent, TreeNode ownedNode, int mode)
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
				if(mode == 1 && assister.getAgentType() == agent.getAgentType()) 
				{
					okToAssist = false;
				} else if(mode == 2 && assister.getAgentNumber() == agent.getAgentNumber()) 
				{
					okToAssist = true;
				}
			}
		
		return okToAssist;
				
	}
	
	public void RemoveAgentFromOwnedNode(Agent agent)
	{
		TreeNode ownedNode = GetOwnedNode(agent.getAgentNumber());
		if(ownedNode.GetPosition() != new Vector3(30, 0.5f, 30))
		{
			if(ownedNode.GetOwner().getAgentNumber() == agent.getAgentNumber())
			{
				ownedNode.SetOwner(null);
			} else {
			
				ownedNode.GetAssisters().Remove(agent);
			}
		}
	}
	
}