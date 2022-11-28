using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld Instance;
    [SerializeField] private LayerMask mouseLayerMask;

    private void Awake()
    {
        Instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.mouseLayerMask);
        return raycastHit.point;
    }

    public static RaycastHit GetObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.mouseLayerMask);
        return raycastHit;
    }
}
