using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class Agent: MonoBehaviour
{
	
	private WorkingMemory wMemory;
	//private WorldState currentWorldState;
	private BlackBoard blackBoard; 
	
	private Planner planner;
	public List<string> plan;

	//public string currentAction;
	private GameObject gui, target;
	private System.Guid agentNumber;
	private List<string> subsystemFacts; 
	
	public int totalSkillpoints;
	private Dictionary<string, float> skillArray;

	private int tick;
	public int energy;
	
	public float Buildskill, Collectskill;
	
	public Color agentColor;
	
	
	void Start(){
		Buildskill = 0.0F;
		Collectskill = 0.0F;
		
		subsystemFacts = new List<string>();
		
		SetColor();
		agentNumber = System.Guid.NewGuid();
		
		totalSkillpoints = 100;
		float numberOfSkills = 2.0f;
		skillArray = new Dictionary<string, float>();
		skillArray.Add("buildSkillpoints", totalSkillpoints/numberOfSkills);
		skillArray.Add("collectSkillpoints", totalSkillpoints/numberOfSkills);
					
		AddSensors();
		AddGUI();
		AddPathfinding();
		
		planner = new Planner();
		plan = new List<string>();
		plan.Add("Idle");
		
		tick = 0;
		energy = 100;
		
		
		//WORKING MEMORY
		wMemory = new WorkingMemory();
		
		blackBoard = BlackBoard.Instance;
	}
	
	void Update(){
		
		//Set speed of agent depending on its energy
		((AIPath)gameObject.GetComponent("AIPath")).speed = 0.1f * energy;
		
		if((transform.childCount == 3 && plan.Count == 1) || plan[0] == "Idle"){
			blackBoard.GetTaskTree().RemoveNode(agentNumber);
			StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(0.0f, 1.0f));//fades the gui out

			plan[0] = blackBoard.GetActionForAgent(this); //returns "Idle" if no actions are possible	
			
			if(plan[0] != "Idle")
			{
				StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(1.0f, 1.0f)); // fades the gui in
				AddSubsystem();
			}
			
		}else if(transform.childCount == 3 && plan.Count > 1){
			//ta bort som ägare till noden? och gör resten av planen
			StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(0.0f, 1.0f));//fades the gui out
			plan.RemoveAt(0);
			StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(1.0f, 1.0f)); // fades the gui in
			AddSubsystem();
			
		}
		
		Collectskill = skillArray["collectSkillpoints"];
		Buildskill = skillArray["buildSkillpoints"];
		
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
		string subsystemToLoad = "";
		GameObject sub = new GameObject("Subsystem"); 
		sub.transform.position = transform.position; 
		sub.transform.parent = transform; 
		if(plan[0] == "Idle")
		{
			subsystemToLoad = "IdleSubsystem"; 
		}else{
			//subsystemToLoad = plan[0].Substring(0, plan[0].Length - 6) + "Subsystem"; 
			
			
			int tempPos = 0;
			
			for (int i = 1; i < plan[0].Length; i++) 
			{ 
				if (char.IsUpper(plan[0][i])) 
				{ 	
					subsystemFacts.Add(plan[0].Substring(tempPos, i-tempPos)); 
					tempPos = i;
					//break; 
				} 
			}
			subsystemToLoad = subsystemFacts[0];
			subsystemToLoad += "Subsystem";
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
	
		string tag = gameObject.tag.ToString(); 
		agentColor = BlackBoard.Instance.GetColorForObject(tag);
		gameObject.renderer.material.color =  agentColor;
		
	}
	
	public WorkingMemory GetWMemory(){
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
	
	public List<string> GetPlan()
	{
		return plan;
	}
		
	public void CreateNewPlan(WorldState currentWorldState, WorldState goalWorldState)
	{
		((AIPath)(gameObject.GetComponent("AIPath"))).canMove = false;
		((AIPath)(gameObject.GetComponent("AIPath"))).canSearch = false;
		StopCoroutine(plan[0].Substring(0, plan[0].Length - 6));
		Destroy(transform.FindChild("Subsystem").gameObject);
		
		planner.SetGoalWorldState(goalWorldState);
		List<string> tempPlan = new List<string>();
		foreach(string action in plan)
		{
			tempPlan.Add(action);
		}
		
		List<string> newPlan = new List<string>();
		foreach (AStarNode node in planner.RunAStar(currentWorldState))
		{
			newPlan.Add(node.getName());
		}
		
		plan.Clear();
		plan.Add ("Crap");
		plan.InsertRange(1, newPlan);
		plan.InsertRange(plan.Count, tempPlan);
	}
	
	public void SetSkillpoints(string inputSkill)
	{
		List<string> keys = new List<string>(skillArray.Keys);
		
		if(skillArray[inputSkill + "Skillpoints"] < 81)
		{
			
			foreach(string key in keys)
			{
			    if(key.Substring(0, key.Length - 11) == inputSkill)
				{
					skillArray[key] += 10;
					
					if(inputSkill == "build")
					{
						if(agentColor.r > 0.3)
						{
							agentColor.r = agentColor.r - 0.2f;
						}
						if(agentColor.g > 0.3)
						{
							agentColor.g = agentColor.g - 0.2f;
						}
						if(agentColor.b > 0.3)
						{
							agentColor.b = agentColor.b - 0.2f;
						}
					}
					else{
						if(agentColor.r < 0.7)
						{
							agentColor.r = agentColor.r + 0.2f;
						}
						if(agentColor.g < 0.7)
						{
							agentColor.g = agentColor.g + 0.2f;
						}
						if(agentColor.b < 0.7)
						{
							agentColor.b = agentColor.b + 0.2f;
						}
					}
					gameObject.renderer.material.color = agentColor;
					
				}else
				{	
					skillArray[key] -= (10/(skillArray.Count-1));
			
				}
			}
			
		}

		Debug.Log("nu blev jag bättre på: " + inputSkill);
	}
	
	void FixedUpdate()
	{
		tick ++;
		
		if(tick == 50)
		{
			/*if(Vector3.Distance(transform.position, new Vector3(30, 0, 30)) <= 2.0f) //Regenerate energy if at a specific position
			{
				if(energy < 96)
				{
					energy += 5;
				}
			}else{
				if(energy > 0)
				{
					energy -= 1;
				}
			}*/
			tick = 0;
		}
	}
	
	public List<string> GetSubsystemFacts()
	{
		List<string> temp = new List<string>();
		foreach(string fact in subsystemFacts)
		{
			temp.Add(fact);
		}
		subsystemFacts.Clear();
		return temp;
	}
	
	public void RemoveEnergy()
	{
		/*if(energy > 9)
		{
			energy -= 10;
		}*/
	}
}