using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackBoard {
	
	public List<Subsystem> subSystems;
	
	private string action;
	
	public BlackBoard(){
	
		subSystems = new List<Subsystem>();
		
		subSystems.Add(new WalkSubsystem(this));
		subSystems.Add(new JumpSubsystem(this));
		subSystems.Add(new ApproachSubsystem(this));
		subSystems.Add(new DetonateBombSubsystem(this));
		subSystems.Add(new ScoutSubsystem(this));
	}
	
	
	
	public string getCurrentAction(){
		
		
		
		return action;
	}
	
	public List<Subsystem> getSubsystems()
	{
		return subSystems;
	}
	
	public void setCurrentAction(string action){
	
		this.action = action; 
		
	}
	
}