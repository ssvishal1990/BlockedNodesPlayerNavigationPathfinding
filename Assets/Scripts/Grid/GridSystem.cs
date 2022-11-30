using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    /// <summary>
    ///  This class will be responsible for 
    /// 1) Creating and managing grid 
    /// 2) converting types from world type to gridPosition and will provide other utilities
    /// 3) Using this class we can make different gridSystems as per level requirement.
    /// </summary>
    // Start is called before the first frame update

    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;


    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }
    
    public Vector3 GetWorldPositionWithConstY(GridPosition gridPosition, float constY)
    {
        return new Vector3(gridPosition.x, constY, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
            );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        GameObject debugObjectParent = GameObject.Find("DebugGameObjectsParent");
        if (debugObjectParent == null)
        {
            debugObjectParent = new GameObject("DebugGameObjectsParent");
        }
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                //Debug.Log("Inside create debug objects");
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                debugTransform.parent = debugObjectParent.transform;
                debugTransform.gameObject.name = "Debug_Object__" + GetGridObject(gridPosition).getGridObjectPosition();
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }


        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }


    public GridObject GetGridObject(int x, int y)
    {
        //Debug.Log($"Mouse int obtained x: {x}, y: {y}");
        return gridObjectArray[x, y];
    }


    public bool isValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < height;
    }

    public int getHeight()
    {
        return height;
    }

    public int getWidhth()
    {
        return width;
    }

    public List<GridPosition> getAllGridPositions()
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridPositions.Add(gridObjectArray[x, z].gridPosition);
            }
        }
        return gridPositions;
    }

    public GridObject[,] getGrid()
    {
        return gridObjectArray;
    }

    public void BlockGridObject(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridObject(gridPosition);
        gridObject.setNodeIsWalkable(false);
    }


}
