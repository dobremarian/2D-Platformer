using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject[] checkpoints;

    Vector2 spawnPoint;

    public Vector2 SpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }

    void Start()
    {
        if(startPoint != null)
        {
            spawnPoint = new Vector2(startPoint.transform.position.x + 0.107f, startPoint.transform.position.y);
        }
    }

    
    void Update()
    {
        
    }

    public void ResetCheckpoints()
    {
        for(int i = 0; i< checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<Checkpoint>().IsActive = false;
        }
    }

}
