using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HexCell : MonoBehaviour
{
	public HexCoordinates coordinates;
	public RectTransform uiRect;
	public HexGridChunk chunk;

    int terrainTypeIndex;

    int elevation = int.MinValue;
    int waterLevel;

    int urbanLevel, farmLevel, plantLevel;

    int specialIndex;

    int distance;

    int visibility;

    bool walled;

    bool hasIncomingRiver, hasOutgoingRiver;
    HexDirection incomingRiver, outgoingRiver;

    [SerializeField]
    HexCell[] neighbors;

    [SerializeField]
    bool[] roads;

    //////////////////////////////////////////////////
    ///////////////////// ProPrety ///////////////////
    //////////////////////////////////////////////////
    public int Index { get; set; }

	public int Elevation
    {
		get
        {
			return this.elevation;
		}
		set
        {
			if (this.elevation == value)
            {
				return;
			}

            this.elevation = value;

            RefreshPosition();
			ValidateRivers();

			for (int i = 0; i < roads.Length; i++)
            {
				if (roads[i] && GetElevationDifference((HexDirection)i) > 1)
                {
					SetRoad(i, false);
				}
			}

			Refresh();
		}
	}

	public int WaterLevel
    {
		get
        {
			return this.waterLevel;
		}
		set
        {
			if (this.waterLevel == value)
            {
				return;
			}

            this.waterLevel = value;

            ValidateRivers();
			Refresh();
		}
	}

	public bool IsUnderwater
    {
		get
        {
			return this.waterLevel > this.elevation;
		}
	}

	public bool HasIncomingRiver
    {
		get
        {
			return this.hasIncomingRiver;
		}
	}

	public bool HasOutgoingRiver
    {
		get
        {
			return this.hasOutgoingRiver;
		}
	}

	public bool HasRiver
    {
		get
        {
			return this.hasIncomingRiver || this.hasOutgoingRiver;
		}
	}

	public bool HasRiverBeginOrEnd
    {
		get
        {
			return this.hasIncomingRiver != this.hasOutgoingRiver;
		}
	}

	public HexDirection RiverBeginOrEndDirection
    {
		get
        {
			return this.hasIncomingRiver ? this.incomingRiver : this.outgoingRiver;
		}
	}

	public bool HasRoads
    {
		get
        {
			for (int i = 0; i < this.roads.Length; i++)
            {
				if (this.roads[i])
                {
					return true;
				}
			}
			return false;
		}
	}

	public HexDirection IncomingRiver
    {
		get
        {
			return this.incomingRiver;
		}
	}

	public HexDirection OutgoingRiver
    {
		get
        {
			return this.outgoingRiver;
		}
	}

	public Vector3 Position
    {
		get
        {
			return this.transform.localPosition;
		}
	}

	public float StreamBedY
    {
		get
        {
			return
				(this.elevation + HexMetrics.streamBedElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

	public float RiverSurfaceY
    {
		get
        {
			return
				(this.elevation + HexMetrics.waterElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

	public float WaterSurfaceY
    {
		get
        {
			return
				(this.waterLevel + HexMetrics.waterElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

	public int UrbanLevel
    {
		get
        {
			return this.urbanLevel;
		}
		set
        {
			if (this.urbanLevel != value)
            {
				this.urbanLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int FarmLevel
    {
		get
        {
			return this.farmLevel;
		}
		set
        {
			if (this.farmLevel != value)
            {
				this.farmLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int PlantLevel
    {
		get
        {
			return this.plantLevel;
		}
		set
        {
			if (this.plantLevel != value)
            {
				this.plantLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int SpecialIndex
    {
		get
        {
			return this.specialIndex;
		}
		set
        {
			if (this.specialIndex != value && !HasRiver)
            {
				this.specialIndex = value;

                RemoveRoads();
				RefreshSelfOnly();
			}
		}
	}

	public bool IsSpecial
    {
		get
        {
			return this.specialIndex > 0;
		}
	}

	public bool Walled
    {
		get
        {
			return this.walled;
		}
		set
        {
			if (this.walled != value)
            {
				this.walled = value;
				Refresh();
			}
		}
	}

	public int TerrainTypeIndex
    {
		get
        {
			return this.terrainTypeIndex;
		}
		set
        {
			if (this.terrainTypeIndex != value)
            {
				this.terrainTypeIndex = value;
				this.ShaderData.RefreshTerrain(this);
			}
		}
	}

	public bool IsVisible
    {
		get
        {
			return this.visibility > 0;
		}
	}

	public int Distance
    {
		get
        {
			return this.distance;
		}
		set
        {
			this.distance = value;
		}
	}

	public HexUnit Unit { get; set; }

	public HexCell PathFrom { get; set; }

	public int SearchHeuristic { get; set; }

	public int SearchPriority
    {
		get
        {
			return distance + SearchHeuristic;
		}
	}

	public int SearchPhase { get; set; }

	public HexCell NextWithSamePriority { get; set; }

	public HexCellShaderData ShaderData { get; set; }
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////

    public void IncreaseVisibility ()
    {
		this.visibility += 1;
		if (this.visibility == 1)
        {
		    ShaderData.RefreshVisibility(this);
		}
	}

	public void DecreaseVisibility ()
    {
		this.visibility -= 1;
		if (this.visibility == 0)
        {
			ShaderData.RefreshVisibility(this);
		}
	}

	public HexCell GetNeighbor (HexDirection direction)
    {
		return this.neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell)
    {
		this.neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public HexEdgeType GetEdgeType (HexDirection direction)
    {
		return HexMetrics.GetEdgeType(
			this.elevation, neighbors[(int)direction].elevation
		);
	}

	public HexEdgeType GetEdgeType (HexCell otherCell)
    {
		return HexMetrics.GetEdgeType(
			this.elevation, otherCell.elevation
		);
	}

	public bool HasRiverThroughEdge (HexDirection direction)
    {
		return
			this.hasIncomingRiver && this.incomingRiver == direction ||
			this.hasOutgoingRiver && this.outgoingRiver == direction;
	}

	public void RemoveRiver ()
    {
		RemoveOutgoingRiver();
		RemoveIncomingRiver();
	}

    public void RemoveIncomingRiver ()
    {
		if (!hasIncomingRiver)
        {
			return;
		}
		this.hasIncomingRiver = false;
		this.RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(incomingRiver);
        {
		    neighbor.hasOutgoingRiver = false;
		    neighbor.RefreshSelfOnly();
        }
	}

	public void RemoveOutgoingRiver ()
    {
		if (!hasOutgoingRiver)
        {
			return;
		}

		this.hasOutgoingRiver = false;
		this.RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(outgoingRiver);
        {
		    neighbor.hasIncomingRiver = false;
		    neighbor.RefreshSelfOnly();
        }
	}

	public void SetOutgoingRiver (HexDirection direction)
    {
		if (this.hasOutgoingRiver && this.outgoingRiver == direction)
        {
			return;
		}

		HexCell neighbor = GetNeighbor(direction);
		if (!IsValidRiverDestination(neighbor))
        {
			return;
		}

		RemoveOutgoingRiver();
		if (hasIncomingRiver && incomingRiver == direction)
        {
			RemoveIncomingRiver();
		}
		this.hasOutgoingRiver = true;
		this.outgoingRiver = direction;
		this.specialIndex = 0;

		neighbor.RemoveIncomingRiver();
		neighbor.hasIncomingRiver = true;
		neighbor.incomingRiver = direction.Opposite();
		neighbor.specialIndex = 0;

		SetRoad((int)direction, false);
	}

	public bool HasRoadThroughEdge (HexDirection direction)
    {
		return roads[(int)direction];
	}

	public void AddRoad (HexDirection direction)
    {
		if (
			!roads[(int)direction] && !HasRiverThroughEdge(direction) &&
			!IsSpecial && !GetNeighbor(direction).IsSpecial &&
			GetElevationDifference(direction) <= 1
		) {
			SetRoad((int)direction, true);
		}
	}

	public void RemoveRoads ()
    {
		for (int i = 0; i < this.neighbors.Length; i++)
        {
			if (this.roads[i])
            {
				SetRoad(i, false);
			}
		}
	}

	public int GetElevationDifference (HexDirection direction)
    {
		int difference = this.elevation - GetNeighbor(direction).elevation;

        return difference >= 0 ? difference : -difference;
	}

	bool IsValidRiverDestination (HexCell neighbor)
    {
		return neighbor && (
			elevation >= neighbor.elevation || waterLevel == neighbor.elevation
		);
	}

	void ValidateRivers ()
    {
		if (
			this.hasOutgoingRiver &&
			!IsValidRiverDestination(GetNeighbor(outgoingRiver))
		) {
			RemoveOutgoingRiver();
		}
		if (
			this.hasIncomingRiver &&
			!GetNeighbor(incomingRiver).IsValidRiverDestination(this)
		) {
			RemoveIncomingRiver();
		}
	}

	void SetRoad (int index, bool state)
    {
		this.roads[index] = state;
		this.neighbors[index].roads[(int)((HexDirection)index).Opposite()] = state;
		this.neighbors[index].RefreshSelfOnly();

		RefreshSelfOnly();
	}

	void RefreshPosition ()
    {
		Vector3 position = this.transform.localPosition;
        {
            position.y = elevation * HexMetrics.elevationStep;
		    position.y +=
			    (HexMetrics.SampleNoise(position).y * 2f - 1f) *
			    HexMetrics.elevationPerturbStrength;
        }
		this.transform.localPosition = position;

		Vector3 uiPosition = this.uiRect.localPosition;
        uiPosition.z = -position.y;
		this.uiRect.localPosition = uiPosition;
	}

	void Refresh ()
    {
		if (this.chunk)
        {
			this.chunk.Refresh();
			for (int i = 0; i < neighbors.Length; i++)
            {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != this.chunk)
                {
					neighbor.chunk.Refresh();
				}
			}

            if (this.Unit)
            {
				Unit.ValidateLocation();
			}
		}
	}

	void RefreshSelfOnly ()
    {
		chunk.Refresh();

        if (this.Unit)
        {
			this.Unit.ValidateLocation();
		}
	}

	public void Save (BinaryWriter writer)
    {
        // Save Propertys
        writer.Write((byte)terrainTypeIndex);
		writer.Write((byte)elevation);
		writer.Write((byte)waterLevel);
		writer.Write((byte)urbanLevel);
		writer.Write((byte)farmLevel);
		writer.Write((byte)plantLevel);
		writer.Write((byte)specialIndex);
		writer.Write(walled);

		if (this.hasIncomingRiver)
        {
			writer.Write((byte)(incomingRiver + 128));
		}
		else
        {
			writer.Write((byte)0);
		}

		if (this.hasOutgoingRiver)
        {
			writer.Write((byte)(outgoingRiver + 128));
		}
		else
        {
			writer.Write((byte)0);
		}

		int roadFlags = 0;
		for (int i = 0; i < roads.Length; i++)
        {
			if (roads[i])
            {
				roadFlags |= 1 << i;
			}
		}

		writer.Write((byte)roadFlags);
	}

	public void Load (BinaryReader reader)
    {
        // Load Protertys
        terrainTypeIndex = reader.ReadByte();
		ShaderData.RefreshTerrain(this);
		elevation = reader.ReadByte();
		RefreshPosition();
		waterLevel = reader.ReadByte();
		urbanLevel = reader.ReadByte();
		farmLevel = reader.ReadByte();
		plantLevel = reader.ReadByte();
		specialIndex = reader.ReadByte();
		walled = reader.ReadBoolean();

		byte riverData = reader.ReadByte();
		if (riverData >= 128)
        {
			hasIncomingRiver = true;
			incomingRiver = (HexDirection)(riverData - 128);
		}
		else
        {
			hasIncomingRiver = false;
		}

		riverData = reader.ReadByte();
		if (riverData >= 128)
        {
			hasOutgoingRiver = true;
			outgoingRiver = (HexDirection)(riverData - 128);
		}
		else
        {
			hasOutgoingRiver = false;
		}

		int roadFlags = reader.ReadByte();
		for (int i = 0; i < roads.Length; i++)
        {
			roads[i] = (roadFlags & (1 << i)) != 0;
		}
	}

	public void SetLabel (string text)
    {
		UnityEngine.UI.Text label = uiRect.GetComponent<Text>();
		label.text = text;
	}

	public void DisableHighlight ()
    {
		Image highlight = uiRect.GetChild(0).GetComponent<Image>();
		highlight.enabled = false;
	}

	public void EnableHighlight (Color color)
    {
		Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        {
            highlight.color = color;
		    highlight.enabled = true;
        }
	}
}