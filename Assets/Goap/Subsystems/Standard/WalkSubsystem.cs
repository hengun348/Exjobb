using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkSubsystem : MonoBehaviour{
	
	public bool hasArrived;
	Vector3 moveToPosition;
	Agent agentComponent;
	AIPath walker;
	GameObject agentObject;
	AudioSource walkSound;
	LineRenderer pathLine;
	
	void Awake()
	{
		agentObject = gameObject.transform.parent.gameObject;
		walker = (AIPath)agentObject.GetComponent("AIPath");
		agentComponent = (Agent)agentObject.GetComponent("Agent");
		walkSound = (AudioSource)gameObject.AddComponent("AudioSource");
		pathLine = agentObject.GetComponent<LineRenderer>();
		//Set speed of agent depending on its energy
		//walker.speed = 0.1f * agentComponent.GetEnergy();
		walker.speed = 10;
		hasArrived = false;
	}
	
	void Start()
	{
		AudioClip walkingSounds;
		walkingSounds = (AudioClip)Resources.Load ("SFX/people002");
		walkSound.clip = walkingSounds;
		walkSound.loop = true;
		walkSound.maxDistance = 10.0f;
		walkSound.minDistance = 1.0f;
		walkSound.volume = 0.5f;
		
	}
	
	public void StartWalking(Vector3 moveToPosition) //TODO: kontrollera att man inte går utanför griden
	{
		if(!walkSound.isPlaying)
		{
			walkSound.Play();
		}
		hasArrived = false;
		walker.canMove = true;
		walker.canSearch = true;
		agentComponent.getTarget().transform.position = moveToPosition;
		walker.target = agentComponent.getTarget().transform;
		//Debug.Log ("!!!!!!!!!!!!!!!!!!!!!!!!!!! Går till " + moveToPosition);
		
		if(Vector3.Distance(agentObject.transform.position, moveToPosition) < 2.0f)
		{
			walker.canMove = false;
			walker.canSearch = false;
			hasArrived = true;
			walkSound.Stop();
		}
	}
	
	void Update()
	{
		List<Vector3> lastCompletedVectorPath = ((Seeker)agentObject.GetComponent("Seeker")).lastCompletedVectorPath;
		
		if (lastCompletedVectorPath != null) {
				pathLine.SetVertexCount(lastCompletedVectorPath.Count);
			for (int i=0;i<lastCompletedVectorPath.Count-1;i++) {
				pathLine.SetPosition(i, lastCompletedVectorPath[i]);
			}
		}
	}
}