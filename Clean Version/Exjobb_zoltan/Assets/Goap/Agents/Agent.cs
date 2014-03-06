using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System;
using System.IO;

public class Agent: MonoBehaviour
{
	private WorkingMemory wMemory;
	
	private Planner planner;
	public List<string> plan; //Contains actions that the agent should do
	
	private GameObject target; //The target position the agent is walking towards
	public System.Guid agentNumber; //a unique number identifying the agent
	private List<string> subsystemFacts; //contains information about which subsystems to initialize 
	
	public int totalSkillpoints; //the max number of skillpoint an agent can have and divide between the skills
	private Dictionary<string, float> skillArray; //Contains all the different skills an agent can have
	
	public float Buildskill, Collectskill; //Just for debugging to be able to see the skills of an agent 
	
	public Color agentColor; //The color of the agent
	
	private string clan; //The clan that the agent belongs to
	
	void Awake()
	{

		wMemory = new WorkingMemory();
		subsystemFacts = new List<string>();
		agentNumber = System.Guid.NewGuid();
	}
	
	void Start(){
		
		Buildskill = 0.0f;
		Collectskill = 0.0f;
		
		SetColor();

		totalSkillpoints = 100;
		float numberOfSkills = 2.0f;
		skillArray = new Dictionary<string, float>();
		skillArray.Add("buildSkillpoints", totalSkillpoints/numberOfSkills);
		skillArray.Add("collectSkillpoints", totalSkillpoints/numberOfSkills);

		planner = new Planner();
		plan = new List<string>();
		plan.Add("IdleAction"); //Set the agent to be Idle by default
		
		AddSensors();
		AddPathfinding();

	}
	
	void Update(){
		
		if((transform.childCount == 1  && plan.Count == 1)){ //Have a plan with only one action in it
			plan[0] = BlackBoard.Instance.GetActionForAgent(clan, this); //returns "IdleAction" if no actions are possible
			AddSubsystem();
			BlackBoard.Instance.GetTaskTree(clan).PrintTree();

		}else if(transform.childCount == 1 && plan.Count > 1){ //have a plan with several actions in it (dont need to search for new actions to do each time)
			
			plan.RemoveAt(0);
			AddSubsystem();

		}
	
		Collectskill = skillArray["collectSkillpoints"]; //For debugging, just updating the skillpoints
		Buildskill = skillArray["buildSkillpoints"]; //For debugging, just updating the skillpoints
	}
		
	private void AddSubsystem() //Adds the appropriate subsystem for the current action to be made
	{
		subsystemFacts.Clear();
		string subsystemToLoad = "";
		GameObject sub = new GameObject("Subsystem"); 
		sub.transform.position = transform.position; 
		sub.transform.parent = transform; 
	
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
		subsystemToLoad += "Subsystem" + clan.Substring(0, clan.Length-5) + "Clan" ;
		
		if (!System.IO.File.Exists(subsystemToLoad + ".cs")) //if the subsystem exist the load it
	    {
			subsystemToLoad = subsystemFacts[0] + "Subsystem";	
		}
		
		Debug.Log ("---- Adding subsystem: " + subsystemToLoad);
		
		sub.AddComponent(subsystemToLoad);
		sub.AddComponent("WalkSubsystem"); //also add the walksubsystem so the agent can move
		
	}
		
	private void AddSensors() //adds the sensors to the agent
	{
		GameObject sens = new GameObject("Sensors");
		sens.transform.position = transform.position;
		sens.transform.parent = transform;
		sens.AddComponent("Sensors");
	}
				
	private void AddPathfinding() //Adds the pathfinding compontents
	{
		target = new GameObject("Target");
		target.transform.parent = gameObject.transform.parent.transform;
		
		gameObject.AddComponent<LineRenderer>();
		gameObject.GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
		gameObject.GetComponent<LineRenderer>().material.shader = Shader.Find("Particles/Additive");
		Color clanColor = BlackBoard.Instance.GetColorForObject(clan.Substring(0, clan.Length-5));
		gameObject.GetComponent<LineRenderer>().SetColors(new Color(clanColor.r, clanColor.g, clanColor.b, 0), clanColor);
		gameObject.GetComponent<LineRenderer>().enabled = false;
		
		((AIPath)gameObject.GetComponent("AIPath")).target = target.transform;
	}
	
