using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Destroy THIS object after a delay
            StartCoroutine(DestroyAfterDelay(gameObject));
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(obj);
    }
}