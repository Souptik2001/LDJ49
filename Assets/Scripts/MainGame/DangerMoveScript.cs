using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerMoveScript : MonoBehaviour
{
    public Transform player;
    public Transform tip;
    float speed;
    float closenessFactor;
    public bool multiplayer;
    void Start()
    {
        speed = 0.35f;
        closenessFactor = 5;
        //if (!multiplayer) speed = 0.41f;
        if (!multiplayer) speed = 0.2f;
        if (multiplayer) Invoke("IncreaseSpeed", 10f);
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime * Mathf.Pow(Vector3.Magnitude(tip.position - player.position + new Vector3(0, closenessFactor, 0)), 1.6f) * speed;
        // transform.position = Vector3.Lerp(transform.position, player.position, 0.1f * Time.deltaTime);
    }

    void IncreaseSpeed()
    {
        speed += 0.0001f;
        closenessFactor += 0.005f;
        Invoke("IncreaseSpeed", 10f);
    }


}
