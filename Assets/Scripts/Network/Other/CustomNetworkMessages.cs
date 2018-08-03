using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// You should put codes for all of your custom messages in this file and each message code should be incremented by one compared to the previous one
/// codes up to MsgTyp.Highest are reserved by Unity and should not be used
/// </summary>
public class CustomNetworkMessages
{
	/// <summary>
	/// Server sends an integer message to each client using this and puts the client's ID on it
	/// </summary>
	public static short SetPlayerIdOnClient = MsgType.Highest + 1;
}
