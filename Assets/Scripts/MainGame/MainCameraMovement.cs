using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    public Transform player;
    public float smoothness;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetPos = player.position - new Vector3(0, 0, 1) * 20f;
        targetPos = new Vector3(0, player.position.y, player.position.z - 20f);
        //transform.position = Vector3.Lerp(transform.position, targetPos, smoothness);
        transform.position = transform.position + (targetPos - transform.position);
    }
}
