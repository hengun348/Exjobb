using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent {
	
	/*public float health;
	public Vector3 position;
	public Vector3 facing;
	public ActionManager AManager;
	public WorkingMemory wMemory;
	public Planner planner;
	public BlackBoard bBoard;
	*/
	
	public List<Action> availableActions;
	public WorldState wState;
	
	public bool hasAction(Action action)
	{
		if(availableActions.Contains(action))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	// Use this for initialization
	/*void Start () {
		wMemory = new WorkingMemory();
		planner = new Planner();
		AManager = new ActionManager();
		bBoard = new BlackBoard();
		
		//------------------Walking action
		Action walk = new Action();
		
		walk.preConditions = new WorldState();
		walk.preConditions.setProperty("hasDestination", new WorldStateValue<bool>(true));
		
		walk.postConditions = new WorldState();
		walk.postConditions.setProperty("reachedDestination", new WorldStateValue<bool>(true));
		
		walk.cost = 4.0f;
		
		AManager.addAction(walk);
		
		//------------------Jump action
		Action jump = new Action();
		
		jump.preConditions = new WorldState();
		jump.preConditions.setProperty("needToJump", new WorldStateValue<bool>(true));
		
		jump.postConditions = new WorldState();
		jump.postConditions.setProperty("needToJump", new WorldStateValue<bool>(false));
		
		jump.cost = 4.0f;
		
		AManager.addAction(jump);
		
		//---------------Working memory
		wMemory.setFact(WorldState.getProperties());
		
	}*/
}