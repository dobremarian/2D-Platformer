using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator checkpointAnim;
    Transform checkpointTr;
    AudioManager theAudioManager;

    CheckpointController theCpController;

    bool isActive = false;

    public bool IsActive
    {
        set { isActive = value; }
    }

    void Start()
    {
        checkpointAnim = GetComponent<Animator>();
        checkpointTr = GetComponent<Transform>();
        theCpController = GameObject.FindObjectOfType<CheckpointController>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    
    void Update()
    {
        if(isActive)
        {
            checkpointAnim.SetBool("Hit_B", true);
        }
        else
        {
            checkpointAnim.SetBool("Hit_B", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!isActive)
            {
                theAudioManager.PlaySFX(11);
                theCpController.ResetCheckpoints();
                theCpController.SpawnPoint = checkpointTr.position;
                isActive = true;
            }
        }
    }
}
