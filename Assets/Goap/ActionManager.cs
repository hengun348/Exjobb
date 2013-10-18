using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager {
	
	public List<Action> actionsList;
	private Dictionary<Action, string> actionsSuitableForGoal;
	private static ActionManager instance;
	
	private ActionManager()
	{
		
		//All possible actions
		actionsList = new List<Action>();
		actionsList.Add(new walkAction());
		actionsList.Add(new jumpAction());
		actionsList.Add(new jumpHigherAction());
		actionsList.Add(new reloadAction());
		
		
		//What actions will result in desired state?
		actionsSuitableForGoal = new Dictionary<Action, string>();
		actionsSuitableForGoal.Add(new jumpAction(), "hasJumped");
		actionsSuitableForGoal.Add(new jumpHigherAction(), "hasJumped");
		actionsSuitableForGoal.Add(new reloadAction(), "hasJumped");
		actionsSuitableForGoal.Add(new walkAction(), "reachedDestination");
		
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
	
	public List<Action> getSuitableActions(string goal)
	{
		
		//return the list with actions suitable for a certain goal
		List<Action> actionList = new List<Action>();
		
		foreach(KeyValuePair<Action, string> pair in actionsSuitableForGoal)
		{
			
			if(pair.Value == goal)
			{
				
				actionList.Add(pair.Key);
				
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
	
	/*public void addAction(Action action)
	{
		actionsList.Add(action);
	}
	
	public void removeAction(Action action)
	{
		actionsList.Remove(action);
	}
	
	public List<Action> getActionForKey(string name)
	{
		return actionsList.FindAll(s => s.Equals(name));
	}
	*/
	
}