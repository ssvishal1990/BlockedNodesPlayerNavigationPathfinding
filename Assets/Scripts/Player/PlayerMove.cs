using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Player
{
    [SerializeField] float speed = 2f;


    private GridObject startPoint;
    private GridObject endPoint;
    

    private bool readyToFindPath = false;
    private bool playerOnTheWay = false;


    // Start is called before the first frame update

    // Update is called once per frame

    protected override void initialization()
    {
        base.initialization();
    }

    void Update()
    {
        CheckAndMove();
        DebugLines();
    }

    internal void initEndPoints()
    {
        startPoint = null;
        endPoint = null;
    }

    private void CheckAndMove()
    {
        if (readyToFindPath & !playerOnTheWay)
        {
            //Find a path
            // Follow that path
            List<GridObject> path = pathfindingBrain.FindPath(startPoint, endPoint);
            PlotPath(path);
            StartCoroutine(FollowPath(path));
        }
    }

    private void PlotPath(List<GridObject> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 tempStartCoordinate = LevelGrid.Instance.getWorldPosition(path[i].gridPosition);
            Vector3 tempEndCoordinate = LevelGrid.Instance.getWorldPosition(path[i + 1].gridPosition);

            Debug.DrawLine(tempStartCoordinate, tempEndCoordinate, Color.red, Mathf.Infinity, false);
        }
    }

    public void setMoveStatus(bool readyToFindPath)
    {
        this.readyToFindPath = readyToFindPath;
    }
    /// <summary>
    /// Intialize start and end Point
    /// </summary>
    /// <param name="startPoint">player start point</param>
    /// <param name="endPoint">player end point</param>
    public void setEndPoints(GridObject startPoint, GridObject endPoint)
    {
        // If startPoint Has not been initialized;
        if (this.startPoint == null)
        {
            Debug.Log("First time initializing coordinates");
            this.startPoint = startPoint;
        }
        this.endPoint = endPoint;
    }

    IEnumerator FollowPath(List<GridObject> path)
    {
        playerOnTheWay = true;
        Vector3 startPosition = LevelGrid.Instance.getWorldPositionWithConstY(path[0].gridPosition, yheight);
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 tempEndPosition = LevelGrid.Instance.getWorldPositionWithConstY(path[i].gridPosition, yheight);
            transform.LookAt(tempEndPosition);
            float travelPercent = 0f;
            while (travelPercent < 1)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, tempEndPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
            startPosition = tempEndPosition;
        }
        EndOfThePathActions();
    }

    private void EndOfThePathActions()
    {
        startPoint = endPoint;
        playerOnTheWay = false;
        readyToFindPath = false;
    }

    private void DebugLines()
    {
        Vector3 downLeft = new Vector3(0, 2, 0);
        Vector3 UpLeft = new Vector3(0, 2, 10);
        Vector3 UpRight = new Vector3(10 , 2, 10);
        Vector3 DownRight = new Vector3(0 , 2, 10);

        Debug.DrawLine(downLeft, UpLeft, Color.red, Mathf.Infinity, false);
        Debug.DrawLine(UpLeft, UpRight, Color.red, Mathf.Infinity, false);
        Debug.DrawLine(UpRight, DownRight, Color.black, Mathf.Infinity, false);
        Debug.DrawLine(DownRight, downLeft, Color.black, Mathf.Infinity, false);
    }


}
