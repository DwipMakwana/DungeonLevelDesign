using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPlayerTrap : MonoBehaviour
{
    [SerializeField]
    UnityEvent overlapEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            overlapEvent.Invoke();
        }
    }
}
