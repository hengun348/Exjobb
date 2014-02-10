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
	private GameObject gui, target, clanRing;
	public System.Guid agentNumber;
	private List<string> subsystemFacts; 
	
	public int totalSkillpoints, energy;
	private Dictionary<string, float> skillArray;

	private int tick, unhappyTick;
	
	public float Buildskill, Collectskill;
	
	public Color agentColor;
	
	public string clan;
	private string mood;
	
	private bool showPlan;
	
	void Awake()
	{
		//WORKING MEMORY
		wMemory = new WorkingMemory();
		
		subsystemFacts = new List<string>();
		
		blackBoard = BlackBoard.Instance;
		
		agentNumber = System.Guid.NewGuid();
		
		
		showClanGUI();
		showPlan = false;
	}
	
	void Start(){
		Buildskill = 0.0F;
		Collectskill = 0.0F;
		
		SetColor();

		totalSkillpoints = 100;
		float numberOfSkills = 2.0f;
		skillArray = new Dictionary<string, float>();
		skillArray.Add("buildSkillpoints", totalSkillpoints/numberOfSkills);
		skillArray.Add("collectSkillpoints", totalSkillpoints/numberOfSkills);

		planner = new Planner();
		plan = new List<string>();
		plan.Add("IdleAction");
		
		AddSensors();
		AddPathfinding();
		
		tick = 0;
		energy = 93;

		mood = "happy";
		
		
	}
	
	void Update(){
		
		//Set speed of agent depending on its energy
		//((AIPath)gameObject.GetComponent("AIPath")).speed = 0.1f * energy;
		
		if((((showPlan == true && transform.childCount == 3) || (showPlan == false && transform.childCount == 2))  && plan.Count == 1)){
			//blackBoard.GetTaskTree(clan).RemoveNode(this);
			plan[0] = blackBoard.GetActionForAgent(clan, this); //returns "IdleAction" if no actions are possible
			if(showPlan == true)
			{
				StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(0.0f, 1.0f));//fades the gui out	
				StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(1.0f, 1.0f)); // fades the gui in
			}
			blackBoard.GetTaskTree(clan).PrintTree();
			AddSubsystem();
			
		}else if(transform.childCount == 2 && plan.Count > 1){
			//ta bort som ägare till noden? och gör resten av planen
			if(showPlan == true)
			{
				StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(0.0f, 1.0f));//fades the gui out	
				StartCoroutine(((PlanGUI)gameObject.transform.FindChild("PlanGUI").GetComponent("PlanGUI")).FadeTo(1.0f, 1.0f)); // fades the gui in
			}
			
			plan.RemoveAt(0);
			AddSubsystem();
		}
	
		
		
		//Collectskill = skillArray["collectSkillpoints"];
		//Buildskill = skillArray["buildSkillpoints"];
		
	}
	
	public int GetTick()
	{
		return tick;	
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
		Debug.Log ("++++++++++++++ LÄGGER TILL SUBSYSTEM " + plan[0]);
		subsystemFacts.Clear();
		string subsystemToLoad = "";
		GameObject sub = new GameObject("Subsystem"); 
		sub.transform.position = transform.position; 
		sub.transform.parent = transform; 
		/*if(plan[0] == "Idle")
		{
			subsystemToLoad = "IdleSubsystem"; 
		}else{
			//subsystemToLoad = plan[0].Substring(0, plan[0].Length - 6) + "Subsystem"; 
			
		*/	
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
		//}
		sub.AddComponent(subsystemToLoad); 
		sub.AddComponent("WalkSubsystem"); 
	}
		
	private void AddSensors()
	{
		GameObject sens = new GameObject("Sensors");
		sens.transform.position = transform.position;
		sens.transform.parent = transform;
		sens.AddComponent("Sensors");
	}
		
	private void showClanGUI()
	{
		clanRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		clanRing.name = "ClanRing";
		clanRing.transform.position = transform.position - new Vector3(0, 0.55f, 0);
		clanRing.transform.parent = transform;
		clanRing.transform.localScale =  new Vector3(1.5f,0.05f,1.5f);	
	}
	
	private void showPlanGUI()
	{
		gui = GameObject.CreatePrimitive(PrimitiveType.Quad);
		gui.name = "PlanGUI";
		gui.collider.isTrigger = true;
		gui.transform.position = transform.position + new Vector3(0, 1.0f, 0);
		gui.transform.parent = transform;
		gui.AddComponent("PlanGUI");
		showPlan = true;
	}
		
	private void AddPathfinding()
	{
		target = new GameObject("Target");
		target.transform.parent = gameObject.transform.parent.transform;
		
		gameObject.AddComponent("Seeker");
		gameObject.AddComponent("AIPath");
		gameObject.AddComponent<LineRenderer>();
		gameObject.GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
		gameObject.GetComponent<LineRenderer>().material.shader = Shader.Find("Particles/Additive");
		Color clanColor = blackBoard.GetColorForObject(clan.Substring(0, clan.Length-5));
		gameObject.GetComponent<LineRenderer>().SetColors(new Color(clanColor.r, clanColor.g, clanColor.b, 0), clanColor);
		gameObject.GetComponent<LineRenderer>().enabled = false;
		
		
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
		/*string clanColor = "";
		for (int i = 1; i < clan.Length; i++) 
		{ 
			if (char.IsUpper(clan[i])) 
			{ 	
				clanColor = clan.Substring(0, i-1); 
				break; 
			} 
		}
		
		agentColor = BlackBoard.Instance.GetColorForObject(clanColor);*/
		agentColor = BlackBoard.Instance.GetColorForObject(name);
		gameObject.renderer.material.color = agentColor;
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
		return name;
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

		//Debug.Log("nu blev jag bättre på: " + inputSkill);
	}
	
	void FixedUpdate()
	{
		tick ++;
		
		
		if(tick == 50)
		{
			if(Vector3.Distance(transform.position, new Vector3(30, 0, 30)) <= 2.0f) //Regenerate energy if at a specific position
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
			}
			tick = 0;
		}
		
		if(mood == "unhappy")
		{
			unhappyTick++;
			
			if(unhappyTick >= 250)
			{
				Migrate();
				mood = "happy";
				energy = 100000; //OBSOBSOBSOBSOBSOBS
				unhappyTick = 0;
			}
		} else {
		
			unhappyTick = 0;
		}
		
		if(energy < 90 && mood == "happy")
		{
			mood = "unhappy";		
		}
		
		if (energy >= 90 && mood == "unhappy")
		{
			mood = "happy";
		}
		
	}
	
	private void Migrate()
	{
		//Debug.Log ("!!!!!!!!!!!SMELL YA LATER!!!!!!");
		
		blackBoard.ChangeNumberAgentsInClan(clan, -1);
		int clanContainsNumberColorAgents = 0;
		
		foreach(Agent agent in ((UnitCommander)GameObject.Find(clan).transform.FindChild("Commanders").FindChild("UnitCommander(Clone)").GetComponent("UnitCommander")).GetAgents())
		{
			if(agent.getAgentType() == getAgentType())
			{
				clanContainsNumberColorAgents ++;
			}
		}
		
		if(clanContainsNumberColorAgents == 1)
		{
			blackBoard.RemoveColorFromClan(clan, getAgentType());
		}
		
		blackBoard.SortClanScores();
		List<string> clans = blackBoard.GetClans();
	
		
		
		if(clan == clans[0] && clans.Count > 1)
		{
			SetClan(clans[1]);
		}
		else
		{
			SetClan(clans[0]);
		}
			
		blackBoard.ChangeNumberAgentsInClan(clan, 1);
		transform.parent.parent = GameObject.Find (clan).transform.FindChild("Agents");
		blackBoard.RemoveAgentFromOwnedNode(clan, this);
		Destroy(gameObject.transform.FindChild("Subsystem").gameObject);
		plan[0] = "MigrateAction";
		AddSubsystem();
	
		//Tell the commander of the reciving clan that new agents are arriving
		((BuildingCommander)GameObject.Find(clan).transform.FindChild("Commanders").FindChild("BuildingCommander(Clone)").GetComponent("BuildingCommander")).ImmigratingAgentArriving(name);
	
	}
	
	
	public List<string> GetSubsystemFacts()
	{
		/*List<string> temp = new List<string>();
		foreach(string fact in subsystemFacts)
		{
			temp.Add(fact);
		}
		subsystemFacts.Clear();*/
		return subsystemFacts;
	}
	
	public void SetSubsystemFacts(string fact)
	{
		subsystemFacts.Add(fact);
	}
	
	public void RemoveEnergy()
	{
		if(energy > 9)
		{
			energy -= 10;
		}
	}
	
	public void SetClan(string clan)
	{
		Color color = blackBoard.GetColorForObject(clan.Substring(0, clan.Length-5));
		clanRing.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		Color newColor = new Color(color.r, color.g, color.b, 0.5f);
		clanRing.transform.renderer.material.color = newColor;
		
	 	this.clan = clan;
		wMemory.SetClan(clan);
		
		List<WorkingMemoryValue> red = wMemory.GetFact("Red");
		List<WorkingMemoryValue> blue = wMemory.GetFact("Blue");
		List<WorkingMemoryValue> yellow = wMemory.GetFact("Yellow");
		
		//TODO: usch va fult
		
		int redCount = red.Count;
		int blueCount = blue.Count;
		int yellowCount = yellow.Count;
		
		for(int i=0; i< redCount; i++)
		{
			BlackBoard.Instance.SetFact(clan, "Red", red[i]);
		}
		
		for(int i=0; i< blueCount; i++)
		{
			BlackBoard.Instance.SetFact(clan, "Blue", blue[i]);
		}
		
		for(int i=0; i< yellowCount; i++)
		{
			BlackBoard.Instance.SetFact(clan, "Yellow", yellow[i]);
		}
		
	}
	
	public string GetClan()
	{
	 	return clan;
	}
	
	void OnMouseDown() 
	{ 
		SmoothFollow follower = (SmoothFollow)Camera.main.gameObject.GetComponent("SmoothFollow"); 
		follower.target = gameObject.transform; 
		follower.height = 5; 
		follower.distance = 15;
		gameObject.GetComponent<LineRenderer>().enabled = true;
		if(showPlan == false)
		{
			showPlanGUI();
		}
	}
	
	public void ShowPlan(bool show)
	{
		showPlan = show;
	}
}