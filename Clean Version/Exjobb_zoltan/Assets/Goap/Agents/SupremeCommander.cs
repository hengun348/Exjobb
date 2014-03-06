using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Commanders/SupremeCommander")]

public class SupremeCommander: MonoBehaviour
{
	string clan;
	UnitCommander unitCommander;
	BuildingCommander buildingCommander;
	
	void Start()
	{
		//create UnitCommander
		unitCommander = (UnitCommander)Instantiate(Resources.Load(("Prefabs/UnitCommander"), typeof(UnitCommander)));
		unitCommander.transform.position = transform.parent.position;
		unitCommander.transform.parent = transform.parent;
		unitCommander.SetClan(clan);
		
		//create BuildingCommander
		buildingCommander = (BuildingCommander)Instantiate(Resources.Load(("Prefabs/BuildingCommander"), typeof(BuildingCommander)));
		buildingCommander.transform.position = transform.parent.position;
		buildingCommander.transform.parent = transform.parent;
		buildingCommander.SetClan(clan);
	}
	
	void Update()
	{	
		//Create new buildings
		if(BlackBoard.Instance.GetTaskTree(clan).GetLeafs().Count == 0)
		{
			AddNewGoals();
		}
	}
	
	public void AddNewGoals()
	{
		
		buildingCommander.SetGoal(new WorldState("blackTowerIsBuilt", new WorldStateValue(true)));
		//buildingCommander.SetGoal(new WorldState("blueHouseIsBuilt", new WorldStateValue(true))); 
		//buildingCommander.SetGoal(new WorldState("redHouseIsBuilt", new WorldStateValue(true))); 
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;
	}
	
}
