using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Commanders/WorldCommander")]

public class WorldCommander: MonoBehaviour
{
	void Start()
	{
		//Create resources
		GameObject resources = new GameObject();
		resources.name = "Resources";
		resources.layer = 9;
		
		GameObject prefab = (GameObject)Instantiate(Resources.Load(("Prefabs/Resource"),typeof(GameObject)));
		
		prefab.transform.position = new Vector3(0, 0, 15);
		
		
		//Create Red resources
		prefab.transform.parent = resources.transform;
		foreach(Transform child in prefab.transform)
		{
			child.renderer.material.color = Color.red;
		}
		
		prefab.tag = "RedSource";
		
		
		//Create Blue resources
		prefab = (GameObject)Instantiate(Resources.Load(("Prefabs/Resource"),typeof(GameObject)));

		prefab.transform.position = new Vector3(0, 0, -15);
		
		prefab.transform.parent = resources.transform;
		foreach(Transform child in prefab.transform)
		{
			child.renderer.material.color = Color.blue;
		}
		
		prefab.tag = "BlueSource";
		
		
		//Create Yellow resources
		prefab = (GameObject)Instantiate(Resources.Load(("Prefabs/Resource"),typeof(GameObject)));

		prefab.transform.position = new Vector3(15, 0, 0);
		
		prefab.transform.parent = resources.transform;
		foreach(Transform child in prefab.transform)
		{
			child.renderer.material.color = Color.yellow;
		}
		
		prefab.tag = "YellowSource";

		//update the gridgraph (pathfinding) with the positions of the resources
		foreach(Transform child in resources.transform)
		{
			AstarPath.active.UpdateGraphs(child.collider.bounds);
		}
		
		//create a new clan
		GameObject clan = new GameObject();
	
		clan.name = BlackBoard.Instance.GetClan(); //get an available name for the clan (3 available)
		clan.transform.position = new Vector3(-20 + Random.Range(-5, 5), 0, -20 + Random.Range(-5, 5));
		
		//Create a group for the commanders and place it in the clan
		GameObject commanderGroup = new GameObject();
		commanderGroup.name = "Commanders";
		commanderGroup.transform.parent = clan.transform;
		commanderGroup.transform.position = clan.transform.position;
		
		//Create a supremecommander for the clan
		SupremeCommander prefab2 = (SupremeCommander)Instantiate(Resources.Load(("Prefabs/SupremeCommander"), typeof(SupremeCommander)));
		
		prefab2.transform.position = commanderGroup.transform.position;
		prefab2.transform.parent = commanderGroup.transform;
		prefab2.SetClan(clan.name);
		
	}
}
