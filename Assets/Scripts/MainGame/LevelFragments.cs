using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFragments : MonoBehaviour
{
    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 5f;
    public List<GameObject> levelFragments;
    List<GameObject> pickUps;
    List<bool> pickUpPresent;
    public GameObject pickUpPrefab;
    GameObject currentGameObject;
    Vector3 lastEndPosition;
    public Transform playerPosition;
    void Start()
    {
        if (pickUps==null)
        {
            pickUps = new List<GameObject>();
            pickUpPresent = new List<bool>();
        }
        else
        {
            Debug.Log("Pickups present");
        }
        for(int i=0; i<levelFragments.Count; i++)
        {
            if(levelFragments[i].transform.GetChild(levelFragments[i].transform.childCount - 1).gameObject.name == "Spawner")
            {
                pickUps.Add(Instantiate(pickUpPrefab, levelFragments[i].transform.GetChild(levelFragments[i].transform.childCount - 1).transform.position, Quaternion.identity));
                pickUpPresent.Add(true);
            }
        }
    }


    public void RemovePickUp(GameObject pickUp)
    {
        for(int i=0; i<pickUps.Count; i++)
        {
            if (pickUps[i] != null)
            {
                if(pickUps[i].transform == pickUp.transform)
                {
                    Destroy(pickUp);
                    pickUpPresent[i] = false;
                }
            }
        }
    }

    void Update()
    {
        lastEndPosition = levelFragments[levelFragments.Count - 1].transform.position;
        if(Mathf.Abs((playerPosition.position - lastEndPosition).y) < PLAYER_DISTANCE_SPAWN_LEVEL_PART)
        {
            levelFragments[0].transform.position = lastEndPosition + Vector3.down * 10f;


            if (pickUpPresent[0] == false)
            {
                if(levelFragments[0].transform.GetChild(levelFragments[0].transform.childCount - 1).gameObject.name == "Spawner")
                {
                    pickUps.RemoveAt(0);
                    pickUps.Add(Instantiate(pickUpPrefab, levelFragments[0].transform.GetChild(levelFragments[0].transform.childCount - 1).transform.position, Quaternion.identity));
                    pickUpPresent.RemoveAt(0);
                    pickUpPresent.Add(true);
                }

            }
            else
            {
                if(levelFragments[0].transform.GetChild(levelFragments[0].transform.childCount - 1).gameObject.name == "Spawner")
                {
                    pickUps[0].transform.position = levelFragments[0].transform.GetChild(levelFragments[0].transform.childCount - 1).transform.position;
                    GameObject tempPickUp = pickUps[0];
                    pickUps.RemoveAt(0);
                    pickUps.Add(tempPickUp);
                    pickUpPresent.RemoveAt(0);
                    pickUpPresent.Add(true);
                }
            }


            GameObject temp = levelFragments[0];
            levelFragments.RemoveAt(0); // Remove from beginning
            levelFragments.Add(temp); // Add at the end
        }
    }
}
