using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class HexMapEditor : MonoBehaviour
{
	public HexGrid hexGrid;
	public Material terrainMaterial;

    // ShaderStyle Levels
	int activeElevation;
	int activeWaterLevel;

	bool applyElevation = true;
	bool applyWaterLevel = true;

    // ObjectStyle Levels
	int activeUrbanLevel, activeFarmLevel, activePlantLevel, activeSpecialIndex;
	bool applyUrbanLevel, applyFarmLevel, applyPlantLevel, applySpecialIndex;

    // TerrainSelectIndex  current : 0 ~ 4
	int activeTerrainTypeIndex;

	int brushSize;

	enum OptionalToggle
    {
		Ignore, Yes, No
	}
	OptionalToggle riverMode, roadMode, walledMode;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;
//////////////////////////////////////////////////////////////
/////////////////// MapEditor Event Method ///////////////////
/// //////////////////////////////////////////////////////////
	public void SetTerrainTypeIndex (int index)
    {
		this.activeTerrainTypeIndex = index;
	}

	public void SetApplyElevation (bool toggle)
    {
		this.applyElevation = toggle;
	}

	public void SetElevation (float elevation)
    {
		this.activeElevation = (int)elevation;
	}

	public void SetApplyWaterLevel (bool toggle)
    {
		this.applyWaterLevel = toggle;
	}

	public void SetWaterLevel (float level)
    {
		this.activeWaterLevel = (int)level;
	}

	public void SetApplyUrbanLevel (bool toggle)
    {
		this.applyUrbanLevel = toggle;
	}

	public void SetUrbanLevel (float level)
    {
		this.activeUrbanLevel = (int)level;
	}

	public void SetApplyFarmLevel (bool toggle)
    {
		this.applyFarmLevel = toggle;
	}

	public void SetFarmLevel (float level)
    {
		this.activeFarmLevel = (int)level;
	}

	public void SetApplyPlantLevel (bool toggle)
    {
		this.applyPlantLevel = toggle;
	}

	public void SetPlantLevel (float level)
    {
		this.activePlantLevel = (int)level;
	}

	public void SetApplySpecialIndex (bool toggle)
    {
		this.applySpecialIndex = toggle;
	}

	public void SetSpecialIndex (float index)
    {
		this.activeSpecialIndex = (int)index;
	}

	public void SetBrushSize (float size)
    {
		this.brushSize = (int)size;
	}

	public void SetRiverMode (int mode)
    {
		this.riverMode = (OptionalToggle)mode;
	}

	public void SetRoadMode (int mode)
    {
		this.roadMode = (OptionalToggle)mode;
	}

	public void SetWalledMode (int mode)
    {
		this.walledMode = (OptionalToggle)mode;
	}

	public void SetEditMode (bool toggle)
    {
		this.enabled = toggle;
	}

	public void SetShowGrid (bool visible)
    {
		if (visible)
        {
			this.terrainMaterial.EnableKeyword("GRID_ON");
		}
		else
        {
			this.terrainMaterial.DisableKeyword("GRID_ON");
		}
	}
//////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////

    void Awake ()
    {
		this.terrainMaterial.DisableKeyword("GRID_ON");
		SetEditMode(false);
	}

	void Update ()
    {
        // MouseInput Handle
        if (!EventSystem.current.IsPointerOverGameObject())
        {
			if (Input.GetMouseButton(0))
            {
				HandleInput();
				return;
			}
            
            // Unit Generate
            if (Input.GetKeyDown(KeyCode.U))
            {
				if (Input.GetKey(KeyCode.LeftShift))
                {
					DestroyUnit();
				}
				else
                {
					CreateUnit();
				}

                return;
			}
		}
		this.previousCell = null;
	}

	private HexCell GetCellUnderCursor ()
    {
		return hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
	}

	private void CreateUnit ()
    {
		HexCell cell = GetCellUnderCursor();
		if (cell && !cell.Unit)
        {
			hexGrid.AddUnit(Instantiate(HexUnit.unitPrefab), cell, Random.Range(0f, 360f));
		}
	}

	private void DestroyUnit ()
    {
		HexCell cell = GetCellUnderCursor();
		if (cell && cell.Unit)
        {
			hexGrid.RemoveUnit(cell.Unit);
		}
	}

	private void HandleInput ()
    {
		HexCell currentCell = GetCellUnderCursor();
		if (currentCell)
        {
			if (previousCell && previousCell != currentCell)
            {
				ValidateDrag(currentCell);
			}
			else
            {
				this.isDrag = false;
			}
            
            // Edit Starting
            EditCells(currentCell);
			this.previousCell = currentCell;
		}
		else
        {
			this.previousCell = null;
		}
	}

	private void ValidateDrag (HexCell currentCell) 
    {
        // Oritation check
        for (this.dragDirection = HexDirection.NE; this.dragDirection <= HexDirection.NW; this.dragDirection++)
        {
			if (previousCell.GetNeighbor(dragDirection) == currentCell)
            {
				this.isDrag = true;
				return;
			}
		}

        this.isDrag = false;
	}

	private void EditCells (HexCell center)
    {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
			for (int x = centerX - r; x <= centerX + brushSize; x++)
            {
				SetCellValue(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
			for (int x = centerX - brushSize; x <= centerX + r; x++)
            {
				SetCellValue(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}

	void SetCellValue (HexCell cell)
    {
		if (cell)
        {
            if (activeTerrainTypeIndex >= 0)
            {
				cell.TerrainTypeIndex = activeTerrainTypeIndex;
			}
			if (applyElevation)
            {
				cell.Elevation = activeElevation;
			}
			if (applyWaterLevel)
            {
				cell.WaterLevel = activeWaterLevel;
			}
			if (applySpecialIndex)
            {
				cell.SpecialIndex = activeSpecialIndex;
			}
			if (applyUrbanLevel)
            {
				cell.UrbanLevel = activeUrbanLevel;
			}
			if (applyFarmLevel)
            {
				cell.FarmLevel = activeFarmLevel;
			}
			if (applyPlantLevel)
            {
				cell.PlantLevel = activePlantLevel;
			}
			if (riverMode == OptionalToggle.No)
            {
				cell.RemoveRiver();
			}
			if (roadMode == OptionalToggle.No)
            {
				cell.RemoveRoads();
			}
			if (walledMode != OptionalToggle.Ignore)
            {
				cell.Walled = walledMode == OptionalToggle.Yes;
			}

            if (isDrag)
            {
				HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
				if (otherCell)
                {
					if (riverMode == OptionalToggle.Yes)
                    {
						otherCell.SetOutgoingRiver(dragDirection);
					}
					if (roadMode == OptionalToggle.Yes)
                    {
						otherCell.AddRoad(dragDirection);
					}
				}
			}
		}
	}
}