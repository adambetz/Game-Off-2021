using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float CameraZoomAmount = 5;
    public float minZoomValue = 5;
    public float maxZoomAmount = 80;

    void FixedUpdate()
    {
        //WASD movemnet
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(horizontalInput * 5 * Time.deltaTime, 0, verticalInput * 5 * Time.deltaTime);

        //Scroll to Zoom cam
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { CameraZoomAmount--; }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { CameraZoomAmount++; }

        //lock cam zoom to greater then 1 and less then 10
        if (CameraZoomAmount <= minZoomValue) { CameraZoomAmount = minZoomValue; }
        if (CameraZoomAmount >= maxZoomAmount) { CameraZoomAmount = maxZoomAmount; }

        GetComponent<Camera>().orthographicSize = CameraZoomAmount;
    }
}
