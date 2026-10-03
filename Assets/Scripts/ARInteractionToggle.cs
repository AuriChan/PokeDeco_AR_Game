using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ARInteractionToggle : MonoBehaviour
{
    [Header("AR elements to disable")]
    [SerializeField] private GameObject screenSpaceRayInteractor;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ObjectSpawner objectSpawner;
    [SerializeField] private GameObject Spawner;
    [SerializeField] private GameObject decorationMode;

    private GameObject selectedObject;

    // This function will be called by Inventory and Photo Mode UI buttons (just Phono Mode for the moment)
    public void SetARInteractionsActive(bool isActive)
    {
        //disable interaction in photo mode
        if (screenSpaceRayInteractor != null)
        {
            screenSpaceRayInteractor.SetActive(isActive);
        }

        //disable plane visualization in photo mode
        if (planeManager != null)
        {
            planeManager.enabled = isActive;

            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(isActive);
            }
        }

        if (objectSpawner != null)
        {
            objectSpawner.enabled = isActive;
        }

    }
    public void DeleteSelectedObject()
    {
        if (selectedObject == null)
        {
            return;
        }

        objectSpawner.GetSpawnedObjects().Remove(selectedObject);
        Destroy(selectedObject);
        selectedObject = null;
    }
    public void DeleteAllObject()
    {
        while (objectSpawner.GetSpawnedObjects().Count > 0)
        {
            GameObject obj = objectSpawner.GetSpawnedObjects()[0];
            objectSpawner.GetSpawnedObjects().RemoveAt(0);
            if (obj != null)
            {
                Destroy(obj);
            }
        }

            selectedObject = null;
    }
    void Update()
    {
        if (Spawner.transform.childCount > 0)
        {
            selectedObject = Spawner.transform.GetChild(Spawner.transform.childCount - 1).gameObject;
        }
        else
        {
            selectedObject = null;
        }
        if ((decorationMode != null) && decorationMode.activeSelf == true)
        {
            for (int i = 0; i < objectSpawner.GetSpawnedObjects().Count; i++)
            {
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackPosition = false;
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackScale = false;
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackRotation = false;
            }
        }
        else
        {
            for (int i = 0; i < objectSpawner.GetSpawnedObjects().Count; i++)
            {
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackPosition = true;
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackScale = true;
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackRotation = true;
            }
        }
    }

}


