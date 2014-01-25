using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class Sensors: MonoBehaviour {
	
	void Start(){

		DirectoryInfo info = new DirectoryInfo("Assets/Goap/Sensors");
		FileInfo[] fileInfo = info.GetFiles();
		foreach(FileInfo file in fileInfo)
		{
			string filePath = file.ToString();
			int index = filePath.LastIndexOf(@"\") + 1;
			string fileName = filePath.Substring(index);
			fileName = fileName.Substring(0, fileName.Length-3);
			if(fileName != "Sensors")
			{
				GameObject sens = new GameObject(fileName);
				sens.transform.position = transform.position;
				sens.transform.parent = transform;
				sens.AddComponent(fileName);
			}
		}
	}	
}