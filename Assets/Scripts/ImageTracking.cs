//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;

//public class PokemonSpawner : MonoBehaviour
//{
//    [Serializable]
//    public struct ImagePrefab
//    {
//        public string imageName;   // must match the name in the reference library
//        public GameObject prefab;
//    }

//    [SerializeField] ARTrackedImageManager imageManager;
//    [SerializeField] List<ImagePrefab> mappings;

//    readonly Dictionary<string, GameObject> prefabLookup = new();
//    readonly Dictionary<TrackableId, GameObject> spawned = new();

//    void Awake()
//    {
//        foreach (var m in mappings)
//            prefabLookup[m.imageName] = m.prefab;
//    }

//    void OnEnable() => imageManager.trackablesChanged.AddListener(OnChanged);
//    void OnDisable() => imageManager.trackablesChanged.RemoveListener(OnChanged);

//    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
//    {
//        foreach (var image in args.added)
//            Spawn(image);

//        foreach (var image in args.updated)
//        {
//            // Spawn if it wasn't ready when first added
//            if (!spawned.ContainsKey(image.trackableId))
//                Spawn(image);

//            if (spawned.TryGetValue(image.trackableId, out var go))
//                go.SetActive(image.trackingState == TrackingState.Tracking);
//        }

//        foreach (var kvp in args.removed)
//        {
//            if (spawned.TryGetValue(kvp.Key, out var go))
//            {
//                Destroy(go);
//                spawned.Remove(kvp.Key);
//            }
//        }
//    }

//    void Spawn(ARTrackedImage image)
//    {
//        string imageName = image.referenceImage.name;

//        if (string.IsNullOrEmpty(imageName)) return; // reference image not resolved yet
//        if (!prefabLookup.TryGetValue(imageName, out var prefab) || prefab == null) return;

//        spawned[image.trackableId] = Instantiate(prefab, image.transform);
//    }
//}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    // Kept as a public field with the same name so your assigned prefabs are not lost
    public GameObject[] ArPrefabs;

    [Tooltip("Extra rotation applied to the spawned prefab (useful for flat 2D/sprite prefabs). " +
             "Try (90, 0, 0) or (-90, 0, 0) if the prefab spawns but you can't see it.")]
    public Vector3 spawnRotationOffset = Vector3.zero;

    [Tooltip("Position of the spawned prefab relative to the center of the image (meters).")]
    public Vector3 spawnPositionOffset = Vector3.zero;

    ARTrackedImageManager trackedImages;

    // One spawned object per tracked image, keyed by the image's trackable ID
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
            Debug.Log($"[ImageTracker] Added: '{image.referenceImage.name}'");
            Spawn(image);
        }

        foreach (var image in args.updated)
        {
            // In case the reference image wasn't resolved when it was first added
            if (!spawned.ContainsKey(image.trackableId))
                Spawn(image);

            if (spawned.TryGetValue(image.trackableId, out var go) && go != null)
                go.SetActive(image.trackingState == TrackingState.Tracking);
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
            Debug.LogWarning("[ImageTracker] Reference image name is empty, will retry on update.");
            return;
        }

        foreach (var prefab in ArPrefabs)
        {
            if (prefab != null && prefab.name == imageName)
            {
                // Parent to the tracked image so it follows it automatically
                var instance = Instantiate(prefab, image.transform);

                // Instantiate keeps the prefab's own local position, so reset it to the image's center
                instance.transform.localPosition = spawnPositionOffset;
                instance.transform.localRotation =
                    Quaternion.Euler(spawnRotationOffset) * instance.transform.localRotation;

                spawned[image.trackableId] = instance;
                Debug.Log($"[ImageTracker] Spawned '{prefab.name}' on '{imageName}'");
                return;
            }
        }

        Debug.LogWarning($"[ImageTracker] No prefab named '{imageName}' in ArPrefabs.");
    }
}