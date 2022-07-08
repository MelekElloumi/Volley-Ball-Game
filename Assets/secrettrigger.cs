using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secrettrigger : MonoBehaviour
{
    public GameObject agent;
    private void OnTriggerEnter(Collider other)
    {
        agent.SetActive(false);
    }
    
}
