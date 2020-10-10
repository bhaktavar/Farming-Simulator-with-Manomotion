using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class Placement : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private float defaultRotation = -90;

    [SerializeField]

    private GameObject placedObject;

    private Vector3 placePosition;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private bool isLocked = false;

    private bool onTouchHold = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private ARPlaneManager arPlaneManager;

    public GameObject ManomotionStuff;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        // do not capture events unless the welcome panel is hidden
        //if(welcomePanel.activeSelf)
        //    return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    //PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
                    //if (placementObject != null)
                    //{
                    //    onTouchHold = isLocked ? false : true;
                    //    placementObject.SetOverlayText(isLocked ? "AR Object Locked" : "AR Object Unlocked");
                    //}
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }
        }

        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (placedObject == null)
            {
                placedPrefab.transform.position = hitPose.position;
                placedPrefab.SetActive(true);
                placedObject = placedPrefab;
                //if (defaultRotation > 0)
                //{
                //    placedObject = Instantiate(placedPrefab, hitPose.position, Quaternion.identity);
                //    placedObject.transform.Rotate(Vector3.up, defaultRotation);
                //}
                //else
                //{
                //    placedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                //}
            }
            else
            {
                foreach (var plane in arPlaneManager.trackables)
                    plane.gameObject.SetActive(false);
                arPlaneManager.enabled = false;
                ManomotionStuff.SetActive(true);
                this.GetComponent<Placement>().enabled = false;

                if (onTouchHold)
                {
                    placedObject.transform.position = hitPose.position;
                    if (defaultRotation == 0)
                    {
                        placedObject.transform.rotation = hitPose.rotation;
                    }
                }
            }
        }
    }
}