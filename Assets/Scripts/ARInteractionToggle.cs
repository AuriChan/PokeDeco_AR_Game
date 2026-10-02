using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARInteractionToggle : MonoBehaviour
{
    [Header("AR elements to disable")]
    [SerializeField] private GameObject screenSpaceRayInteractor;
    [SerializeField] private ARPlaneManager planeManager;
    //[SerializeField] private MonoBehaviour rayInteractorComponent;
    //[SerializeField] private MonoBehaviour objectSpawner;

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

        //if (objectSpawner != null)
        //{
        //    objectSpawner.enabled = isActive;
        //}

        //if (rayInteractorComponent != null)
        //{
        //    rayInteractorComponent.enabled = isActive;
        //}
    }
}


