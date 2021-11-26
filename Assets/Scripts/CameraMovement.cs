using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float CameraZoomAmount = 5;
    public float minZoomValue = 5;
    public float maxZoomAmount = 80;

    public float targetMinZoomValue = 1;
    public float targetMaxZoomAmount = 20;

    private bool focuseOnTarget;
    private GameObject currentTarget;

    void FixedUpdate()
    {
        //Scroll to Zoom cam
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { CameraZoomAmount--; }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { CameraZoomAmount++; }
        GetComponent<Camera>().orthographicSize = CameraZoomAmount;

        if (focuseOnTarget)
        {
            //follow target
            transform.position = new Vector3(currentTarget.transform.position.x, 90, currentTarget.transform.position.z);

            //lock cam zoom to greater then 1 and less then 10
            if (CameraZoomAmount <= targetMinZoomValue) { CameraZoomAmount = targetMinZoomValue; }
            if (CameraZoomAmount >= targetMaxZoomAmount) { CameraZoomAmount = targetMaxZoomAmount; }
        }
        else
        {
            //WASD movemnet
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            transform.position = transform.position + new Vector3(horizontalInput * 5 * Time.deltaTime, 0, verticalInput * 5 * Time.deltaTime);

            //lock cam zoom to greater then 1 and less then 10
            if (CameraZoomAmount <= minZoomValue) { CameraZoomAmount = minZoomValue; }
            if (CameraZoomAmount >= maxZoomAmount) { CameraZoomAmount = maxZoomAmount; }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            focuseOnTarget = false;
        }
    }

    public void ZoomOnTarget(GameObject target)
    {
        currentTarget = target;
        focuseOnTarget = true;
    }
}
