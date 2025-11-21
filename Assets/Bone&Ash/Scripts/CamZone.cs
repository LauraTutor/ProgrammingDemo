using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{

    [SerializeField]
    private CinemachineCamera cinemachineCamera = null;

    private void Start()
    {
        cinemachineCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            cinemachineCamera.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            cinemachineCamera.enabled = false;
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }

}