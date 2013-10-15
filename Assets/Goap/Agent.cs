using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {
	
	public float health;
	public Vector3 position;
	public Vector3 facing;
	public ActionManager AManager;
	public WorkingMemory wMemory;
	public Planner planner;
	
	
	// Use this for initialization
	void Start () {
		wMemory = new WorkingMemory();
		planner = new Planner();
		AManager = new ActionManager();
		
		//Walking action
		Action walk = new Action();
		AManager.addAction(walk);
		
		WorldStateProperty hasDestination = new WorldStateProperty();
		//hasDestination.setProperty("hasDestination", true);
		
		WorldStateProperty hasArrived = new WorldStateProperty();
		//hasArrived.setProperty("setArrived", true);
	
		walk.setPreCondition(hasDestination);
		walk.setPostCondition(hasArrived);
		walk.setCost(4.0f);
		
		//Jump action
		Action jump = new Action();
		AManager.addAction(jump);
		
		WorldStateProperty cantReach = new WorldStateProperty();
		//hasDestination.setProperty("cantReach", true);
		
		WorldStateProperty Reached = new WorldStateProperty();
		//hasArrived.setProperty("Reached", true);
	
		jump.setPreCondition(cantReach);
		jump.setPostCondition(Reached);
		jump.setCost(4.0f);
		
		
	}
}