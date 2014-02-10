using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ground: MonoBehaviour
{
	SmoothFollow follower;
	
	void Start()
	{
		follower = (SmoothFollow)Camera.main.gameObject.GetComponent("SmoothFollow");
	}
	
	void OnMouseDown()
	{
		follower.target = gameObject.transform;
		follower.height = 20;
		follower.distance = 20;
		
		foreach(string  clan in BlackBoard.Instance.GetClans())
		{
			foreach (WorkingMemoryValue agent in BlackBoard.Instance.GetFact(clan, "Agents"))
			{
				((Agent)agent.GetFactValue()).GetComponent<LineRenderer>().enabled = false;
				
				if(((Agent)agent.GetFactValue()).gameObject.transform.childCount == 4)
				{
				//Debug.Log ("DESTROY " + ((Agent)agent.GetFactValue()).gameObject.transform.FindChild("PlanGUI").gameObject.name);
					
					Destroy(((Agent)agent.GetFactValue()).gameObject.transform.FindChild("PlanGUI").gameObject);
					((Agent)agent.GetFactValue()).ShowPlan(false);
				}
			}
		}
		
		
	}
}