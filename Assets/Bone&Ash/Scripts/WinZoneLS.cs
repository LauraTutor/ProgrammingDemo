using UnityEngine;

public class WinZoneLS : MonoBehaviour
{
    public GameObject player;
    public GameObject winPanel;
    public MonoBehaviour fpsController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            winPanel.SetActive(true);

            Time.timeScale = 0f;

            if (fpsController != null)
                fpsController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
