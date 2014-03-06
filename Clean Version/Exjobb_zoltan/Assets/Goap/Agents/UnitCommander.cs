using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Commanders/UnitCommander")]

public class UnitCommander: MonoBehaviour
{
	GameObject agentGroup; //the group in a clan which all agents are placed under
	string clan;
	
	void Start()
	{	
		agentGroup = new GameObject();	
		agentGroup.name = "Agents";
		agentGroup.transform.parent = gameObject.transform.parent.parent;
		agentGroup.transform.position = gameObject.transform.parent.parent.position + new Vector3(0, 0.5f, 0);
		
		AddAgent("Blue");
		AddAgent("Blue");
		
		AddAgent("Red");
		AddAgent("Red");
		
		AddAgent("Yellow");
		AddAgent("Yellow");
		//AddAgent("Red");
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;
	}
	
	void Update()
	{
		UpdateGrid();
	}
	
	public void AddAgent(string color) //add a new agent to the world
	{
		GameObject agentObject = new GameObject();
		agentObject.name = "AgentObject";
		agentObject.transform.parent = agentGroup.transform;
		Agent prefab = (Agent)Instantiate(Resources.Load(("Prefabs/Agent"),typeof(Agent))); //load its prefab object
		prefab.transform.position = new Vector3(Random.Range(-10, 10), 1, 0);
		prefab.transform.parent = agentObject.transform;
		prefab.renderer.material.color = BlackBoard.Instance.GetColorForObject(color);
		prefab.name = color;
		prefab.SetClan(clan);
		BlackBoard.Instance.UpdateScore(clan, prefab.name + " " + prefab.tag); //increase the score for the clan the agent is added to
		
		BlackBoard.Instance.SetFact(clan, "Agents", new WorkingMemoryValue(prefab)); //add the agent to the clan facts of available agents
		BlackBoard.Instance.AddColorToClan(clan, color);
				
	}
	
	public List<Agent> GetAgents() //return all the agents of a clan
	{
		List<Agent> agents = new List<Agent>();
		foreach(WorkingMemoryValue wm in BlackBoard.Instance.GetFact(clan, "Agents"))
		{
			agents.Add((Agent)wm.GetFactValue());
		}
		return agents;
	}
	
	void UpdateGrid() //Update the agents positions in the gridgraph
	{
		
		foreach(Agent agent in GetAgents())
		{	
			AstarPath.active.UpdateGraphs (agent.collider.bounds);
		}
	}
}
