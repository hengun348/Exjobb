using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserGUI : MonoBehaviour
{
    public Rect labelPosition; //Set labelPosition public so it can be changed in inspector (editor). 
    string labelText;
    public GUIStyle labelStyle;
	List<string> clans;
	
	void Start()
	{
		clans = BlackBoard.Instance.GetClans();
	}
    

    void Update()
    {
        //score = BlackBoard.Instance.GetScore();
        //labelText = "Score - " + score.ToString();
		labelText = "Score -";
		
		
		foreach(string clan in clans)
		{
			labelText += " " + clan + ": " + BlackBoard.Instance.GetScore(clan) + " PopCap: " + BlackBoard.Instance.GetAgentsInClan(clan) + "/" + BlackBoard.Instance.GetPopulationCap(clan) + " |";
		}
		
    }

    void OnGUI()
    {
        GUI.Label(labelPosition, labelText, labelStyle);
    }
}