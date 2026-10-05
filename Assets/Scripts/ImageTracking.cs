using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    public GameObject[] ArPrefabs;
    private Vector3 spawnPositionOffset = Vector3.zero;

    ARTrackedImageManager trackedImages;
    readonly Dictionary<TrackableId, GameObject> spawned = new Dictionary<TrackableId, GameObject>();

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    void OnDisable()
    {
        trackedImages.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var image in args.added)
        {
           
            Spawn(image);
        }

        foreach (var image in args.updated)
        {
           
            if (!spawned.ContainsKey(image.trackableId))
            {
                Spawn(image);
            }
                

            if (spawned.TryGetValue(image.trackableId, out var go) && go != null)
            {
                go.SetActive(image.trackingState == TrackingState.Tracking);
            }
                
        }

        foreach (var kvp in args.removed)
        {
            if (spawned.TryGetValue(kvp.Key, out var go))
            {
                if (go != null) Destroy(go);
                spawned.Remove(kvp.Key);
            }
        }
    }

    void Spawn(ARTrackedImage image)
    {
        string imageName = image.referenceImage.name;

        if (string.IsNullOrEmpty(imageName))
        {
          
            return;
        }

        foreach (var prefab in ArPrefabs)
        {
            if (prefab != null && prefab.name == imageName)
            {
               
                var instance = Instantiate(prefab, image.transform);
                switch (prefab.name)
                {
                    case "Mudkip_Card":
                        spawnPositionOffset = new Vector3(0,0.2f,0);
                        break;
                    case "Torchic_Card":
                        spawnPositionOffset = new Vector3(0, 0.15f, 0);
                        break;
                    case "Treecko_Card":
                        spawnPositionOffset = new Vector3(0,0,0);
                        break;
                    default:
                        spawnPositionOffset = Vector3.zero;
                        break;
                }
                instance.transform.localPosition = spawnPositionOffset;
                instance.transform.localRotation = instance.transform.localRotation;

                spawned[image.trackableId] = instance;
               
                return;
            }
        }

       
    }
}