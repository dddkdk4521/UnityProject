using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : BaseObject
{
	ItemInstance itemInstance;
	public ItemInstance ITEM_INSTANCE
	{
		get { return itemInstance; }
		set { itemInstance = value; }
	}

	UILabel Label;
	UITexture Texture;

	public void Init(ItemInstance inst)
	{
		ITEM_INSTANCE = inst;

		Label = GetComponentInChildren<UILabel>();
		Texture = GetComponentInChildren<UITexture>();

		Label.text = inst.ITEM_INFO.NAME;
		Texture.mainTexture = Resources.Load(
			"Textures/" + inst.ITEM_INFO.ITEM_IMAGE
			) as Texture;
	}

	void OnClick()
	{
		BaseObject bo = UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Popup);
		UI_Popup popup = bo.GetComponent<UI_Popup>();

		popup.Set(
			ItemYes,
			ItemNo,
			"장비 장착",
			"이 장비를 장착 하시겠습니까?"
			);
	}

	void ItemYes()
	{
		ItemManager.Instance.EquipItem(ITEM_INSTANCE);
		UI_Tools.Instance.HideUI(eUIType.Pf_UI_Popup);
	}

	void ItemNo()
	{
		UI_Tools.Instance.HideUI(eUIType.Pf_UI_Popup);
	}

	
}
