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
				//Debug.Log ("node in tree-1: " + node.GetActionName());
				//Debug.Log(HasChild(tree[i], node));
				if(HasChild(tree[i], node) == false) //has no childs == is a leaf
				{
					leafs.Add(node);
				}
			}
		}
		PrintLeafs();
	}
	
	
	public void PrintLeafs()
	{
		string leafString = "--Printing leafs!-- ";
		foreach(TreeNode node in leafs)
		{
			leafString += node.GetActionName() + " "; 	
		}
		Debug.Log(leafString);
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
			if(leaf.GetOwner() != null && leaf.GetOwner().getAgentNumber() == agent.getAgentNumber())
			{
				return leaf.GetActionName();
			} 
		}/*else if(leaf.GetOwner() != null) {
				
				bool okToAssist = false;
				List<Agent> assisters = leaf.GetAssisters();
				foreach(Agent assister in assisters)
				{
					if(assister.getAgentNumber() == agent.getAgentNumber()) 
					{
						okToAssist = true;
					}
				}
				
				if(okToAssist)
				{
					leaf.GetAssisters().Add(agent);
					//Debug.Log("AGNETSTHATDOACVTION " + agents[0] + " " + agents[1] + " for action " + leaf.GetActionName());
					return "AssistingAction";
				}
			}
		}*/

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
	public void RemoveNode(Agent agent)
	{
		
		TreeNode ownedNode = GetOwnedNode(agent.getAgentNumber());
		List<Agent> assisters = ownedNode.GetAssisters();
		
		//TODO: Kontrollera så att detta verkligen itne behövs
		foreach(Agent assister in assisters)
		{
			assister.GetPlan()[0] = "Idle";
			Destroy(assister.transform.FindChild("Subsystem").gameObject);
		}
		leafs.Remove(ownedNode);
		
		if(ownedNode.GetActionName() != "")
		{
			Debug.Log("____________ node removed: " + ownedNode.GetActionName() + "(" + ownedNode.GetIndex() + ")");
		}
		
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

		
		if(ownedNode.GetParent() != null && HasChild(tree[level], ownedNode.GetParent()) == false )
		{
			
			leafs.Add(ownedNode.GetParent());
			ownedNode.GetParent().SetOwner(agent); //TODO: fixa så att även assisters sätts som ägare direkt
			

			Debug.Log ("*********************Agent som har fått parent: " + agent.getAgentNumber());
			
			Debug.Log("____________ node added: " + ownedNode.GetParent().GetActionName() + "(" + ownedNode.GetParent().GetIndex() + ")");
			PrintTree();
				
			string currentLevel = "";
			foreach(TreeNode node in leafs)
			{
				currentLevel += node.GetActionName() +  " ";
			}
			//Debug.Log("leafs -----------------------------> " + currentLevel);
		}
		
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
}