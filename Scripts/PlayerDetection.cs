using System;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public event Action<Transform> OnDetection = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var player = other.gameObject;
            Debug.Log("Aggrod");
            OnDetection(player.transform);
        }
    }
}
