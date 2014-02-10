using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Temple: MonoBehaviour
{
	ParticleSystem ps;
	string clan, color;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		
		//To get the correct color for the temple
		string color = "";
		for (int i = 1; i < clan.Length; i++) 
		{ 
			if (char.IsUpper(clan[i])) 
			{
				color = clan.Substring(0, i - 1); 
				break; 
			} 
		}
		ps.startColor = BlackBoard.Instance.GetColorForObject(color);
		
		ps.Stop();
	}
	
	public void Sacrifice()
	{
		ps.Play();
	}
	
	public bool IsSacrificing()
	{
		return ps.isPlaying;
	}
	
	public void SetClan(string clan)
	{
	 	this.clan = clan;
	}
}