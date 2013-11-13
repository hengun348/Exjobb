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
		actionsList.Add(new AimAction());
		actionsList.Add(new approachAction());
		actionsList.Add(new detonateBombAction());
		actionsList.Add(new fleeAction());
		actionsList.Add(new loadAction());
		actionsList.Add(new scoutAction());
		actionsList.Add(new shootAction());
		actionsList.Add(new BuildHouseAction());
		actionsList.Add(new CommanderBuildPyramidAction());
		actionsList.Add(new AgentBuildPyramidAction());
		actionsList.Add(new GetStoneAction());
		actionsList.Add(new GetWoodAction());
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
				if(action.containsPostCondition(pair.Key, !(bool)pair.Value.propertyValues["bool"]))
				{
					okayToAddAction = false;
					break;
				}

				else if(action.containsPostCondition(pair.Key, (bool)pair.Value.propertyValues["bool"]) && action.getAgentTypes().Contains(currentAgent)){
					okayToAddAction = true;
				}
					
				if(okayToAddAction == true){
					actionList.Add(action);
				}
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