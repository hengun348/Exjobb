using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkSubsystem : MonoBehaviour{
	
	public bool hasArrived; //if the agent has arrived to the destination or not
	Vector3 moveToPosition;
	Agent agentComponent;
	AIPath walker; //the pathfinding plugin
	GameObject agentObject;
	AudioSource walkSound; //sound while walking
	LineRenderer pathLine; //a trail to be drawn behind the walker
	
	void Awake()
	{
		agentObject = gameObject.transform.parent.gameObject;
		walker = (AIPath)agentObject.GetComponent("AIPath");
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		walkSound = (AudioSource)gameObject.AddComponent("AudioSource");
		pathLine = agentObject.GetComponent<LineRenderer>();
		walker.speed = 10; //set the walking speed
		hasArrived = false;
	}
	
	void Start()
	{
		//start the walking sound
		AudioClip walkingSounds;
		walkingSounds = (AudioClip)Resources.Load ("SFX/people002");
		walkSound.clip = walkingSounds;
		walkSound.loop = true;
		walkSound.maxDistance = 10.0f;
		walkSound.minDistance = 1.0f;
		walkSound.volume = 0.5f;
		
	}
	
	public void StartWalking(Vector3 moveToPosition) //start walking towards moveToPosition
	{
		//TODO: kontrollera att man inte går utanför griden
		
		if(!walkSound.isPlaying)
		{
			walkSound.Play();
		}
		hasArrived = false;
		walker.canMove = true;
		walker.canSearch = true;
		agentComponent.GetTarget().transform.position = moveToPosition;
		walker.target = agentComponent.GetTarget().transform;
		
		float distanceToObject = 2.0f; // how close the agent should be to be considered to have arrived to it
				
		if(Vector3.Distance(agentObject.transform.position, moveToPosition) < distanceToObject)
		{
			walker.canMove = false;
			walker.canSearch = false;
			hasArrived = true;
			walkSound.Stop();
		}
	}
	
	void Update()
	{
		//get information to be used if the trail is going to get drawn
		List<Vector3> lastCompletedVectorPath = ((Seeker)agentObject.GetComponent("Seeker")).lastCompletedVectorPath;
		
		if (lastCompletedVectorPath != null) {
				pathLine.SetVertexCount(lastCompletedVectorPath.Count);
			for (int i=0;i<lastCompletedVectorPath.Count-1;i++) {
				pathLine.SetPosition(i, lastCompletedVectorPath[i]);
			}
		}
	}
}