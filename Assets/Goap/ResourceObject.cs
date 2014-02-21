using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceObject: MonoBehaviour {
	
	public int amount;
	
	void Start()
	{
		amount = 3;
	}
	
	public void UpdateResource(string clanThatCollectedResource, int resourcesTaken)
	{
		amount = amount - resourcesTaken;
		//Debug.Log ("Recource -" + resourcesTaken + "-> " + amount);
		
		if(amount < 0)
		{
			//update blackboard
			/*string color = "";
			for(int i = 1; i < gameObject.tag.Length; i++)
			{
				if(char.IsUpper(gameObject.tag[i]))
				{
					color = gameObject.tag.Substring(0, i);
					break;
				}
			}
			
			BlackBoard.Instance.RemoveFact(clanThatCollectedResource, color, new WorkingMemoryValue(gameObject.transform.position));
			foreach(WorkingMemoryValue wm in BlackBoard.Instance.GetFact("Blue Clan", color))
			{
				Debug.Log ("!!!!!!!!!!!!! " + wm.GetFactValue());
			}
			*/
			Destroy(gameObject);
		}
	}
}