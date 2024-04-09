using UnityEngine;
using Google.XR.ARCoreExtensions;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class Doer : MonoBehaviour
{
    // ARCore Extensions object reference.
    ARAnchorManager anchorManager;
    ARRaycastManager aRRaycastManager;

    // Prefab for the anchor.
    public GameObject anchorPrefab;

    AREarthManager AREarth;
    void Start()
    {
        // Get the ARAnchorManager component from ARCoreExtensions.
        anchorManager = GetComponent<ARAnchorManager>();
        ARAnchorManagerExtensions.SetAuthToken(anchorManager, "867467600330-sbu6nb94giba6pp53b4k7u1vhoom2c9m.apps.googleusercontent.com");
        aRRaycastManager = GetComponent<ARRaycastManager>();
        // Subscribe to the Anchor changed event.
        
        AREarth = GetComponent<AREarthManager>();

        if (anchorManager == null)
        {
            Debug.LogError("ARAnchorManager component is missing.");
            return;
        }
        else if (aRRaycastManager == null)
        {
            Debug.LogError("ARRaycastManager component is missing.");
            return;
        }
        else if (AREarth == null)
        {
            Debug.LogError("AREarthManager component is missing.");
            return;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CreateAnchor(Input.GetTouch(0).position);
        }
    }
    void CreateAnchor(Vector2 screenPosition)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (anchorManager==null||aRRaycastManager==null||AREarth==null)
        {
            Debug.Log("These are the problems");
            return;
        }
        if (aRRaycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // Instantiate anchor at the hit point.
            Pose pose = hits[0].pose;
            Debug.Log(pose);
            GeospatialPose p = AREarth.Convert(pose);

            ARGeospatialAnchor anchor = ARAnchorManagerExtensions.AddAnchor(anchorManager,p.Latitude,p.Longitude,p.Altitude,p.EunRotation);
            if (anchor == null) Debug.Log("This is wrong");
            else Debug.Log("Added successfully");
            // Instantiate your anchor prefab at the anchor's position.
            Instantiate(anchorPrefab, anchor.transform.position, anchor.transform.rotation);
        }
    }
    // Method to handle anchor changes.
    //void OnAnchorsChanged(ARAnchorsChangedEventArgs eventArgs)
    //{
    //    foreach (var addedAnchor in eventArgs.added)
    //    {
    //        Debug.Log("Anchor added");
    //    }
    //    foreach (var updatedAnchor in eventArgs.updated)
    //    {
    //        Debug.Log("Anchor updated");
    //    }
    //    foreach (var removedAnchor in eventArgs.removed)
    //    {
    //        Debug.Log("Anchor removed");
    //    }
    //}

}