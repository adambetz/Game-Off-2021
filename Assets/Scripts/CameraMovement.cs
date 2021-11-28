using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float MovementSpeed = 5;
    private float AppliedMovementSpeed;

    private float CameraZoomAmount = 5;
    public float minZoomValue = 5;
    public float maxZoomAmount = 80;

    public float targetMinZoomValue = 1;
    public float targetMaxZoomAmount = 20;

    public GameObject PauseMenu;

    private bool focuseOnTarget;
    private GameObject currentTarget;

    private bool RayDone; //this bool is used to avoid multi clicking objects when its unwanted

    void FixedUpdate()
    {
        if (focuseOnTarget)
        {
            //follow target
            transform.position = new Vector3(currentTarget.transform.position.x, 90, currentTarget.transform.position.z);

            //lock cam zoom to greater then 1 and less then 10
            if (CameraZoomAmount <= targetMinZoomValue) { CameraZoomAmount = targetMinZoomValue; }
            if (CameraZoomAmount >= targetMaxZoomAmount) { CameraZoomAmount = targetMaxZoomAmount; }
        }
        else if(!PauseMenu.activeSelf)
        {
            //WASD movemnet
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            transform.position = transform.position + new Vector3(horizontalInput * AppliedMovementSpeed * Time.deltaTime, 0, verticalInput * AppliedMovementSpeed * Time.deltaTime);

            //lock cam zoom to greater then 1 and less then 10
            if (CameraZoomAmount <= minZoomValue) { CameraZoomAmount = minZoomValue; }
            if (CameraZoomAmount >= maxZoomAmount) { CameraZoomAmount = maxZoomAmount; }
        }

        if (PauseMenu.activeSelf) { return; }

        //Scroll to Zoom cam
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            CameraZoomAmount--;
            AppliedMovementSpeed = MovementSpeed * CameraZoomAmount / 2;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            CameraZoomAmount++;
            AppliedMovementSpeed = MovementSpeed * CameraZoomAmount / 2;
        }

        GetComponent<Camera>().orthographicSize = CameraZoomAmount;

        //Ray cast to see where you are right clicking
        if (Input.GetMouseButtonDown(1))
        {
            if (!RayDone)
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Transform hitObject = hit.transform;

                    //follow ant if right click
                    if (hitObject.tag == "Ant") { ZoomOnTarget(hitObject.gameObject); }
                    else { focuseOnTarget = false; }
                }
                RayDone = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            RayDone = false;
        }
    }

    public void ZoomOnTarget(GameObject target)
    {
        currentTarget = target;
        focuseOnTarget = true;
    }
}
