using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    [Header(" Bản đồ đi của AI")]

    public float minDistanceToReachWaypoin = 5;

    public WaypointNode[] nextWaypointNode;
}
