using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesappearEffect : MonoBehaviour
{

    private void Awake()
    {
        Destroy(gameObject, 0.6f);
    }
}
