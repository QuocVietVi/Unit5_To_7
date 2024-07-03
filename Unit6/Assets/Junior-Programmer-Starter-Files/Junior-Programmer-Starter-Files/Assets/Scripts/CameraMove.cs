using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public float minX = -27f;
    public float maxX = 2f;
    public float minZ = -12f;
    public float maxZ = 19f;
    public float zoomSpeed = 0.01f; 
    public float zoomOutMin = 23f; 
    public float zoomOutMax = 68f; 
    private float previousDistance;

    Camera mainCamera;

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    Vector2 firstTouchPrevPos, secondTouchPrevPos;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;

            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.y * dragSpeed, 0, pos.x * dragSpeed);
            move.z = -move.z;
            transform.Translate(-move, Space.World);

            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

            transform.position = clampedPosition;
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
#endif

#if UNITY_ANDROID 
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(touch.position) - Camera.main.ScreenToViewportPoint(dragOrigin);
                Vector3 move = new Vector3(pos.y * dragSpeed *2, 0, pos.x * dragSpeed *2);
                move.z = -move.z;
                transform.Translate(-move, Space.World);

                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
                clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

                transform.position = clampedPosition;

                dragOrigin = touch.position; // Update dragOrigin
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = Vector3.Distance(touchZeroPrevPos, touchOnePrevPos);
            float touchDeltaMag = Vector3.Distance(touchZero.position, touchOne.position);
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Zoom(deltaMagnitudeDiff * zoomSpeed);
        }
#endif
    }

    void Zoom(float increment)
    {
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView + increment, zoomOutMin, zoomOutMax);
        //Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment * 10, zoomOutMin, zoomOutMax);
    }
}

