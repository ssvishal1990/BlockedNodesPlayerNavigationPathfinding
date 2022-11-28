using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding 
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private const int MAX_MOVE_DISTANCE = 500;

    private GridObject[,] grid;
    private int gridWidth;
    private int gridHeight;

    private List<GridObject> openList;
    private List<GridObject> closedList;
    private List<GridObject> path;



    public Pathfinding()
    {
        grid = LevelGrid.Instance.getGrid();
        gridWidth = LevelGrid.Instance.getWidth();
        gridHeight = LevelGrid.Instance.getHeight();
        path = new List<GridObject>();
    }

    public List<GridObject> FindPath(int startX, int startY, int endX, int endY)
    {
        //Graph Setup
        GridObject startNode = LevelGrid.Instance.GetGridObject(startX, startY);
        GridObject endNode = LevelGrid.Instance.GetGridObject(endX, endY);

        openList = new List<GridObject>() { startNode };
        closedList = new List<GridObject>();

        //Debug.Log($"Inside Find path Current Grid Props  H : {gridHeight} ,W :{gridWidth}");
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GridObject pathNodeGridObject = LevelGrid.Instance.GetGridObject(x, y);
                GridPosition pathNode = pathNodeGridObject.gridPosition ;
                //Debug.Log($"Setting up grid position : {pathNode}");
                pathNode.gcost = MAX_MOVE_DISTANCE;
                pathNode.calculateFcost();
                pathNodeGridObject.cameFromGridPosition = null;
            }
        }


        startNode.gridPosition.gcost = 0;
        startNode.gridPosition.hcost = CalculateDistanceCost(startNode.gridPosition, endNode.gridPosition);
        startNode.gridPosition.calculateFcost();

        bool debugBreaker = false;
        //Cycling Through Graph
        while (openList.Count > 0 && !debugBreaker)
        {
            GridObject currentNode = getLowestFCostPathNode(openList);
            if (currentNode == endNode)
            {
                // Reached the destination
                return CalculatePath(endNode, startNode);
            }

            

            openList.Remove(currentNode);

            if (closedList.Contains(currentNode))
            {
                continue;
            }
            closedList.Add(currentNode);

            foreach (GridObject neighbourNode in getNeighboursList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (neighbourNode.gridPosition.x == endNode.gridPosition.x && neighbourNode.gridPosition.z == endNode.gridPosition.z)
                {
                    int tentativeGCost2 = currentNode.gridPosition.gcost + CalculateDistanceCost(currentNode.gridPosition, neighbourNode.gridPosition);
                    //Debug.Log($"tentative G cost of current and it's neighbour end node :: {tentativeGCost2}  , {neighbourNode.gridPosition.gcost} ");
                }
                if (!openList.Contains(neighbourNode))
                {
                    openList.Add(neighbourNode);
                }
                

                int tentativeGCost = currentNode.gridPosition.gcost + CalculateDistanceCost(currentNode.gridPosition, neighbourNode.gridPosition);
                if (tentativeGCost < neighbourNode.gridPosition.gcost)
                {
                    neighbourNode.cameFromGridObject = currentNode;
                    neighbourNode.gridPosition.gcost = tentativeGCost;
                    neighbourNode.gridPosition.hcost = CalculateDistanceCost(neighbourNode.gridPosition, endNode.gridPosition);
                    neighbourNode.gridPosition.calculateFcost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }


        //Out of nodes in open list
        return null;

    }

    private List<GridObject> getNeighboursList(GridObject currentNode)
    {
        List<GridObject> neighbourList = new List<GridObject> ();
        GridObject potentialNeighbhour;
        GridPosition currentPosition = currentNode.gridPosition;

        if (currentPosition.x - 1 >= 0)
        {
            // We can go left
            //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(-1, 0)));
            potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(-1, 0));
            neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);

            if (currentPosition.z + 1 < gridHeight)
            {
                // left up
                //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(-1, 1)));
                potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(-1, 1));
                neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);
            }

            if (currentPosition.z - 1 >= 0)
            {
                // left down
                //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(-1, -1)));
                potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(-1, -1));
                neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);
            }
        }

        if (currentPosition.x + 1 < gridWidth)
        {
            // We can go Right
            //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(1, 0)));
            potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(1, 0));
            neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);

            if (currentPosition.z + 1 < gridHeight)
            {
                // Right up
                //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(1, 1)));
                potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(1, 1));
                neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);
            }
            
            if (currentPosition.z - 1 >= 0)
            {
                // Right down
                //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(1, -1)));
                potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(1, -1));
                neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);
            }
        }

        if (currentPosition.z + 1 < gridHeight)
        {
            // Up
            //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(0, 1)));
            potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(0, 1));
            neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);
        }
        
        if (currentPosition.z - 1 >= 0)
        {
            // Down
            //neighbourList.Add(LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(0, -1)));
            potentialNeighbhour = LevelGrid.Instance.GetGridObject(currentPosition + new Vector2Int(0, -1));
            neighbourList = AddNeighbourIfNeighbhourNodeIsValid(neighbourList, potentialNeighbhour);
        }

        return neighbourList;
    }

    private List<GridObject> AddNeighbourIfNeighbhourNodeIsValid(List<GridObject> neighbourList, GridObject potentialNeighbhour)
    {
        if (potentialNeighbhour.getIsWalkable())
        {
            neighbourList.Add(potentialNeighbhour);
            return neighbourList;
        }
        //Debug.Log($"Node was invalid {potentialNeighbhour.getIsWalkable()} ");
        return neighbourList;
    }

    private List<GridObject> CalculatePath(GridObject endNode, GridObject startNode)
    {
        path.Clear();
        path.Add(endNode);

        GridObject currentNode = endNode;
        //Debug.Log($"Node : {currentNode} is connected to {currentNode.cameFromGridObject}");
        while (currentNode.cameFromGridObject != null)
        {
            if (path.Count > 100)
            {
                Debug.Log("Some size issue here");
                path.Clear();
                return path;
            }
            Debug.Log($"Node : {currentNode} is connected to {currentNode.cameFromGridObject}");
            path.Add(currentNode.cameFromGridObject);

            if (currentNode.cameFromGridObject == startNode)
            {
                break;
            }
            currentNode = currentNode.cameFromGridObject;
        }
        path.Reverse();
        return path;
    }


    private bool checkIfNodeIsValid(GridObject gridObject)
    {
        return gridObject.getIsWalkable();
    }


    private int CalculateDistanceCost(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        int xDistance = Mathf.Abs(startGridPosition.x - endGridPosition.x);
        int yDistance = Mathf.Abs(startGridPosition.z - endGridPosition.z);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private GridObject getLowestFCostPathNode(List<GridObject> nodesList)
    {
        GridObject lowestFCostPathNode = nodesList[0];
        for (int i = 0; i < nodesList.Count; i++)
        {
            if (nodesList[i].gridPosition.fcost < lowestFCostPathNode.gridPosition.fcost)
            {
                lowestFCostPathNode = nodesList[i];
            }
        }
        return lowestFCostPathNode;
    }

    
}
