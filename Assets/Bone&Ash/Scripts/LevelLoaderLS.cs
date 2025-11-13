using UnityEngine;

public class LevelLoaderLS : MonoBehaviour
{
    public MonoBehaviour fpsController;
     
    void Awake()
    {
            Time.timeScale = 1f;

            if (fpsController != null)
                fpsController.enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
    }
}