using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// uNet doesn't have a global time value synchronized by the server so I'm creating one here, This is not as great as uLink's one but is good enough.
/// The uLink one uses an algorithm used in HFT (high frequency trading) applications
/// Later on that can be implemented from a paper but we don't use the time for anything critical in milliseconds
/// </summary>
public class NetworkTime : NetworkBehaviour
{
	private const int DELAY_BETWEEN_SYNCS = 6;//10 times per minute
	public static NetworkTime instance;
    private static float serverTime;
    private static float lastSyncTime;
    private static float delay;

    public static void Reset()
    {
        serverTime = 0;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Only one instance of network time should be present");
        }
    }

    public override void OnStartServer()
    {
        StartCoroutine(SendTimeToClients());
    }

    IEnumerator SendTimeToClients()
    {
        while (isServer)
        {
            RpcSyncServerTimeOnClients(GetServerTime(), NetworkTransport.GetNetworkTimestamp());
            yield return new WaitForSeconds(DELAY_BETWEEN_SYNCS);
        }
    }

    [ClientRpc]
    public void RpcSendTimeToSpecificClient(int playerID, float sentServerTime, int timestamp)
    {
        if (isServer)
            return;
        if (MatchManager.instance.playerID == playerID)
        {
            CalculateTimeOnClient(sentServerTime, timestamp);
        }
    }

    [ClientRpc]
    private void RpcSyncServerTimeOnClients(float sentServerTime, int timestamp)
    {
        if (!isServer)
            CalculateTimeOnClient(sentServerTime, timestamp);
    }

    public static void InitializeTimeOnApproval(float sentServerTime, int timestamp)
    {
        CalculateTimeOnClient(sentServerTime, timestamp);
    }

    private static void CalculateTimeOnClient(float sentServerTime, int timestamp)
    {
        lastSyncTime = Time.time;
		//This method converts the server sent timestamp to the delay between the time that the timestamp was created
		//and current moment which we received it and are processing it
        delay = MatchManager.instance.GetTimeDelayOnClient(timestamp);
		//Due to error and on WebGL the value might have not been calculated
        if (delay < 0) delay = 0;
        serverTime = sentServerTime + delay;
    }

	/// <summary>
	/// Gets the server synchronized time
	/// If not initialized/synced yet would return 0
	/// </summary>
	/// <returns></returns>
    public float GetServerTime()
    {
        if (isServer)
            return Time.time;
        else
            return (serverTime > 0) ? serverTime + Time.time - lastSyncTime : 0;
    }

	/// <summary>
	/// Gets the server synchronized time
	/// If not initialized/synced yet would return 0
	/// </summary>
	public static float time { get { return instance.GetServerTime(); } }

}
