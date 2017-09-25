using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Unit Manager
/// </summary>
public class HexGameUI : MonoBehaviour
{
    public HexGrid grid;

    HexCell currentCell;
    HexUnit selectedUnit;

    //////////////////////////////////////////////////////////////
    /////////////////// MapEditor Event Method ///////////////////
    //////////////////////////////////////////////////////////////
    public void SetEditMode(bool toggle)
    {
        enabled = !toggle;

        this.grid.ShowUI(!toggle);
        this.grid.ClearPath();
    }
    //////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////

    void Update()
    {
        // PathFinding
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoSelection();
            }
            else if (selectedUnit)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    DoMove();
                }
                else
                {
                    DoPathfinding();
                }
            }
        }


    }

    void DoSelection()
    {
        this.grid.ClearPath();
        UpdateCurrentCell();

        if (this.currentCell)
        {
            this.selectedUnit = this.currentCell.Unit;
        }
    }

    void DoPathfinding()
    {
        if (UpdateCurrentCell())
        {
            if (this.currentCell && selectedUnit.IsValidDestination(this.currentCell))
            {
                this.grid.FindPath(selectedUnit.Location, this.currentCell, 24);
            }
            else
            {
                this.grid.ClearPath();
            }
        }
    }

    void DoMove()
    {
        if (grid.HasPath)
        {
            //			selectedUnit.Location = currentCell;
            selectedUnit.Travel(grid.GetPath());
            grid.ClearPath();
        }
    }

    bool UpdateCurrentCell()
    {
        HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (cell != this.currentCell)
        {
            this.currentCell = cell;

            return true;
        }

        return false;
    }
}