using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState : MonoBehaviour {
	
	public Dictionary<string, WorldStateProperty> properties;
	
	public Dictionary<string, WorldStateProperty> getProperties()
	{
		return properties;
	}
	
	public void setProperty(string name, WorldStateProperty property)
	{
		properties.Add(name, property);
	}
	
	public WorldStateProperty getProperty(string name)
	{
		return properties[name];
	}
	
	public void removeProperty(string name)
	{
		properties.Remove(name);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}