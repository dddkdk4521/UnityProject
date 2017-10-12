using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
	int ItemID = -1;
	int ItemNO = -1;

	eSlotType EquipSlotType = eSlotType.Slot_None;
	ItemInfo Info = null;

	public int ITEM_ID { get { return ItemID; } }
	public int ITEM_NO { get { return ItemNO; } }
	public ItemInfo ITEM_INFO { get { return Info; } }

	public eSlotType EQUIP_SLOT
	{
		get { return EquipSlotType; }
		set { EquipSlotType = value; }
	}

	public ItemInstance(int no, eSlotType equipType, ItemInfo info)
	{
		this.ItemNO = no;
		this.ItemID = int.Parse(info.KEY);

		this.EquipSlotType = equipType;
		this.Info = info;
	}
}
