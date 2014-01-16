using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SupremeCommander: MonoBehaviour
{
	public float globalEnergy;
	List<GameObject> agents;
	
	void Start()
	{
		agents = new List<GameObject>();
		GetAllAgents();	
	}
	
	void Update()
	{
		CalculateScore();
	}
	
	private void GetAllAgents()
	{
		foreach(WorkingMemoryValue val in BlackBoard.Instance.GetFact("Agents"))
		{
			agents.Add((GameObject)val.GetFactValue());
		}
	}
	
	private float CalculateScore()
	{
		globalEnergy = 0;
		foreach(GameObject agent in agents)
		{
			globalEnergy += ((Agent)agent.GetComponent("Agent")).energy;
		}
		globalEnergy = globalEnergy/agents.Count;
		
		return globalEnergy;
	}	
}
