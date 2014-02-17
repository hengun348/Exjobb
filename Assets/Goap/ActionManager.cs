using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.IO;

public class ActionManager /*: MonoBehaviour*/{
	
	public List<Action> actionsList = new List<Action>();
	private Dictionary<Action, string> actionsSuitableForGoal;
	private static ActionManager instance;
	public string currentAgent;
	
	private ActionManager()
	{
		
		//Dynamically add all actions
		
		var type = typeof(Action);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
				.Where(p => type.IsAssignableFrom(p) && p.GetType() != type);

		foreach(var actionType in types)
		{
			actionsList.Add((Action)Activator.CreateInstance(actionType));
		}
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
	
	public WorldState GetGoal(int slump)
	{
		string key = "";
		WorldStateValue val = new WorldStateValue(true);
		Action action = actionsList[slump];//.GetActionName();
		foreach(KeyValuePair<string,WorldStateValue> pair in action.postConditions.getProperties())
		{
			key = pair.Key;
			val = pair.Value;
			break;
		}
		//int slump = 3;
		/*string toBuild;
		
		string action = actionsList[slump].GetActionName();
		
		for (int i = 1; i < action.Length; i++) 
		{ 
				if (char.IsUpper(action[i])) 
				{ 	
					toBuild = action.Substring(i, action.Length-6); 
					break; 
				} 
		}
		
		
		if(action.Substring(0, 1) == "B")
		{
			toBuild = action.Substring(5, action.Length-6);
			char.ToLower(toBuild[0]);
			//toBuild = char.ToLower(toBuild[0]) + toBuild.Substring(1);
			
			Debug.Log ("------------------------------>GetGoal()" + toBuild);
			return new WorldState(toBuild + "IsBuilt", new WorldStateValue(true));
		}*/
		//Debug.Log ("------------------------------>GetGoal() = " + key);
		return new WorldState(key, val);
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
			if (action.GetActionName() == name)
			{
				return action;
			}
		}
		return new Action();
	}
	
	
	public List<string> AssistingAgentsToAction(string agent, string actionName)
	{
		
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
	
	public List<List<string>> AgentsThatDoAction(string actionName) //Returns lists of agents that can do an action
	{
		Action action = getAction(actionName);
		List<string> agents = action.GetAgentTypes();
		List<List<string>> returnAgents = new List<List<string>>();
		
			foreach(string str in agents){
				if(str.Contains("&"))
				{
					List<string> tempList = new List<string>();
					string[] temp = str.Split('&'); 
					
						foreach(string tempAgent in temp ){

							tempList.Add (tempAgent);
							
						}
					returnAgents.Add(tempList);
				} else {
					List<string> temp2 = new List<string>();
					temp2.Add (str);
					returnAgents.Add(temp2);
				}
			}	
		
		return returnAgents;
	}
	
	public int NumberOfActions()
	{
		return actionsList.Count;
	}
}