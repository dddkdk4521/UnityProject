using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : BaseObject 
{
	bool IsInit = false;
	GameObject ItemPrefab;

	UIGrid Grid;
	UIButton CloseBtn;

	UILabel WeaponLabel;
	UILabel ArmorLabel;
	UILabel ShieldLabel;
	UILabel AccLabel;

	public override void ThrowEvent(string keyData, params object[] datas)
	{
        if (keyData.Equals("Inven_Init") == true)
		{
			Init();
			Reset();
		}
	}

	public void Init()
	{
		if (this.IsInit == true)
			return;

        this.IsInit = true;

		this.ItemPrefab = Resources.Load("Prefabs/UI/Pf_UI_Item") as GameObject;
		this.Grid = GetComponentInChildren<UIGrid>();

		// Close Btn
		this.CloseBtn = GetChild("CloseBtn").GetComponent<UIButton>();
		EventDelegate.Add(
			CloseBtn.onClick,
			new EventDelegate(
				()=> { UI_Tools.Instance.HideUI(eUIType.Pf_UI_Inventory); }
				));

		this.WeaponLabel = GetChild("Weapon").GetComponent<UILabel>();
		this.ArmorLabel = GetChild("Armor").GetComponent<UILabel>();
		this.ShieldLabel = GetChild("Shield").GetComponent<UILabel>();
		this.AccLabel = GetChild("Acc").GetComponent<UILabel>();

		EquipItemReset();
		ItemManager.Instance.EquipE = EquipItemReset;
	}

	public void Reset()
	{
		for(int i = 0; i< Grid.transform.childCount; i++)
		{
			Destroy(Grid.transform.GetChild(i).gameObject);
		}

        AddItem();
	}

	void AddItem()
	{
		List<ItemInstance> list = ItemManager.Instance.LIST_ITEM;
		for(int i = 0; i<list.Count; i++)
		{
			GameObject go = Instantiate(ItemPrefab, Grid.transform);
            {
                go.transform.localScale = Vector3.one;
			    go.GetComponent<UI_Item>().Init(list[i]);
            }
		}

		Grid.repositionNow = true;
	}

	public void EquipItemReset()
	{
		Dictionary<eSlotType, ItemInstance> dic = ItemManager.Instance.DIC_EQUIP;

		foreach(KeyValuePair<eSlotType, ItemInstance> pair in dic)
		{
			switch (pair.Key)
			{
				case eSlotType.Slot_Weapon:
					WeaponLabel.text = pair.Value.ITEM_INFO.GetSlotString()
						+ "    " + pair.Value.ITEM_INFO.NAME;
					break;
				case eSlotType.Slot_Armor:
					ArmorLabel.text = pair.Value.ITEM_INFO.GetSlotString()
						+ "    " + pair.Value.ITEM_INFO.NAME;
					break;
				case eSlotType.Slot_Shield:
					ShieldLabel.text = pair.Value.ITEM_INFO.GetSlotString()
						+ "    " + pair.Value.ITEM_INFO.NAME;
					break;
				case eSlotType.Slot_Acc:
					AccLabel.text = pair.Value.ITEM_INFO.GetSlotString()
						+ "    " + pair.Value.ITEM_INFO.NAME;
					break;
			}
		}
	}
}
