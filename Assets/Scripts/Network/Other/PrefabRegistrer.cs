using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Each prefab which is spawned using the NetworkServer.Spawn method
/// or other means of spawning on the network should be registered on the client
/// We use this component to register them all in a single place
/// </summary>
public class PrefabRegistrer : MonoBehaviour
{
	/// <summary>
	/// All the prefabs which will be spawned on the network should be filled
	/// in here
	/// </summary>
	public GameObject[] prefabs;

	void Start()
	{
		ClientScene.ClearSpawners();
		for (int i = 0; i < prefabs.Length; ++i)
			ClientScene.RegisterPrefab(prefabs[i]);
	}

}
