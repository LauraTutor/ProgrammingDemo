using UnityEngine;

public class ButtonDoorLS : MonoBehaviour
{
    public Animator anim;
    AudioSource m_MyAudioSource;
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("onKey", true);
        }
    }
}