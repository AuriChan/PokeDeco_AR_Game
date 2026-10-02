using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ARInteractionToggle : MonoBehaviour
{
    [Header("AR elements to disable")]
    [SerializeField] private GameObject screenSpaceRayInteractor;
    [SerializeField] private ARPlaneManager planeManager;
    //[SerializeField] private MonoBehaviour rayInteractorComponent;
    [SerializeField] private ObjectSpawner objectSpawner;
    [SerializeField] private GameObject decorationMode;

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

        //if (rayInteractorComponent != null)
        //{
        //    rayInteractorComponent.enabled = isActive;
        //}
    }
    public void SetBlockTransforms(bool isActive)
    {

        if (objectSpawner != null)
        { 
            objectSpawner.GetSpawnedObjects();
            for (int i = 0; i < objectSpawner.GetSpawnedObjects().Count; i++)
            {
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackPosition = isActive;
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackScale = isActive;
                objectSpawner.GetSpawnedObjects()[i].GetComponent<XRGrabInteractable>().trackRotation = isActive;
            }
        }

    }
    void Update()
    {

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


