using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarNodecs
{
    public Vector2Int gridPosition;
    public bool isObstaacle = false;
    public List<AStarNodecs> neighbours = new List<AStarNodecs>();
    public float gCostDistanceFromStart = 0;  
    public int hCostDistanceFromGoal = 0;
    public float fCostTotal = 0; 
    public int pickedOrder = 0;
    public bool isSlough = false;

    private bool isCostsCalculated = false;

    public AStarNodecs(Vector2Int gridPosition_)
    {
        gridPosition = gridPosition_;
    }

    public void CalculateCostsForNode(Vector2Int aiPosition, Vector2Int aiDestination)
    {
        if (isCostsCalculated)
            return;

        gCostDistanceFromStart = Mathf.Abs(gridPosition.x - aiPosition.x) + Mathf.Abs(gridPosition.y - aiPosition.y);
        hCostDistanceFromGoal = Mathf.Abs(gridPosition.x - aiDestination.x) + Mathf.Abs(gridPosition.y - aiDestination.y);
        fCostTotal = gCostDistanceFromStart + hCostDistanceFromGoal;
        isCostsCalculated = true;
    }

    public void Reset()
    {
        isCostsCalculated = false;
        pickedOrder = 0;
        gCostDistanceFromStart = 0;
        hCostDistanceFromGoal = 0;
        fCostTotal = 0;
    }
}

