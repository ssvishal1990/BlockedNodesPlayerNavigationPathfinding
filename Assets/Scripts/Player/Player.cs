using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected Pathfinding pathfindingBrain;

    protected const float yheight = 1.0f;// Overall y coordinate of our player object should be constant right now
    // Start is called before the first frame update

    PlayerMove playerMove;
    
    void Start()
    {
        Debug.Log("Initializing Path Finding");
        initialization();
        playerMove.initEndPoints();
    }

    protected virtual void initialization()
    {
        pathfindingBrain = new Pathfinding();
        playerMove = GetComponent<PlayerMove>();
    }
}
