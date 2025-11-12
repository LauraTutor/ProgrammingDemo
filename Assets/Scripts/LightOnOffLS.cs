using UnityEngine;

public class LightOnOffLS : MonoBehaviour
{
    public Light targetLight;
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (targetLight != null)
            {
                targetLight.enabled = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (targetLight != null)
            {
                targetLight.enabled = true;
            }
        }
    }
}