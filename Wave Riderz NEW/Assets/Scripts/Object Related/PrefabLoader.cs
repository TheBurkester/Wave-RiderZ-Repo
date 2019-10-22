using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PrefabLoader : MonoBehaviour
{
	private string[] m_prefabFiles;


    void Start()
    {
		m_prefabFiles = Directory.GetFiles(Application.dataPath + "/Resources", "*.prefab");
		foreach (string path in m_prefabFiles)
		{
			if (File.Exists(path))
			{
				Console.WriteLine("Processed file '{0}'.", path);
			}
		}
		//GameObject test = Instantiate(Resources.Load<GameObject>("largeRock")) as GameObject;

		//object test = Resources.Load<GameObject>("") as object;

		GameObject[] prefabs = Resources.LoadAll<GameObject>("Obstacles");
	}
}
