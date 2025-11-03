using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;  // Time before "disappearing"
    [SerializeField] private float respawnDelay = 30f; // Time before respawn
    private Coroutine destroyCoroutine;
    public Animator anim;
    AudioSource m_MyAudioSource;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        anim = GetComponent<Animator>();
        m_MyAudioSource = GetComponent<AudioSource>();

        // Save initial transform to respawn at the same position
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            destroyCoroutine = StartCoroutine(DisappearAndRespawn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (destroyCoroutine != null)
            {
                StopCoroutine(destroyCoroutine);
                destroyCoroutine = null;
                anim.SetBool("isRumbling", false);
                m_MyAudioSource.Stop();
            }
        }
    }

    private IEnumerator DisappearAndRespawn()
    {
        anim.SetBool("isRumbling", true);
        m_MyAudioSource.Play();

        // Wait before "disappearing"
        yield return new WaitForSeconds(destroyDelay);

        // Hide the object
        gameObject.SetActive(false);

        // Wait before respawning
        yield return new WaitForSeconds(respawnDelay);

        // Reset position/rotation (optional)
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Show object again
        gameObject.SetActive(true);

        // Reset animator/audio states
        anim.SetBool("isRumbling", false);
        m_MyAudioSource.Stop();
    }
}