using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will be used to test pathfinding
/// </summary>
public class TestPathFInding : MonoBehaviour
{
    [SerializeField] Vector2Int start;
    [SerializeField] Vector2Int end;
    [SerializeField] List<GridObject> path;
    Pathfinding pathfinding;


    [SerializeField] int testType =1 ;


    //public event EventHandler<List<GridObject>> AfterFindingThePathVisualUpdate;
    public event EventHandler<List<GridObject>> BeforeFindingThePathVisualUpdate;
    // Start is called before the first frame update
    void Start()
    {
        start = Vector2Int.zero;
        path = new List<GridObject>();
        pathfinding = new Pathfinding();
        testType = 1;
    }

    // Update is called once per frame
    void Update()
    {
        TestForPathFInding();
        TestForPathFinding();
        TestByBlockingNodes();
    }

    private void TestByBlockingNodes()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector3 mousePosition = MouseWorld.GetPosition();
            GridPosition mouseDownPosition = LevelGrid.Instance.getGridPosition(mousePosition);
            if (!LevelGrid.Instance.isValidGridPosition(mouseDownPosition))
            {
                return;
            }
            GridObject mousePositionGridObject = LevelGrid.Instance.GetGridObject(mouseDownPosition);
            LevelGrid.Instance.BlockGridObject(mouseDownPosition);
            GridSystemVisual.Instance.UpdateBlockedNodeVisual(mouseDownPosition);

            if (path != null || path.Count != 0)
            {
                //check if the blocked node exists in the path
                
                if (path.Contains(mousePositionGridObject))
                {
                    // If it exists calculate a new path
                    CalculateNewPath();
                }
            }
        }
    }

    private void TestForPathFInding()
    {
        if (Input.GetKeyDown(KeyCode.T))
            CalculateNewPath();
    }

    public void TestForPathFinding()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = MouseWorld.GetPosition();
            GridPosition mouseDownPosition = LevelGrid.Instance.getGridPosition(mousePosition);
            if (!LevelGrid.Instance.isValidGridPosition(mouseDownPosition))
            {
                return;
            }
            //Debug.Log("Mouse world position on button 1" + mouseDownPosition);
            start.x = mouseDownPosition.x;
            start.y = mouseDownPosition.z;
            //CalculateNewPath();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePosition = MouseWorld.GetPosition();
            GridPosition mouseDownPosition = LevelGrid.Instance.getGridPosition(mousePosition);
            if (!LevelGrid.Instance.isValidGridPosition(mouseDownPosition))
            {
                return;
            }
            //Debug.Log("Mouse world position on button 2" + mouseDownPosition);
            end.x = mouseDownPosition.x;
            end.y = mouseDownPosition.z;
            CalculateNewPath();
        }
    }

    private void CalculateNewPath()
    {
        if (path != null)
        {
            if (path.Count != 0)
            {
                //Debug.Log(path);
                BeforeFindingThePathVisualUpdate?.Invoke(this, path);
                GridSystemVisual.Instance.updatePathMaterialOnSearchingOrClearing(path, false);
                path.Clear();
            }
            
        }
        path = pathfinding.FindPath(start.x, start.y, end.x, end.y);
        GridSystemVisual.Instance.updatePathMaterialOnSearchingOrClearing(path, true);
        //if (path != null)
        //{
        //    for (int i = 0; i < path.Count - 1; i++)
        //    {
        //        Debug.Log("Calling Debug . draw Line");
        //        Debug.DrawLine(LevelGrid.Instance.getWorldPosition(path[i].gridPosition),
        //                       LevelGrid.Instance.getWorldPosition(path[i + 1].gridPosition),
        //                       Color.red);
        //    }
        //}
    }
}
