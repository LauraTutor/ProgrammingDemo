using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    public GameObject cineCamera1; 
    public GameObject cineCamera2; 
    public GameObject cineCamera3; 
    public GameObject cineCamera4; 

    private int currentCamIndex = 0;
    private GameObject[] cameras;

    void Awake()
    {
        cameras = new GameObject[] { cineCamera1, cineCamera2, cineCamera3, cineCamera4 };
        SwitchToCamera(0); 
    }

    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
        {
            SwitchLeft();
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            SwitchRight();
        }
    }

    private void SwitchLeft()
    {
        currentCamIndex--;
        if (currentCamIndex < 0)
            currentCamIndex = cameras.Length - 1;

        SwitchToCamera(currentCamIndex);
    }

    private void SwitchRight()
    {
        currentCamIndex++;
        if (currentCamIndex >= cameras.Length)
            currentCamIndex = 0;

        SwitchToCamera(currentCamIndex);
    }

    private void SwitchToCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(i == index);
        }
    }
}