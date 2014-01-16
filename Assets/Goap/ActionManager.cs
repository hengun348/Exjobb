using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ActionManager /*: MonoBehaviour*/{
	
	public List<Action> actionsList;
	private Dictionary<Action, string> actionsSuitableForGoal;
	private static ActionManager instance;
	public string currentAgent;
	
	private ActionManager()
	{
		
		//Dynamically add all actions
		
		DirectoryInfo info = new DirectoryInfo("Assets/Goap/Actions");
		FileInfo[] fileInfo = info.GetFiles();
		actionsList = new List<Action>();
		foreach(FileInfo file in fileInfo)
		{
			string filePath = file.ToString();
			int index = filePath.LastIndexOf(@"\") + 1;
			string fileName = filePath.Substring(index);
			fileName = fileName.Substring(0, fileName.Length-3);
			if(fileName != "Action")
			{
				var ci = Type.GetType(fileName).GetConstructor(Type.EmptyTypes);
				Action myTypeInstance = (Action)ci.Invoke(new object[]{});
				actionsList.Add(myTypeInstance);
			}
			
			
		}
		

		//All possible actions
		
		/*actionsList.Add(new BuildBlueHouseAction());
		actionsList.Add(new BuildRedHouseAction());
		actionsList.Add(new BuildPurpleHouseAction());
		actionsList.Add(new GetBlueAction());
		actionsList.Add(new GetRedAction());
		
		actionsList.Add(new AimAction());
		actionsList.Add(new ApproachAction());
		actionsList.Add(new DetonateBombAction());
		actionsList.Add(new FleeAction());
		actionsList.Add(new LoadAction());
		actionsList.Add(new ScoutAction());
		actionsList.Add(new ShootAction());
		actionsList.Add(new WalkAction());*/
	}
	
	public static ActionManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new ActionManager();
			}
			return instance;
		}
	}
	
	public List<Action> getSuitableActions(WorldState postCon)
	{
		//return the list with actions suitable for a certain goal
		List<Action> actionList = new List<Action>();
		
		bool okayToAddAction;
			
		foreach (Action action in actionsList){
			
			okayToAddAction = false;
			
			foreach(KeyValuePair<string, WorldStateValue> pair in postCon.getProperties())
			{
				if(action.ContainsPostCondition(pair.Key, !(bool)pair.Value.propertyValues["bool"]))
				{
					okayToAddAction = false;
					break;
				}

				else if(action.ContainsPostCondition(pair.Key, (bool)pair.Value.propertyValues["bool"]) /*&& action.getAgentTypes().Contains(currentAgent)*/){
					okayToAddAction = true;
				} else {
					okayToAddAction = false;
					break;
				}
					
				
			}
			
			if(okayToAddAction == true){
					actionList.Add(action);
				}
		}
		return actionList;
	}
	
	public Action getAction(string name)
	{
		foreach(Action action in actionsList)
		{
			if (action.actionName == name)
			{
				return action;
			}
		}
		return new Action();
	}
	
	
	public List<string> AgentsThatDoAction(string agent, string actionName){
		
		Action action = getAction(actionName);
		List<string> agents = action.GetAgentTypes();
		List<string> returnAgents = new List<string>();
		
	
		//will only contain the agent itself (size 1) if dont need help
		if(agents.Contains(agent)){
			returnAgents.Add(agent);
			return returnAgents;
			
		}else //return a list of agents that need to help current agent with the action, 
		{
			
			foreach(string str in agents){
				
				if(str.Contains("&"))
				{
					string[] temp = str.Split('&'); 
					if(Array.IndexOf(temp, agent) != -1){
						foreach(string tempAgent in temp ){

							returnAgents.Add (tempAgent);
							
						}
					}
				}
			}	
		}
		
		//return list with size 0 if cant do action 
		return returnAgents;
	}
}