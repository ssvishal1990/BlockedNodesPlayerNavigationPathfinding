using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManageSystem : MonoBehaviour
{
    [SerializeField] GameObject playerGameObject;

    //Right now the overall idea is to , from start go to end and came back
    // Need to make it so, that 2 players on game path do not collide

    List<GameObject> playersGameObjects;
    // Start is called before the first frame update


    // this should be the constant y value throught out the game
    const float heightValue = 1.0f;


    GridObject startPoint;
    Vector3 startPosition;

    GridObject endPoint;
    Vector3 endPosition;

    
    void Start()
    {
        startPosition = new Vector3(0, heightValue, 0);
        startPoint = LevelGrid.Instance.GetGridObject(startPosition);

        playersGameObjects = new List<GameObject>();
        playersGameObjects.Add(playerGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEndPointAndStartNavigationOnLeftClick();
    }

    private void UpdateEndPointAndStartNavigationOnLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Input Detected");
            Vector3 mousePosition = MouseWorld.GetPosition();
            GridPosition mouseDownPosition = LevelGrid.Instance.getGridPosition(mousePosition);
            if (!LevelGrid.Instance.isValidGridPosition(mouseDownPosition))
            {
                return;
            }
            endPoint = LevelGrid.Instance.GetGridObject(mouseDownPosition);
            endPosition = LevelGrid.Instance.getWorldPosition(mouseDownPosition);

            UpdatePlayerThatTheyCanMove();
        }
    }

    // Need to make a building manager
    // Building Manager will contain it's specific end point and
    // List of players, those players for that building should follow that path only 
    private void UpdatePlayerThatTheyCanMove()
    {
        foreach (GameObject player in playersGameObjects)
        {
            Debug.Log($"Setting end Points for player -> {player.name}");
            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            playerMove.setEndPoints(startPoint, endPoint);
            playerMove.setMoveStatus(true);
        }
    }
}
