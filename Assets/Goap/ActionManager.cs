using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager {
	
	public List<Action> actionsList;
	private Dictionary<Action, string> actionsSuitableForGoal;
	private static ActionManager instance;
	public string currentAgent;
	
	private ActionManager()
	{
		//All possible actions
		actionsList = new List<Action>();
		actionsList.Add(new aimAction());
		actionsList.Add(new approachAction());
		actionsList.Add(new detonateBombAction());
		actionsList.Add(new fleeAction());
		actionsList.Add(new loadAction());
		actionsList.Add(new scoutAction());
		actionsList.Add(new shootAction());
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
			
			foreach(KeyValuePair<string, bool> pair in postCon.getProperties())
			{
				if(action.containsPostCondition(pair.Key, !pair.Value))
				{
					Debug.Log("Här är inte roligt att hamna!");
					okayToAddAction = false;
					break;
				}
				else if(action.containsPostCondition(pair.Key, pair.Value) && action.getAgentTypes().Contains(currentAgent)){
				
					okayToAddAction = true;
				}
					
			}
			//if(action.containsPostCondition(pair.Key, pair.Value) && action.getAgentTypes().Contains(currentAgent.agentType) /*&& action.containsPostCondition(pair.Value, !pair.Value)*/){
					
				if(okayToAddAction == true){
					Debug.Log("***********Lägger till action " + action.actionName);
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
}