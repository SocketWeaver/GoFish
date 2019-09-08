using System.Collections.Generic;
using SWNetwork;
using UnityEngine;

public class NonPlayerFilterable : MonoBehaviour
{
    NetworkID networkID;
    HashSet<GameObject> visiblePlayers = new HashSet<GameObject>();
    public float gizmoSize = 0.2f;

    bool shouldSendRealtimeData = false;

    void Start()
    {
        networkID = GetComponent<NetworkID>();
        networkID.SendRealtimeData = false;
    }

    void OnDrawGizmos()
    {
        if (shouldSendRealtimeData)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, gizmoSize);
        }
    }

    public void AddVisiblePlayer(GameObject playerGameObject)
    {
        visiblePlayers.Add(playerGameObject);
        UpdateShouldSendRealtimeData();
    }

    public void RemoveVisiblePlayer(GameObject playerGameObject)
    {
        visiblePlayers.Remove(playerGameObject);
        UpdateShouldSendRealtimeData();
    }

    void UpdateShouldSendRealtimeData()
    {
        if (visiblePlayers.Count > 0)
        {
            shouldSendRealtimeData = true;
        }
        else
        {
            shouldSendRealtimeData = false;
        }

        networkID.SendRealtimeData = shouldSendRealtimeData;
    }
}
