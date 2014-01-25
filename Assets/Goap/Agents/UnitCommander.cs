using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCommander: MonoBehaviour
{
	GameObject agentGroup;
	string clan;
	//List<Agent> agents;
	
	void Awake()
	{
		//agents = new List<Agent>();
	}
	
	void Start()
	{	
		agentGroup = new GameObject();	
		agentGroup.name = "Agents";
		agentGroup.transform.parent = gameObject.transform.parent.transform.parent;
		agentGroup.transform.position = new Vector3(0, 1, 0);
		
		for(int i = 0; i < 2; i++)
		{
			AddAgent("Red");
			AddAgent("Blue");
			AddAgent("Yellow");
		}
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;
	}
	
	void Update()
	{
		UpdateGrid();
	}
	
	void AddAgent(string color)
	{
		GameObject agentObject = new GameObject();
		agentObject.name = "AgentObject";
		agentObject.transform.parent = agentGroup.transform;
		Agent prefab = (Agent)Instantiate(Resources.Load(("Prefabs/Agent"),typeof(Agent)));
		prefab.transform.position = new Vector3(Random.Range(gameObject.transform.position.x -10, gameObject.transform.position.x + 10), 1.0f, Random.Range(gameObject.transform.position.z - 10, gameObject.transform.position.z + 10));
		prefab.transform.parent = agentObject.transform;
		prefab.renderer.material.color = BlackBoard.Instance.GetColorForObject(color);
		prefab.tag = "Citizen";
		prefab.name = color;
		prefab.SetClan(clan);
		BlackBoard.Instance.UpdateScore(clan, prefab.name + " " + prefab.tag);
		
		BlackBoard.Instance.SetFact(clan, "Agents", new WorkingMemoryValue(prefab));
	}
	
	public List<Agent> GetAgents()
	{
		//Varför använda blackboarden när man bara kan ha en lista sparad här?
		List<Agent> agents = new List<Agent>();
		foreach(WorkingMemoryValue wm in BlackBoard.Instance.GetFact(clan, "Agents"))
		{
			agents.Add((Agent)wm.GetFactValue());
		}
		return agents;
	}
	
	void UpdateGrid()
	{
		//Uppdatera agenternas positioner i gridgraphen
		foreach(Agent agent in GetAgents())
		{	
			AstarPath.active.UpdateGraphs (agent.collider.bounds);
		}
	}
}
