using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Get Access to Unity XR classes.
using UnityEngine.XR.ARFoundation;
public class Clicker : MonoBehaviour
{
    bool isSessionQualityOK;
    public bool ManoClicked = false, ManoReleased = false;
    private void Start()
    {
        ARSession.stateChanged += HandleStateChanged;
    }
    // Update is called once per frame
    void Update()
    {
        if (isSessionQualityOK)
            SpawnCubewithclickTriggerGesture();

        if (ManoClicked)
        {
            print("Manomotion Clicked");
        }
        if (ManoReleased)
        {
            print("Manomotion Released");
        }
    }

    /// <summary>
    /// Handles the situation when the status of the Session has changed. In this case keep track if the session is in Good Quality and Tracking.
    /// </summary>
    /// <param name="stateEventArguments"></param>
    private void HandleStateChanged(ARSessionStateChangedEventArgs stateEventArguments)
    {
        isSessionQualityOK = stateEventArguments.state == ARSessionState.SessionTracking;
    }
    //TODO
    //Replace this with a prefab of items you feel like spawning.
    public GameObject itemPrefab;
    /// <summary>
    /// Spawns a cube in front of the user when the user performs a Click Gesture and its detected by the system.
    /// </summary>

    private void SpawnCubewithclickTriggerGesture()
    {
        //All the information for a detected hand. It refers to a single hand.
        HandInfo handInformation = ManomotionManager.Instance.Hand_infos[0].hand_info;
        //All the gesture information for this hand.
        GestureInfo gestureInformation = handInformation.gesture_info;
        //The trigger gesture that is detected in this frame.
        
        ManoGestureTrigger currentDetectedTriggerGesture = gestureInformation.mano_gesture_trigger;
        if (currentDetectedTriggerGesture == ManoGestureTrigger.CLICK)
        {
            //A click has been performed by the user and it has been detected as the current trigger Gesture.
            ManoClicked = true;
            StartCoroutine(timer(1));
            //GameObject newItem = Instantiate(itemPrefab);
            ////Spawn a cube at the position of the camera, also adding a small offset forward so it does not spawn right in front of it.
            //Vector3 positionToMove = Camera.main.transform.position + (Camera.main.transform.forward * 2);
            //newItem.transform.position = positionToMove;
            ////Make the phone vibrate for feedback.
            Handheld.Vibrate();
        }
        else if (currentDetectedTriggerGesture == ManoGestureTrigger.RELEASE_GESTURE)
        {
            //A grab and release has been performed by the user and it has been detected as the current trigger Gesture.
            ManoReleased = true;
            StartCoroutine(timer(2));
            Handheld.Vibrate();
        }
    }
    IEnumerator timer(int i)
    {
        yield return new WaitForEndOfFrame();
        if (i == 1)
            ManoClicked = false;
        else if (i == 2)
            ManoReleased = false;

    }
}