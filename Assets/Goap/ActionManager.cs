using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour {
	
	public List<Action> actionsList;

	public void addAction(Action action)
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
	
}