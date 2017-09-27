using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby : BaseObject 
{
	UIButton StageBtn = null;
	UIButton GachaBtn;
	UIButton InvenBtn;

	private void Awake()
	{
		Transform trans = GetChild("StageBtn");
		if(trans == null)
		{
			Debug.LogError("StageBtn 을 찾을수 없습니다.");
			return;
		}
		StageBtn = trans.GetComponent<UIButton>();
		EventDelegate.Add(StageBtn.onClick,
			new EventDelegate(this, "ShowStage"));


		trans = GetChild("GachaBtn");
		if (trans == null)
		{
			Debug.LogError("GachaBtn 을 찾을수 없습니다.");
			return;
		}
		GachaBtn = trans.GetComponent<UIButton>();
		EventDelegate.Add(GachaBtn.onClick,
			new EventDelegate(
						()=>
						{
							ItemManager.Instance.Gacha();
						}
					)
				);

		//--------------------------------------
		// Inventory
		trans = GetChild("InventoryBtn");
		if (trans == null)
		{
			Debug.LogError("InventoryBtn 을 찾을수 없습니다.");
			return;
		}
		InvenBtn = trans.GetComponent<UIButton>();
		EventDelegate.Add(InvenBtn.onClick,
			new EventDelegate(this, "ShowInventory"));
		
	}

	void ShowStage()
	{
		UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Stage);
	}

	void ShowInventory()
	{
		BaseObject inven = UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Inventory);
		inven.ThrowEvent("Inven_Init");
	}


}
