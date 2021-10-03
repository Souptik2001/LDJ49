using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Boundary : MonoBehaviour
{
    public Camera cam;
    float width;
    float height;
    Rigidbody2D rb;
    EdgeCollider2D edge;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        edge = GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        transform.position = cam.transform.position;
        FindBoundaries();
        SetBounds();
    }


    void SetBounds()
    {
        Vector2 pointa = new Vector2(width / 2, height / 2);
        Vector2 pointb = new Vector2(width / 2, -height / 2);
        Vector2 pointc = new Vector2(-width / 2, -height / 2);
        Vector2 pointd = new Vector2(-width / 2, height / 2);
        Vector2[] tempArray = new Vector2[]{pointa, pointb, pointc, pointd, pointa};
        edge.points = tempArray;
    }

    void FindBoundaries()
    {
        width = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        // height = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);
        height = 10f;
    }

}
