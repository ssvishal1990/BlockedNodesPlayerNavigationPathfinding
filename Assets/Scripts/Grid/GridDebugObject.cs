using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] TextMeshPro GridPosition;
    [SerializeField] TextMeshPro G;
    [SerializeField] TextMeshPro H;
    [SerializeField] TextMeshPro F;

    private void Start()
    {
        updateGridText();
    }

    private void Update()
    {
        updateGridText();
    }
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void updateGridText()
    {
        GridPosition.text = gridObject.getGridObjectPosition();
        F.text = gridObject.gridPosition.fcost.ToString();
        G.text = gridObject.gridPosition.gcost.ToString();
        H.text = gridObject.gridPosition.hcost.ToString();
    }

}
