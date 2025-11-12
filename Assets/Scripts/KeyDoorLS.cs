using UnityEngine;
using System.Collections;

public class KeyDoorLS : MonoBehaviour
{
    // Assign these in the Inspector
    public GameObject keyObject;
    public GameObject padlockObject;
    public Animator doorAnimator; // Animator component of the door

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player triggered it
        if (other.CompareTag("Player"))
        {
            // Destroy the key and padlock
            if (keyObject != null)
                Destroy(keyObject);

            if (padlockObject != null)
                Destroy(padlockObject);
                doorAnimator.SetBool("Open", true);
            
        }
    }
}