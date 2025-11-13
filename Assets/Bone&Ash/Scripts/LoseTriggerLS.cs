using UnityEngine;

public class LoseTriggerLS : MonoBehaviour
{
    public GameObject player;
    public GameObject losePanel;
    public MonoBehaviour fpsController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            losePanel.SetActive(true);

            Time.timeScale = 0f;

            if (fpsController != null)
                fpsController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
