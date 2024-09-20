using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCtrl : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector2 mousePos;
    private Vector2 startMousePos;

    private Transform[] points;

    void Awake()
    {
        this.LoadComponent();
    }

    private void LoadComponent()
    {
        this.lineRenderer = transform.GetComponent<LineRenderer>();
        this.lineRenderer.positionCount = 2;
    }

    public void SetUp (Transform[] p)
    {
        this.lineRenderer.positionCount = p.Length;
        this.points = p;
    }

    private void Update()
    {
        this.SetLinePosition();
    }

    private void SetLinePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            this.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.lineRenderer.SetPosition(0, new Vector3(startMousePos.x, startMousePos.y, 0f));
            this.lineRenderer.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));
        }
    }
}
