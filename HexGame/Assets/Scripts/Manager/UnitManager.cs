using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{
    public HexUnit unitPrefab;// TODO: UnitManager로 이동
    List<HexUnit> units = new List<HexUnit>();// TODO : UnitManager 대체
}
