using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private Transform gridSystemVisualPrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    private int gridHeight;
    private int gridWidth;

    private bool gridVisible = false;


    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material pathMaterial;
    [SerializeField] private Material BlockedNodeMaterial;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 unit action system  " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

        GameObject GridSystemVisualParent = GameObject.Find("GridSystemVisualParent");
        if (GridSystemVisualParent == null)
        {
            GridSystemVisualParent = new GameObject("GridSystemVisualParent");
        }
        gridHeight = LevelGrid.Instance.getHeight();
        gridWidth = LevelGrid.Instance.getWidth();
        gridSystemVisualSingles = new GridSystemVisualSingle[
            gridWidth,
            gridHeight
            ];
        //Debug.Log("Grid height --> " + LevelGrid.Instance.getHeight() + "  Grid Width --> " + LevelGrid.Instance.getWidth());
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {

                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform =
                Instantiate(gridSystemVisualPrefab, LevelGrid.Instance.getWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleTransform.SetParent(GridSystemVisualParent.transform);
                gridSystemVisualSingles[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();

            }
        }
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                gridSystemVisualSingles[x, z].Hide();
            }
        }
        gridVisible = false;
    }

    public void showGridPositionList(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            Vector3 gridLocation = LevelGrid.Instance.getWorldPosition(gridPosition);
            gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show();
        }
        gridVisible = true;
    }



    private void updateGridVisual()
    {
        HideAllGridPosition();

        showGridPositionList(LevelGrid.Instance.GetAllGridPositions());
    }

    public void UpdateBlockedNodeVisual(GridPosition gridPosition)
    {
        Transform gridSystemVisualSingle =  gridSystemVisualSingles[gridPosition.x, gridPosition.z].transform;
        foreach (Transform child in gridSystemVisualSingle)
        {
            if (child.name.Equals("Quad"))
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                meshRenderer.material = BlockedNodeMaterial;
            }
        }
    }
    public void updatePathMaterialOnSearchingOrClearing(List<GridObject> path, bool newPath)
    {
        if (path == null)
        {
            Debug.Log("Obtained Invalid path here");
            return;
        }

        foreach (GridObject obj in path)
        {
            if (!obj.getIsWalkable())
            {
                continue;
            } 
            GridPosition gridPosition = obj.gridPosition;
            //Debug.Log($"Grid system visual for position object :: {gridPosition}");
            foreach (Transform child in gridSystemVisualSingles[gridPosition.x, gridPosition.z].transform)
            {
                if (child.name.Equals("Quad"))
                {
                    MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                    if (newPath)
                    {
                        meshRenderer.material = pathMaterial;
                    }
                    else
                    {
                        meshRenderer.material = defaultMaterial;
                    }
                    
                }
            }
        }
    }

}
