using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;
    private Coroutine destroyCoroutine;
    public Animator anim;
    AudioSource m_MyAudioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start destroy timer
            destroyCoroutine = StartCoroutine(DestroyAfterDelay());
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

    private IEnumerator DestroyAfterDelay()
    {
        anim.SetBool("isRumbling", true);
        m_MyAudioSource.Play();
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}