using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoSingleton<BoardManager>
{
	Dictionary<BaseObject, List<BaseBoard>> DicBoard =
		new Dictionary<BaseObject, List<BaseBoard>>();

	GameObject BoardUI = null;

	Dictionary<eBoardType, GameObject> DicPrefab = new Dictionary<eBoardType, GameObject>();

	private void Awake()
	{
		if (BoardUI == null)
		{
			BoardUI = new GameObject("BoardUI_Root");
			BoardUI.layer = LayerMask.NameToLayer("UI");

			BoardUI.transform.SetParent(UIManager.Instance.transform);
			BoardUI.transform.localPosition = Vector3.zero;
			BoardUI.transform.localScale = Vector3.one;
		}

		GameObject go = Resources.Load(ConstValue.UI_PATH_HP) as GameObject;
		if (go != null)
		{
			DicPrefab.Add(eBoardType.BOARD_HP, go);
		}
		else
			Debug.LogError("HP Board 로드 실패 ");

		go = Resources.Load(ConstValue.UI_PATH_DAMAGE) as GameObject;
		if (go != null)
		{
			DicPrefab.Add(eBoardType.BOARD_DAMAGE, go);
		}
		else
			Debug.LogError("Damage Board 로드 실패 ");

	}

	private void Update()
	{
		if (GameManager.Instance.GAME_OVER)
			return;

		BaseBoard destroyBoard = null;

		foreach (KeyValuePair<BaseObject, List<BaseBoard>> pair
			in DicBoard)
		{
			List<BaseBoard> listBoard = pair.Value;

			for (int i = 0; i < listBoard.Count; i++)
			{
				listBoard[i].UpdateBoard();

				if (listBoard[i].CheckDestroyTime() == true)
				{
					destroyBoard = listBoard[i];
					listBoard.Remove(destroyBoard);
					Destroy(destroyBoard.gameObject);
				}
			}
		}
	}

	public BaseBoard AddBoard(BaseObject keyObject, eBoardType boardType)
	{
		if (GameManager.Instance.GAME_OVER)
			return null;

		List<BaseBoard> listBoard = null;

		if (DicBoard.ContainsKey(keyObject) == false)
		{
			listBoard = new List<BaseBoard>();
			DicBoard.Add(keyObject, listBoard);
		}
		else
			listBoard = DicBoard[keyObject];

		BaseBoard boardData = MakeBoard(boardType);
		boardData.TargetComponent = keyObject;
		listBoard.Add(boardData);

		return boardData;
	}

	BaseBoard MakeBoard(eBoardType boardType)
	{
		BaseBoard boardData = null;

		switch (boardType)
		{
			case eBoardType.BOARD_NONE:
				{
					Debug.LogError(eBoardType.BOARD_NONE.ToString() + "으로 접근 하였습니다.");
				}
				break;
			case eBoardType.BOARD_HP:
				{
					GameObject hpBoard = NGUITools.AddChild(
						BoardUI,
						DicPrefab[eBoardType.BOARD_HP]);
					boardData = hpBoard.GetComponent<HpBoard>();
				}
				break;
			case eBoardType.BOARD_DAMAGE:
				{
					GameObject damageBoard = NGUITools.AddChild(
						BoardUI,
						DicPrefab[eBoardType.BOARD_DAMAGE]);
					boardData = damageBoard.GetComponent<DamageBoard>();
				}
				break;
		}

		return boardData;
	}

	public BaseBoard GetBoardData(BaseObject keyObject, eBoardType boardType)
	{
		if (DicBoard.ContainsKey(keyObject) == false)
			return null;

		List<BaseBoard> listBoard = DicBoard[keyObject];

		for (int i = 0; i < listBoard.Count; i++)
		{
			if (listBoard[i].BOARD_TYPE == boardType)
				return listBoard[i];
		}

		return null;
	}

	public void ShowBoard(BaseObject keyObject, bool bEnable = true)
	{
		if (DicBoard.ContainsKey(keyObject) == false)
			return;

		List<BaseBoard> listBoard = DicBoard[keyObject];

		for (int i = 0; i < listBoard.Count; i++)
		{
			if (listBoard[i].gameObject.activeSelf != bEnable)
				listBoard[i].gameObject.SetActive(bEnable);
		}
	}

	public void ClearBoard(BaseObject keyObject)
	{
		if (DicBoard.ContainsKey(keyObject) == false)
			return;

		List<BaseBoard> listBoard = DicBoard[keyObject];

		for (int i = 0; i < listBoard.Count; i++)
		{
			if (listBoard[i] != null)
				Destroy(listBoard[i].gameObject);
		}
		DicBoard.Remove(keyObject);
	}
}
