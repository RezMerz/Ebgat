using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCode : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.H))
		{
			MatchManager.instance.CreateDedicatedServer();
			MatchManager.instance.ConnectLocally();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			MatchManager.instance.ConnectLocally();
		}


	}
}
