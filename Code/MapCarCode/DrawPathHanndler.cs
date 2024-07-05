using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathHanndler : MonoBehaviour
{
    public Transform transformRootObject;
    private Color lineColor = Color.red; // Màu của đường vẽ

    // Phương thức để lấy danh sách các transform của waypoints
    public List<Transform> GetWaypointsTransforms()
    {
        List<Transform> transforms = new List<Transform>();

        if (transformRootObject != null)
        {
            // Add positions from Mapcar objects
            foreach (Mapcar mapcar in transformRootObject.GetComponentsInChildren<Mapcar>())
            {
                // Add position of current Mapcar
                transforms.Add(mapcar.transform);

                // Add positions from nextWaypointNode array of current Mapcar
                foreach (Mapcar nextWaypoint in mapcar.nextWaypointNode)
                {
                    if (nextWaypoint != null)
                    {
                        transforms.Add(nextWaypoint.transform);
                    }
                }
            }
        }

        return transforms;
    }

    public void OnDrawGizmos()
    {
        // Set gizmo color
        Gizmos.color = lineColor;

        // Lấy danh sách các transform của waypoints
        List<Transform> waypointsTransforms = GetWaypointsTransforms();

        // Vẽ đường vẽ nếu có waypoints
        if (waypointsTransforms.Count > 1)
        {
            for (int i = 0; i < waypointsTransforms.Count - 1; i++)
            {
                Gizmos.DrawLine(waypointsTransforms[i].position, waypointsTransforms[i + 1].position);
            }
        }
    }
}
