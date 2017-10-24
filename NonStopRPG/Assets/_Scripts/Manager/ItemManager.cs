using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EquipEvent();

public class ItemManager : MonoSingleton<ItemManager>
{
	bool IsInit = false;
	public EquipEvent EquipE;

	// 소지 아이템
	List<ItemInstance> listItem = new List<ItemInstance>();
	public List<ItemInstance> LIST_ITEM
	{
		get { return listItem; }
	}

	// 장착 슬롯
	Dictionary<eSlotType, ItemInstance> DicEquipItem = new Dictionary<eSlotType, ItemInstance>();
	public Dictionary<eSlotType,ItemInstance> DIC_EQUIP
	{
		get { return DicEquipItem; }
	}

	// 아이템 원본 데이터
	Dictionary<int, ItemInfo> DicItemInfo = new Dictionary<int, ItemInfo>();
	public Dictionary<int, ItemInfo> DIC_ITEMINFO
	{
		get { return DicItemInfo; }
	}

	public void ItemInit()
	{
		if (IsInit == true)
			return;
		IsInit = true;

		TextAsset itemInfo = 
			Resources.Load("JSON/ITEM_INFO") as TextAsset;

		JSONNode rootNode = JSON.Parse(itemInfo.text);

		foreach(KeyValuePair<string, JSONNode> pair
			in rootNode["ITEM_INFO"] as SimpleJSON.JSONObject)
		{
			ItemInfo info = new ItemInfo(pair.Key, pair.Value);
			DicItemInfo.Add(int.Parse(pair.Key), info);
		}

		GetLocalData();
	}

	public void Gacha()
	{
		int id = Random.Range(1, DicItemInfo.Count + 1);
		ItemInfo info = null;

		DicItemInfo.TryGetValue(id, out info);
		if(info == null)
		{
			Debug.LogError(id + " is not Valid Key");
			return;
		}
		ItemInstance instance = new ItemInstance(listItem.Count + 1, eSlotType.Slot_None, info);
		listItem.Add(instance);

		SetLocalData();

		// Debug.Log(instance.ITEM_INFO.NAME + " 뽑음.");

		BaseObject ui = UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Gacha);
		ui.ThrowEvent("GACHA", instance);
	}

	public void GetLocalData()
	{
		// ITEM_NO _ SlotType _ ITEM_ID | ITEM_NO _ SlotType _ ITEM_ID
		string instanceStr = PlayerPrefs.GetString(
			ConstValue.LocalSave_ItemInstance, string.Empty);

		string[] array = instanceStr.Split('|');

		// [0] ITEM_NO _ SlotType _ ITEM_ID
		// [1] ITEM_NO _ SlotType _ ITEM_ID
		// ...

		for(int i = 0; i< array.Length; i++)
		{
			// ITEM_NO _ SlotType _ ITEM_ID 문자열의 길이
			if (array[i].Length <= 0)
				continue;

			string[] detail = array[i].Split('_');
			// [0] ITEM_NO 
			// [1] SlotType
			// [2] ITEM_ID

			if (detail.Length < 3)
				continue;

			int itemNo = int.Parse(detail[0]);

			eSlotType slotType = (eSlotType)(int.Parse(detail[1]));

			ItemInfo info = null;
			DicItemInfo.TryGetValue(int.Parse(detail[2]), out info);
			if(info == null)
			{
				Debug.LogError(
					"NO : " + itemNo + " ItemID : " + detail[2]
					+ " ID로 키값을 찾을수 없습니다."
					);
				continue;
			}

			ItemInstance instance = new ItemInstance(itemNo, slotType, info);
			listItem.Add(instance);		

			if(slotType != eSlotType.Slot_None)
			{
				EquipItem(instance, false);
			}

		}
	}

	public void SetLocalData()
	{
		// ITEM_NO _ SlotType _ ITEM_ID | ITEM_NO _ SlotType _ ITEM_ID
		// 1_-1_3|2_1_5|3_2_5

		string resultStr = string.Empty;

		for(int i = 0; i<listItem.Count; i++)
		{
			string itemStr = string.Empty;
			// ItemNO
			itemStr += (i + 1) + "_";
			// SlotType
			itemStr += (int)listItem[i].EQUIP_SLOT + "_";
			// ItemID
			itemStr += listItem[i].ITEM_ID;

			if (i != listItem.Count - 1)
				itemStr += "|";

			resultStr += itemStr;
		}

		PlayerPrefs.SetString(ConstValue.LocalSave_ItemInstance, resultStr);

        Debug.Log(resultStr);
	}

	public void EquipItem(ItemInstance instance, bool isSave = true)
	{
		ItemInfo info = instance.ITEM_INFO;

		if(DicEquipItem.ContainsKey(info.TYPE))
		{
			// 착용 해제
			DicEquipItem[info.TYPE].EQUIP_SLOT = eSlotType.Slot_None;

			// 새롭게 장착
			instance.EQUIP_SLOT = info.TYPE;
			DicEquipItem[info.TYPE] = instance;
		}
		else
		{
			instance.EQUIP_SLOT = info.TYPE;
			DicEquipItem.Add(info.TYPE, instance);
		}

		if (EquipE != null)
			EquipE();

		if (isSave)
			SetLocalData();
	}
}