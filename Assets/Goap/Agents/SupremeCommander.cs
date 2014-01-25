using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SupremeCommander: MonoBehaviour
{
	public float globalEnergy;
	string clan;
	UnitCommander unitCommander;
	BuildingCommander buildingCommander;
	
	void Awake()
	{
	
	}
	
	void Start()
	{
		//create UnitCommander
		unitCommander = (UnitCommander)Instantiate(Resources.Load(("Prefabs/UnitCommander"),typeof(UnitCommander)));
		unitCommander.transform.position = transform.position;
		unitCommander.transform.parent = transform.parent.transform;
		unitCommander.SetClan(clan);
		
		//create BuildingCommander
		buildingCommander = (BuildingCommander)Instantiate(Resources.Load(("Prefabs/BuildingCommander"),typeof(BuildingCommander)));
		buildingCommander.transform.position = transform.position;
		buildingCommander.transform.parent = transform.parent.transform;
		buildingCommander.SetClan(clan);
	}
	
	void Update()
	{
		CalculateScore();
		
		if(BlackBoard.Instance.GetTaskTree(clan).GetLeafs().Count == 0)
		{
			AddNewGoals();
		}
	}
	
	private float CalculateScore()
	{
		globalEnergy = 0;
		foreach(Agent agent in unitCommander.GetAgents())
		{
			globalEnergy += agent.energy;
		}
		globalEnergy = globalEnergy/unitCommander.GetAgents().Count;
		
		return globalEnergy;
	}	
	
	private void AddNewGoals()
	{
		Debug.Log ("Nu borde det slumpas mer tasks");
		//loopa igenom actions, välj ut en och posta dess postcondition
		
		buildingCommander.SetGoal(new WorldState("blackTowerIsBuilt", new WorldStateValue(true)));
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;
	}
}
