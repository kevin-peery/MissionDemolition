using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rubberband : MonoBehaviour
{
    [Header("Inscribed")]
    public Color color;
    public Vector3 startPoint,
                   endPoint;

    [Header("Dynamic")]
    private LineRenderer _line;
    private Slingshot slingshot;
    private float projRadius;

    void Start()
    {
        _line = GetComponent<LineRenderer>();

        DrawRest();

        slingshot = GetComponentInParent<Slingshot>();
    }

    public void DrawRest()
    {
        _line.positionCount = 2;
        _line.SetPosition(0, startPoint);
        _line.SetPosition(1, endPoint);
        _line.startColor = _line.endColor = color;
    }

    public void DrawPull(Vector3 point)
    {
        _line.positionCount = 4;
        _line.SetPosition(0, startPoint);
        
        _line.useWorldSpace = true;
        point.z = startPoint.z;
        _line.SetPosition(1, point);

        point.z = endPoint.z;
        _line.SetPosition(2, point);

        _line.useWorldSpace = false;
        _line.SetPosition(3, endPoint);
        _line.startColor = _line.endColor = color;
    }
}
