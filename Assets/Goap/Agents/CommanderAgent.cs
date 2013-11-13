using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommanderAgent: MonoBehaviour
{
	
	public BlackBoard blackBoard;
	private WorldState currentWorldState;
	private WorldState goalWorldState;
	private GameObject[] agents;
	
	public Planner planner;
	
	List<AStarNode> plan;
	
	int planIndex = 0;
	
	public bool actionIsDone;
	public string currentAction;
	
	public Dictionary<string, int> buildings = new Dictionary<string, int>();
	
	private string agentType { get; set; }
	
	void Start(){
		
		blackBoard = new BlackBoard();
		buildings["house"] = 0;
		currentWorldState = new WorldState();
		
		currentWorldState.setProperty("stoneIsAvailable", new WorldStateValue(true));
		currentWorldState.setProperty("woodIsAvailable", new WorldStateValue(true));
		currentWorldState.setProperty("houseIsBuilt", new WorldStateValue(false));
		currentWorldState.setProperty("pyramidIsBuilt", new WorldStateValue(false));
		
		goalWorldState = new WorldState();
		//if(buildings["house"] > 14)
		//{
			goalWorldState.setProperty("pyramidIsBuilt", new WorldStateValue(true));
		//}else
		//{
			//Debug.Log("aaa");
			//goalWorldState.setProperty("houseIsBuilt", true);	
		//}
		
		
		//Debug.Log ("index: " + planIndex);
		this.agentType = "commanderAgent";
		
		actionIsDone = true;
		

		planner = new Planner();
		setGoalWorldState(goalWorldState);
		ActionManager.Instance.currentAgent = agentType;
		plan = planner.runAStar(currentWorldState);
		
		
		Debug.Log("COMMANDERNS PLAN!!!!!!!!: " + plan.Count);
		foreach(AStarNode node in plan)
		{
			Debug.Log("-----" + node.name);
		}
		//buildings["house"] ++;
		//currentWorldState.setProperty("houseIsBuilt", false);
		agents = GameObject.FindGameObjectsWithTag("SpecialAgent");
		
		
		
		
	}
	
	void Update(){
		
		
		
		
		if(plan.Count > 0 && planIndex < plan.Count){
		
			foreach(GameObject agent in agents){
			
				if(((Agent)(agent.GetComponent("Agent"))).isAvailable){
				
					((Agent)(agent.GetComponent("Agent"))).setGoalWorldState(ActionManager.Instance.getAction(plan[planIndex].name).postConditions);
					planIndex ++;
					break;
				}
				
			}
		}
		
		/*if(actionIsDone && (planIndex < plan.Count)){
			Debug.Log("planindex: " + planIndex + " och plan.count-1: " + plan.Count);	
			currentAction = plan[planIndex].name;
			planIndex ++;
			actionIsDone = false;
		}else if(actionIsDone && (planIndex == plan.Count))
		{
			currentAction = "";
			//plan = planner.runAStar(this.agentType);
		}/*else{
			//do nothing
		}*/
		

	}
	
	void setGoalWorldState(WorldState goalWorldState){
		
		planner.goalWorldState = goalWorldState;
	}
	
}