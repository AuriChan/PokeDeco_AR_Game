using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject Inventory;
    [SerializeField] Button openInventoryButton;

    void Start()
    {
        openInventoryButton.onClick.AddListener(Show);
    }

   
    void Update()
    {
        
    }

     void Show() 
    {
        Inventory.SetActive(true); 
    }
    public void Hide() 
    { 
        Inventory.SetActive(false); 
    }
}
