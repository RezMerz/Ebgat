using System;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

/// <summary>
/// This component discovers servers in the LAN so you can easily join them
/// </summary>
public class LANDiscoveryManager : NetworkDiscovery
{
    /// <summary>
    /// Is called when we find a server
    /// </summary>
    public Action<string,string> OnServerFound;

    /// <summary>
    /// Called by unity when we receive a broadcast message from a server
    /// </summary>
    /// <param name="fromAddress"></param>
    /// <param name="data"></param>
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if (OnServerFound != null)
        {
            Debug.Log(fromAddress + " " + data);
            OnServerFound(fromAddress, data);
        }
    }
}
