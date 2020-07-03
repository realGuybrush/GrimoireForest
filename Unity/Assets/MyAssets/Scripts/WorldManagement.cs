using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*public class AnimatorProperties
{
	public string state_name = "";
	public List<string>BoolsNames = new List<string>();
	public List<bool>Bools = new List<bool>();
	public List<string>FloatsNames = new List<string>();
	public List<float>Floats = new List<float>();
	//public List<bool>Bools = new List<bool>();
	//public List<bool>Bools = new List<bool>();
}*/

public class WorldManagement : MonoBehaviour 
{
	//public string initial_scene_data;
	//public List<Scene> Scenes = new List<Scene>();
	public List<GameObject> ItemPrefabs = new List<GameObject>();
	//public List<GameObject> CloneItemPrefabs = new List<GameObject>();
	//public List<int> APnum = new List<int>();
	//public List<AnimatorProperties>AP = new List<AnimatorProperties>();
	//public int current_Scene;
	//public Font font;
	//public Passage Pass = null;
	// Use this for initialization
	void Start () 
	{
	
		//current_Scene = 0;//move it in load after it is fixed
		//LoadInitialScenesData();
		//SaveItemsData();
		//GameObject.Find("Player").GetComponent<Inventory>().takeall = true;
		//LoadWHUD();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if(Pass != null)
			//Enter();
	}
}
