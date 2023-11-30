using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, ICpListener
{
    public void OnNotify(Transform notifier)
    {
        gameObject.SetActive(true);
    }
}
