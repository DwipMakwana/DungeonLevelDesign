using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PressType
{
    Once,
    Limitless
}

public class OverlapObjects : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjects;
    [SerializeField]
    bool overlapping;
    [SerializeField]
    bool pressedF;

    [SerializeField]
    UnityEvent onPressF;

    [SerializeField]
    PressType pressType;

    private void OnTriggerEnter(Collider other)
    {
        if (pressedF) return;

        if (other.tag == "Player")
        {
            overlapping = true;

            foreach (var obj in gameObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            overlapping = false;
            
            foreach (var obj in gameObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (overlapping && Input.GetKeyDown(KeyCode.F))
        {
            if(pressType == PressType.Once)
                pressedF = true;

            foreach (var obj in gameObjects)
            {
                obj.SetActive(false);
            }
            onPressF.Invoke();
        }
    }
}
