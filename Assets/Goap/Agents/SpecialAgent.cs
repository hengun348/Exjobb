using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecialAgent: MonoBehaviour
{
	
	public WorkingMemory wMemory;
	private WorldState currentWorldState;
	
	public Planner planner;
	List<AStarNode> plan;
	CharacterController controller;
	int planIndex = 0;
	public bool actionIsDone;
	public string currentAction;
	public bool isAvailable;
	
	private string agentType { get; set; }
	
	void Start(){
		
		isAvailable = true;
		
		currentWorldState = new WorldState();
		currentWorldState.setProperty("enemyVisible", new WorldStateValue(false));
		currentWorldState.setProperty("armedWithGun", new WorldStateValue(false));
		currentWorldState.setProperty("weaponLoaded", new WorldStateValue(false));
		currentWorldState.setProperty("enemyLinedUp", new WorldStateValue(false));
		currentWorldState.setProperty("enemyAlive", new WorldStateValue(true));
		currentWorldState.setProperty("armedWithBomb", new WorldStateValue(true));
		currentWorldState.setProperty("nearEnemy", new WorldStateValue(false));
		currentWorldState.setProperty("agentAlive", new WorldStateValue(true));
		//TODO fixa så att man itne behöver lägga in allt för hand
		currentWorldState.setProperty("stoneIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("woodIsAvailable", new WorldStateValue(false));
		currentWorldState.setProperty("houseIsBuilt", new WorldStateValue(false));
		currentWorldState.setProperty("pyramidIsBuilt", new WorldStateValue(false));
		
		WorldState goalWorldState = new WorldState();
		goalWorldState.setProperty("enemyAlive", new WorldStateValue(false));
		//goalWorldState.setProperty("agentAlive", true); //------ HADE GLÖMT DENNA HAHA! BUMMER!
		
		
		
		
		
		//Debug.Log ("index: " + planIndex);
		this.agentType = "specialAgent";
		//controller = GetComponent<CharacterController>();
		
		actionIsDone = true;

		planner = new Planner();
		plan = new List<AStarNode>();
		//setGoalWorldState(goalWorldState);
		
		
		//controller.gameObject.AddComponent("BlackBoard");
		//Debug.Log("finns det en blackboard? " + blackBoard);
		//((BlackBoard)controller.gameObject.GetComponent("BlackBoard")).setCurrentAction(plan[0].name);
		
		GameObject sub = new GameObject("Subsystems");
		sub.transform.parent = transform;
		
		sub.AddComponent("Subsystems");
		
		GameObject sens = new GameObject("Sensors");
		sens.transform.parent = transform;
		
		sens.AddComponent("Sensors");
	
		
		//foreach(AStarNode node in plan)
		//{
			//TODO: dubbelkolla att returnerande action faktiskt finns
		//}	
		
		
		//WORKING MEMORY
		wMemory = new WorkingMemory();
		
		//Faktan ska sättas i sensorerna 
		wMemory.setFact("woodPosition", new WorkingMemoryValue(new Vector3(3.0f,0.0f,10.0f)));
		wMemory.setFact("stonePosition", new WorkingMemoryValue(new Vector3(-5.0f,0.0f,0.0f)));
		
		wMemory.setFact("buildPyramidPosition", new WorkingMemoryValue(new Vector3(10.0f,0.0f,0.0f)));
		
	}
	
	void Update(){
		
		//Debug.Log("CURRENT ACTION " + currentAction);
		wMemory.setFact("buildHousePosition", new WorkingMemoryValue(new Vector3(Random.Range(0.0f, 10.0f), 0.0f, Random.Range(0.0f, 10.0f))));
		
		if(actionIsDone && (planIndex < plan.Count)){
			
			//Debug.Log("planindex: " + planIndex + " och plan.count-1: " + plan.Count);
			//Debug.Log(plan[planIndex].name);
			currentAction = plan[planIndex].name;
			planIndex ++;
			actionIsDone = false;
			setCurrentWorldState(ActionManager.Instance.getAction(currentAction).postConditions); //Ska vi ändra currentAction här!? 
		}else if(actionIsDone && (planIndex == plan.Count))
		{
			//Debug.Log("NÄR KOMEMR VI IN HÄR DÅ???");
			currentAction = ""; 
			planIndex = 0;
			isAvailable = true;
			plan.Clear();
			//Debug.Log("VI ÄR KLARA OCH PLANEN STÅR PÅ " + plan[planIndex].name);
			//Debug.Log("*CURRENTACTION = 0!!!!!!!!!!!!!!!!!!!!!!!!!*");
			//plan = planner.runAStar(this.agentType);
		}
		

	}
	
	void setCurrentWorldState(WorldState currentWorldState){
		
	 foreach(KeyValuePair<string,WorldStateValue> pair in currentWorldState.getProperties())
		{
		
			this.currentWorldState.getProperties()[pair.Key] = pair.Value;
			
		}
	
	}
	
	void OnTriggerEnter(Collider other) { 
		if (other.gameObject.tag == "Stonesource") {
			//Debug.Log("Yay!"); 
			wMemory.setFact("stonePosition", new WorkingMemoryValue(new Vector3(-12.0f,0.0f,-23.0f))); 
		} 
	}
	
	public void setGoalWorldState(WorldState goalWorldState){
		//Debug.Log("NU SKA SPECIALAGENTEN GÖRA NÅNTING!!!  ");
		planner.goalWorldState = goalWorldState;
		ActionManager.Instance.currentAgent = agentType;
		isAvailable = false;
		plan = planner.runAStar(currentWorldState);
		
		Debug.Log("AGENTENS PLAN!!!!!!!!: " + plan.Count);
		foreach(AStarNode node in plan)
		{
			Debug.Log("-----" + node.name);
		}
	}
}