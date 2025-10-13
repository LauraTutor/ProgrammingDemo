using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{

    [SerializeField] private float YMin = -20.0f;
    [SerializeField] private float YMax = 60.0f;

    public Transform lookAt;

    public Transform Player;

    public float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensivity = 4.0f;



    void Start()
    {


    }

    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * direction;

        transform.LookAt(lookAt.position);
    }
}