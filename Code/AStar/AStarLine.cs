using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class AStarLine : MonoBehaviour
{

    int gridSizeX = 50;
    int gridSizeY = 30;
    float cellSize = 2;

    AStarNodecs[,] aStarNodes;
    AStarNodecs startNode;

    List<AStarNodecs> nodesToCheck = new List<AStarNodecs>();
    List<AStarNodecs> nodesChecked = new List<AStarNodecs>();
    List<Vector2> aiPath = new List<Vector2>();
    Vector3 startPositionDebug = new Vector3(1000, 0, 0);
    Vector3 destinationPositionDebug = new Vector3(1000, 0, 0);

    void Start()
    {
        CreateGrid();
        FindPath(new Vector2(20, 5));
    }

    void Update()
    {
        // Bỏ đoạn code cho phím Space nếu không cần thiết
    }

    void CreateGrid()
    {
        aStarNodes = new AStarNodecs[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                aStarNodes[x, y] = new AStarNodecs(new Vector2Int(x, y));
                Vector3 worldPosition = ConvertGridPositionToWorldPosition(aStarNodes[x, y]);

                Collider2D[] hitColliders2D = Physics2D.OverlapBoxAll(worldPosition, new Vector2(cellSize, cellSize), 0f);
                foreach (Collider2D hitCollider2D in hitColliders2D)
                {
                    if (hitCollider2D.gameObject.CompareTag("AI") || hitCollider2D.gameObject.CompareTag("Player"))
                    {
                        continue;
                    }
                    if (!hitCollider2D.isTrigger)
                    {
                        aStarNodes[x, y].isObstaacle = true;
                        break;
                    }
                }
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                AddNeighbours(aStarNodes[x, y], x, y);
            }
        }
    }

    void AddNeighbours(AStarNodecs node, int x, int y)
    {
        int[,] directions = {
            { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 }, 
            { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } 
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = x + directions[i, 0];
            int newY = y + directions[i, 1];

            if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
            {
                if (!aStarNodes[newX, newY].isObstaacle)
                {
                    node.neighbours.Add(aStarNodes[newX, newY]);
                }
            }
        }
    }

    private void Reset()
    {
        nodesToCheck.Clear();
        nodesChecked.Clear();
        aiPath.Clear();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                aStarNodes[x, y].Reset();
            }
        }
    }

    public List<Vector2> FindPath(Vector2 destination)
    {
        if (aStarNodes == null)
            return null;

        Reset();
        Vector2Int destinationGridPoint = ConvertWorldToGridPoint(destination);
        Vector2Int currentPositionGridPoint = ConvertWorldToGridPoint(transform.position);

        destinationPositionDebug = destination;
        startNode = GetNodeFromPoint(currentPositionGridPoint);

        startPositionDebug = ConvertGridPositionToWorldPosition(startNode);
        AStarNodecs currentNode = startNode;
        bool isDoneFindingPath = false;
        int pickedOrder = 1;

        while (!isDoneFindingPath)
        {
            nodesChecked.Remove(currentNode);
            currentNode.pickedOrder = pickedOrder;
            pickedOrder++;
            nodesChecked.Add(currentNode);

            if (currentNode.gridPosition == destinationGridPoint)
            {
                isDoneFindingPath = true;
                break;
            }

            CalculateCostsForNodeAndNeighbours(currentNode, currentPositionGridPoint, destinationGridPoint);

            foreach (AStarNodecs neighbourNode in currentNode.neighbours)
            {
                if (nodesChecked.Contains(neighbourNode))
                    continue;
                if (nodesToCheck.Contains(neighbourNode))
                    continue;
                nodesToCheck.Add(neighbourNode);
            }

            if (nodesToCheck.Count == 0)
            {
                return null;
            }
            else
            {
                currentNode = nodesToCheck.OrderBy(n => n.fCostTotal).ThenBy(n => n.hCostDistanceFromGoal).First();
                nodesToCheck.Remove(currentNode);
            }
        }

        aiPath = CreatePathForAi(currentPositionGridPoint);
        return aiPath;
    }

    List<Vector2> CreatePathForAi(Vector2Int currentPositionGridPoint)
    {
        List<Vector2> resultAIPath = new List<Vector2>();
        List<AStarNodecs> aiPath = new List<AStarNodecs>();

        nodesChecked.Reverse();
        AStarNodecs currentNode = nodesChecked[0];
        aiPath.Add(currentNode);
        bool isPathCreated = false;
        int attempts = 0;

        while (!isPathCreated)
        {
            currentNode.neighbours = currentNode.neighbours.OrderBy(x => x.pickedOrder).ToList();
            foreach (AStarNodecs aStarNode in currentNode.neighbours)
            {
                if (!aiPath.Contains(aStarNode) && nodesChecked.Contains(aStarNode))
                {
                    aiPath.Add(aStarNode);
                    currentNode = aStarNode;
                    break;
                }
            }

            if (currentNode == startNode)
                isPathCreated = true;

            if (attempts > 1000)
            {
                break;
            }
            attempts++;
        }

        foreach (AStarNodecs aStarNode in aiPath)
        {
            resultAIPath.Add(ConvertGridPositionToWorldPosition(aStarNode));
        }

        resultAIPath.Reverse();
        return resultAIPath;
    }

    void CalculateCostsForNodeAndNeighbours(AStarNodecs aStarNode, Vector2Int aiPosition, Vector2Int aiDestination)
    {
        aStarNode.CalculateCostsForNode(aiPosition, aiDestination);

        foreach (AStarNodecs neighbourNode in aStarNode.neighbours)
        {
            if (Mathf.Abs(neighbourNode.gridPosition.x - aStarNode.gridPosition.x) == 1 &&
                Mathf.Abs(neighbourNode.gridPosition.y - aStarNode.gridPosition.y) == 1)
            {
                neighbourNode.gCostDistanceFromStart = aStarNode.gCostDistanceFromStart + 1.4f; // Đường chéo
            }
            else
            {
                neighbourNode.gCostDistanceFromStart = aStarNode.gCostDistanceFromStart + 1; // Đường thẳng
            }

            neighbourNode.CalculateCostsForNode(aiPosition, aiDestination);
        }

        nodesToCheck = nodesToCheck.OrderBy(x => x.fCostTotal).ThenBy(x => x.hCostDistanceFromGoal).ToList();
    }

    AStarNodecs GetNodeFromPoint(Vector2Int gridPoint)
    {
        if (gridPoint.x < 0 || gridPoint.x >= gridSizeX || gridPoint.y < 0 || gridPoint.y >= gridSizeY)
            return null;

        return aStarNodes[gridPoint.x, gridPoint.y];
    }

    Vector2Int ConvertWorldToGridPoint(Vector2 position)
    {
        Vector2Int gridPoint = new Vector2Int(Mathf.RoundToInt(position.x / cellSize + gridSizeX / 2.0f), Mathf.RoundToInt(position.y / cellSize + gridSizeY / 2.0f));
        return gridPoint;
    }

    Vector3 ConvertGridPositionToWorldPosition(AStarNodecs aStarNode)
    {
        return new Vector3(aStarNode.gridPosition.x * cellSize - (gridSizeX * cellSize) / 2.0f + cellSize / 2.0f,
                              aStarNode.gridPosition.y * cellSize - (gridSizeY * cellSize) / 2.0f + cellSize / 2.0f,
                              0);
    }
    void OnDrawGizmos()
    {
        if (aStarNodes == null)
            return;
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                if (aStarNodes[x, y].isObstaacle)

                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.green;

                Gizmos.DrawWireCube(ConvertGridPositionToWorldPosition(aStarNodes[x, y]), new Vector3(cellSize, cellSize, cellSize));

            }

        foreach (AStarNodecs checkedNode in nodesChecked)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(checkedNode), 1.0f);
        }
        foreach (AStarNodecs tocheckedNode in nodesChecked)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(tocheckedNode), 1.0f);
        }
        Vector3 lastAIPoint = Vector3.zero;
        bool isFirstStep = true;

        Gizmos.color = Color.black;
        foreach (Vector2 point in aiPath)
        {
            Vector3 currentPoint = new Vector3(point.x, point.y, 0);

            if (isFirstStep)
            {
                isFirstStep = false;
            }
            else
            {
                Gizmos.DrawLine(lastAIPoint, currentPoint);
            }

            lastAIPoint = currentPoint;
        }
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(startPositionDebug, 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(destinationPositionDebug, 1f);
    }

}
