using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class Agent2: MonoBehaviour
{
	
	private WorkingMemory wMemory;
	//private WorldState currentWorldState;
	private BlackBoard blackBoard; 
	
	//private Planner planner;
	//List<AStarNode> plan;
	//int planIndex = 0;

	public string currentAction;
	private GameObject gui, target;
	private System.Guid agentNumber;
	
	void Start(){
				
		SetColor();
		agentNumber = System.Guid.NewGuid();
					
		AddSensors();
		AddGUI();
		AddPathfinding();
		
		//planner = new Planner();
		//plan = new List<AStarNode>();	

		//WORKING MEMORY
		wMemory = new WorkingMemory();
		
		blackBoard = BlackBoard.Instance;
	}
	
	void Update(){
		if(transform.childCount == 3 || currentAction == "Idle"){
			blackBoard.getTaskTree().removeNode(agentNumber);
			StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(0.0f, 1.0f));//fades the gui out

			currentAction = blackBoard.getActionForAgent(this); //returns "Idle" if no actions are possible	
			if(currentAction != "Idle")
			{
				StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(1.0f, 1.0f)); // fades the gui in
				AddSubsystem();
			}
		}
	}
	
	/*void setCurrentWorldState(WorldState currentWorldState){
	 	foreach(KeyValuePair<string,WorldStateValue> pair in currentWorldState.getProperties())
		{
			this.currentWorldState.getProperties()[pair.Key] = pair.Value;	
		}
	}
	
	
	
	public void setGoalWorldState(WorldState goalWorldState){
		//Debug.Log("NU SKA SPECIALAGENTEN GÖRA NÅNTING!!!  ");
		
		planner.goalWorldState = goalWorldState;
	//	ActionManager.Instance.currentAgent = agentType;
		plan = planner.runAStar(currentWorldState);
				
		Debug.Log("AGENTENS PLAN!!!!!!!!: " + plan.Count);
		foreach(AStarNode node in plan)
		{
			Debug.Log("-----" + node.name);
		}
		
	}*/
		
	private void AddSubsystem() 
	{ 
		string subsystemToLoad;
		GameObject sub = new GameObject("Subsystem"); 
		sub.transform.position = transform.position; 
		sub.transform.parent = transform; 
		if(currentAction == "Idle")
		{
			subsystemToLoad = "IdleSubsystem"; 
		}else{
			subsystemToLoad = currentAction.Substring(0, currentAction.Length - 6) + "Subsystem"; 
		}
		sub.AddComponent(subsystemToLoad); 
	}
		
	private void AddSensors()
	{
		GameObject sens = new GameObject("Sensors");
		sens.transform.position = transform.position;
		sens.transform.parent = transform;
		sens.AddComponent("Sensors");
	}
		
	private void AddGUI()
	{
		//TODO: only show when agent is marked instead -> leaving only one guiPlan to be shown at any time.
		gui = GameObject.CreatePrimitive(PrimitiveType.Quad);
		gui.name = "PlanGUI";
		gui.collider.isTrigger = true;
		gui.transform.position = transform.position + new Vector3(0, 1.0f, 0);
		gui.transform.parent = transform;
		gui.AddComponent("PlanGUI");
	}
	
	private void AddPathfinding()
	{
		target = new GameObject("Target");
		target.transform.parent = gameObject.transform;
		
		gameObject.AddComponent("Seeker");
		gameObject.AddComponent("AIPath");
		((AIPath)gameObject.GetComponent("AIPath")).canMove = false;
		((AIPath)gameObject.GetComponent("AIPath")).canSearch = false;
		((AIPath)gameObject.GetComponent("AIPath")).target = target.transform;
		((AIPath)gameObject.GetComponent("AIPath")).speed = 10;
	}
	
	private void ActionTookTooLong()
    {		
			Debug.Log("!!!!!!!PLANEN HAR TAGIT FÖR LÅNG TID!!!!!!");
        	//currentAction = ""; 
			//planIndex = 0;
			//plan.Clear();
			//status = "available"
			
		
			
			//Tell commander plan failed
			
    }
	
	private void SetColor(){
	
		Dictionary<string, Color> colors = new Dictionary<string, Color>(); 
		colors["Gray"] = Color.gray;
		colors["Blue"] = Color.blue; // and so on 
		colors["Red"] = Color.red; 
		string tag = gameObject.tag.ToString(); 
		string color = ""; 
		
		for (int i = 1; i < tag.Length; i++) 
		{ 
			if (char.IsUpper(tag[i])) 
			{ 	color = tag.Substring(0, i); 
				break; 
			} 
		} 
		
		gameObject.renderer.material.color = colors[color];
		
	}
	
	public WorkingMemory getWMemory(){
		return wMemory;
	}
	
	public GameObject getTarget()
	{
		return target;
	}
	
	public string getAgentType()
	{
		return tag;
	}
	
	public System.Guid getAgentNumber()
	{
		return agentNumber;
	}		
}