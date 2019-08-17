using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceIndicator : MonoBehaviour
{
    public Vector2 StartPosition { set => vertices[0] = value; }
    public Vector2 TargetPosition { set => vertices[1] = value; }
    public Vector3 force;

    public LineRenderer lineRenderer;

    private Vector3[] vertices = new Vector3[5];

    [SerializeField] private float maxScale = 5;
    [Range(.001f, 1f)][SerializeField] private float scaleFactor = 0.1f;
    private float scale = 0f;

    private bool increasing = true;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Calculate();
    }

    public void Calculate()
    {
        transform.position = vertices[0];
        
        var direction = (vertices[1] - vertices[0]).normalized;
        
        if (increasing)
        {
            scale += scaleFactor;
            if (scale > maxScale)
            {
                increasing = false;
                scale = maxScale;
            }
        }
        else
        {
            scale -= scaleFactor;
            if (scale < 0)
            {
                increasing = true;
                scale = 0;
            }
        }

        vertices[3] = vertices[1] = vertices[0] + (force = direction * scale);
        vertices[2] = vertices[1] - Quaternion.AngleAxis(45, Vector3.forward) * direction;
        vertices[4] = vertices[1] - Quaternion.AngleAxis(45, -Vector3.forward) * direction;

        lineRenderer.SetPositions(vertices);
    }
}
