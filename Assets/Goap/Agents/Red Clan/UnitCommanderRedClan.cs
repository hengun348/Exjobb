using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Commanders/UnitCommanderRedClan")]

public class UnitCommanderRedClan: MonoBehaviour
{
	GameObject agentGroup;
	string clan;
	bool okToAdd;
	//List<Agent> agents;
	
	void Awake()
	{
		//agents = new List<Agent>();
		okToAdd = false;
	}
	
	void Start()
	{	
		agentGroup = new GameObject();	
		agentGroup.name = "Agents";
		//agentGroup.transform.parent = gameObject.transform.parent.transform.parent;
		//agentGroup.transform.position = new Vector3(0, 1, 0);
		agentGroup.transform.parent = gameObject.transform.parent.parent;
		agentGroup.transform.position = gameObject.transform.parent.parent.position + new Vector3(0, 0.5f, 0);
		
		
		string color = "";
		
		for (int k = 1; k < clan.Length; k++) 
			{ 
				if (char.IsUpper(clan[k])) 
				{ 	
					color = clan.Substring(0, k-1); 
					break; 
				} 
			}
		
		for(int i = 0; i < 2; i++)
		{
			//AddAgent(color, agentGroup.transform.position);
			StartCoroutine(AddAgent(color));
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
	
	public IEnumerator AddAgent(string color)
	{
		Debug.Log("Nu ska det skapas en ny gubbe");
		while(okToAdd == false)
		{
			if(GameObject.Find(clan).transform.FindChild("Buildings") == null)
			{
				Debug.Log("Nu finns inte buildings");
				yield return null;
			}
			else
			{
				if(((Temple)GameObject.Find(clan).transform.FindChild("Buildings").FindChild("Temple(Clone)").GetComponent("Temple")).IsSacrificing())
				{
					//Debug.Log("men det offras tydligen just nu");
				}
				else{
					//Debug.Log(GameObject.Find(clan).transform.name);
					((Temple)GameObject.Find(clan).transform.FindChild("Buildings").FindChild("Temple(Clone)").GetComponent("Temple")).Sacrifice();
					yield return new WaitForSeconds(2);
					
			
					GameObject agentObject = new GameObject();
					agentObject.name = "AgentObject";
					agentObject.transform.parent = agentGroup.transform;
					Agent prefab = (Agent)Instantiate(Resources.Load(("Prefabs/Agent"),typeof(Agent)));
					prefab.transform.position = agentGroup.transform.position;
					prefab.transform.parent = agentObject.transform;
					prefab.renderer.material.color = BlackBoard.Instance.GetColorForObject(color);
					prefab.name = color;
					prefab.SetClan(clan);
					BlackBoard.Instance.UpdateScore(clan, prefab.name + " " + prefab.tag);
					
					BlackBoard.Instance.SetFact(clan, "Agents", new WorkingMemoryValue(prefab));
					BlackBoard.Instance.AddColorToClan(clan, color);
					okToAdd = true;
				}
			}
			yield return null;
		}
		okToAdd = false;
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
