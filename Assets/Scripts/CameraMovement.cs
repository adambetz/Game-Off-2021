using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool CameraState;
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;
    private float CameraZoom = 5;

    //Args for pan cam (in the works)
    //public float dragSpeed = 2;
    //private Vector3 dragOrigin;
    //private Vector3 dragToPoint;
    //private bool dragBool;

    private Vector3 CamPos;

    void Update()
    {
        //press W to toggle side / top view
        if (Input.GetKeyDown(KeyCode.W)) { CameraState = !CameraState; }
        
        if (CameraState)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }

        //Scroll to Zoom cam
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { CameraZoom--; }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { CameraZoom++; }

        //lock cam zoom to greater then 1 and less then 10
        if (CameraZoom <= 1) { CameraZoom = 1; }
        if (CameraZoom >= 10) { CameraZoom = 10; }

        vcam1.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = CameraZoom;
        vcam2.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = CameraZoom;

        //Middle mouse to pan cam
        /*if (Input.GetMouseButton(2))
        {

            if (dragBool)
            {
                dragOrigin = Input.mousePosition;

                CamPos = new Vector3(vcam2.transform.position.x, vcam2.transform.position.y, vcam2.transform.position.z);

                dragBool = false;
            }

            dragToPoint = Input.mousePosition;

            var y = dragOrigin.y / 100 - dragToPoint.y / 100;
            var x = dragOrigin.x / 100 - dragToPoint.x / 100;
            var z = dragOrigin.z / 100 - dragToPoint.z / 100;

            if (CameraState)
            {
                //Y & X
                vcam2.transform.position = new Vector3(x, y, CamPos.z);
            }
            else
            {
                //X & Z
                vcam1.transform.position = new Vector3(x, CamPos.y, z);
            }
        }
        else
        {
            dragBool = true;
        }*/
    }
}
