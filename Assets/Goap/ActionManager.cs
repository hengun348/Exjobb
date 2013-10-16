using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager {
	
	public List<Action> actionsList;
	public Dictionary<string, Action> actionsSuitableForGoal;

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
	public ActionManager()
	{
		
		//All possible actions
		actionsList.Add(new walkAction());
		actionsList.Add(new jumpAction());
		
		//What actions will result in desired state?
		actionsSuitableForGoal.Add("needToJump", jump);
		actionsSuitableForGoal.Add("needToJump", jumpHigher);
		actionsSuitableForGoal.Add("reachedDestination", walk);
		
	}
	
}