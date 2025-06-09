using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform craftSlotParent;

    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }


    public void SetupCraftList()
    {
        CleanupCraftSolt();
        for (var i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }


        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void CleanupCraftSolt()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}