	private void ActionTookTooLong() //Is called if an action is taken too long
    {		
			Debug.Log("!!!!!!!PLANEN HAR TAGIT FÖR LÅNG TID!!!!!!");
        	//currentAction = ""; 
			//planIndex = 0;
			//plan.Clear();
			//status = "available"
			
			//Tell commander plan failed
			
    }
	
	private void SetColor(){ //The the color of the agentObject

		agentColor = BlackBoard.Instance.GetColorForObject(name);
		gameObject.renderer.material.color = agentColor;
	}
	
	public WorkingMemory GetWMemory(){
		return wMemory;
	}
	
	public GameObject GetTarget()
	{
		return target;
	}
	
	public string GetAgentType() //Ex. blue agent -> return "Blue"
	{
		return name;
	}
	
	public System.Guid GetAgentNumber()
	{
		return agentNumber;
	}		
	
	public List<string> GetPlan()
	{
		return plan;
	}
		
	public void CreateNewPlan(WorldState currentWorldState, WorldState goalWorldState) //create a new plan if old plan fails for example
	{
		StopCoroutine(plan[0].Substring(0, plan[0].Length - 6));
		Destroy(transform.FindChild("Subsystem").gameObject);
		
		planner.SetGoalWorldState(goalWorldState);
		
		//Saves current plan so that agent can continue after new plan is completed 
		List<string> tempPlan = new List<string>();
		foreach(string action in plan)
		{
			tempPlan.Add(action);
		}
		
		List<string> newPlanList = new List<string>();
		foreach (AStarNode node in planner.RunAStar(currentWorldState))
		{
			newPlanList.Add(node.GetName());
		}
			
		if(newPlanList.Count == 0)
		{
			//A new plan could not be made so continue on the old plan
			//Implement some kind of report up to commanders?
			Debug.Log("NEW PLAN COULD NOT BE MADE!!");
		} else 
		{
			//A new plan was created, is added in front of old plan 
			plan.Clear();
			plan.Add ("Crap"); //Added because remove plan[0] in update
			plan.InsertRange(1, newPlanList);
			plan.InsertRange(plan.Count, tempPlan);
		}
	}
	
	public void SetSkillpoints(string inputSkill) //Update a skill for an agent, ex. SetSkillPoint("build") and change the color appropriately
	{
		List<string> keys = new List<string>(skillArray.Keys);
		
		if(skillArray[inputSkill + "Skillpoints"] < 81) // 2 skills, totalSkillpoints = 100, 80 + 10 = 90 -> 10 skillpoints left for other skill (cant be 0 skilled at a skill)
		{
			Debug.Log("--------------------> " + inputSkill);
			foreach(string key in keys)
			{
			    if(key.Substring(0, key.Length - 11) == inputSkill) //if skill is the same as inputSkill get better at it
				{
					skillArray[key] += 10;
					
					if(inputSkill == "build") //Skill is build, get paler color
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
					else{ //Collect, get intenser color
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
					
				}else //if skill not the same as inputskill then get worse at it 
				{	
					skillArray[key] -= (10/(skillArray.Count-1));
			
				}
			}
		}
	}
	
	public List<string> GetSubsystemFacts()
	{

		return subsystemFacts;
	}
	
	public void SetSubsystemFacts(string fact)
	{
		subsystemFacts.Add(fact);
	}
	
	public void SetClan(string clan) //set the clan of the agent
	{
	 	this.clan = clan;
		wMemory.SetClan(clan);
	}
	
	public string GetClan()
	{
	 	return clan;
	}
	
	public float GetSkill(string skill)
	{
		return skillArray[skill];
	}
}