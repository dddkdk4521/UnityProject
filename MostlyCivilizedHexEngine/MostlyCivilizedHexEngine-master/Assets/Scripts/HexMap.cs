﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;
using System.Linq;

public class HexMap : MonoBehaviour, IQPathWorld {

	// Use this for initialization
	void Start () {
        GenerateMap();
	}

    public bool AnimationIsPlaying = false;

    public delegate void CityCreatedDelegate ( City city, GameObject cityGO );
    public event CityCreatedDelegate OnCityCreated;

    void Update()
    {
        // TESTING: Hit spacebar to advance to next turn
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine( DoAllUnitMoves() );
        }
    }

    IEnumerator DoAllUnitMoves()
    {
        if(units != null)
        {
            foreach(Unit u in units)
            {
                yield return DoUnitMoves( u );
            }
        }
    }

    public IEnumerator DoUnitMoves( Unit u )
    {
        // Is there any reason we should check HERE if a unit should be moving?
        // I think the answer is no -- DoMove should just check to see if it needs
        // to do anything, or just return immediately.
        while( u.DoMove() )
        {
            Debug.Log("DoMove returned true -- will be called again.");
            // TODO: Check to see if an animation is playing, if so
            // wait for it to finish. 
            while(AnimationIsPlaying) {
                yield return null; // Wait one frame
            }

        }

    }

    public GameObject HexPrefab;

    public Mesh MeshWater;
    public Mesh MeshFlat;
    public Mesh MeshHill;
    public Mesh MeshMountain;

    public GameObject ForestPrefab;
    public GameObject JunglePrefab;

    public Material MatOcean;
    public Material MatPlains;
    public Material MatGrasslands;
    public Material MatMountains;
    public Material MatDesert;

    public GameObject UnitDwarfPrefab;
    public GameObject CityPrefab;

    // Tiles with height above whatever, is a whatever
    [System.NonSerialized] public float HeightMountain = 1f;
    [System.NonSerialized] public float HeightHill = 0.6f;
    [System.NonSerialized] public float HeightFlat = 0.0f;

    [System.NonSerialized] public float MoistureJungle = 1f;
    [System.NonSerialized] public float MoistureForest = 0.5f;
    [System.NonSerialized] public float MoistureGrasslands = 0f;
    [System.NonSerialized] public float MoisturePlains = -0.75f;

    [System.NonSerialized] public int NumRows = 30;
    [System.NonSerialized] public int NumColumns = 60;

    // TODO: Link up with the Hex class's version of this
    [System.NonSerialized] public bool AllowWrapEastWest = true;
    [System.NonSerialized] public bool AllowWrapNorthSouth = false;

    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;
    private Dictionary<GameObject, Hex> gameObjectToHexMap;

    // TODO: Separate unit list for each player
    private HashSet<Unit> units;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;
    public Unit[] Units
    {
        get { return units.ToArray(); }
    }

    private HashSet<City> cities;
    private Dictionary<City, GameObject> cityToGameObjectMap;
    public City[] Cities
    {
        get { return cities.ToArray(); }
    }

    public Hex GetHexAt(int x, int y)
    {
        if(hexes == null)
        {
            Debug.LogError("Hexes array not yet instantiated.");
            return null;
        }

        if(AllowWrapEastWest)
        {
            x = x % NumColumns;
            if(x < 0)
            {
                x += NumColumns;
            }
        }
        if(AllowWrapNorthSouth)
        {
            y = y % NumRows;
            if(y < 0)
            {
                y += NumRows;
            }
        }

        try {
            return hexes[x, y];
        }
        catch
        {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
    }

    public Hex GetHexFromGameObject(GameObject hexGO)
    {
        if( gameObjectToHexMap.ContainsKey(hexGO) )
        {
            return gameObjectToHexMap[hexGO];
        }

        return null;
    }

    public GameObject GetHexGO(Hex h)
    {
        if( hexToGameObjectMap.ContainsKey(h) )
        {
            return hexToGameObjectMap[h];
        }

        return null;
    }

    public Vector3 GetHexPosition(int q, int r)
    {
        Hex hex = GetHexAt(q, r);

        return GetHexPosition(hex);
    }

    public Vector3 GetHexPosition(Hex hex)
    {
        return hex.PositionFromCamera( Camera.main.transform.position, NumRows, NumColumns );
    }


    virtual public void GenerateMap()
    {
        // Generate a map filled with ocean

        hexes = new Hex[NumColumns, NumRows];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();
        gameObjectToHexMap = new Dictionary<GameObject, Hex>();

        for (int column = 0; column < NumColumns; column++)
        {
            for (int row = 0; row < NumRows; row++)
            {
                // Instantiate a Hex
                Hex h = new Hex( this, column, row );
                h.Elevation = -0.5f;

                hexes[ column, row ] = h;

                Vector3 pos = h.PositionFromCamera( 
                    Camera.main.transform.position, 
                    NumRows, 
                    NumColumns 
                );


                GameObject hexGO = (GameObject)Instantiate(
                    HexPrefab, 
                    pos,
                    Quaternion.identity,
                    this.transform
                );

                hexToGameObjectMap[h] = hexGO;
                gameObjectToHexMap[hexGO] = h;

                h.TerrainType = Hex.TERRAIN_TYPE.OCEAN;
                h.ElevationType = Hex.ELEVATION_TYPE.WATER;

                hexGO.name = string.Format("HEX: {0},{1}", column, row);
                hexGO.GetComponent<HexComponent>().Hex = h;
                hexGO.GetComponent<HexComponent>().HexMap = this;


            }
        }

        UpdateHexVisuals();

        //StaticBatchingUtility.Combine( this.gameObject );
    }

    public void UpdateHexVisuals()
    {
        for (int column = 0; column < NumColumns; column++)
        {
            for (int row = 0; row < NumRows; row++)
            {
                Hex h = hexes[column,row];
                GameObject hexGO = hexToGameObjectMap[h];

                HexComponent hexComp = hexGO.GetComponentInChildren<HexComponent>();
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();


                if(h.Elevation >= HeightFlat && h.Elevation < HeightMountain)
                {
                    if(h.Moisture >= MoistureJungle)
                    {
                        mr.material = MatGrasslands;
                        h.TerrainType = Hex.TERRAIN_TYPE.GRASSLANDS;
                        h.FeatureType = Hex.FEATURE_TYPE.RAINFOREST;

                        // Spawn trees
                        Vector3 p = hexGO.transform.position;
                        if(h.Elevation >= HeightHill)
                        {
                            p.y += 0.25f;
                        }


                        GameObject.Instantiate(JunglePrefab, p, Quaternion.identity, hexGO.transform);
                    }
                    else if(h.Moisture >= MoistureForest)
                    {
                        mr.material = MatGrasslands;
                        h.TerrainType = Hex.TERRAIN_TYPE.GRASSLANDS;
                        h.FeatureType = Hex.FEATURE_TYPE.FOREST;

                        // Spawn trees
                        Vector3 p = hexGO.transform.position;
                        if(h.Elevation >= HeightHill)
                        {
                            p.y += 0.25f;
                        }
                        GameObject.Instantiate(ForestPrefab, p, Quaternion.identity, hexGO.transform);
                    }
                    else if(h.Moisture >= MoistureGrasslands)
                    {
                        mr.material = MatGrasslands;
                        h.TerrainType = Hex.TERRAIN_TYPE.GRASSLANDS;
                    }
                    else if(h.Moisture >= MoisturePlains)
                    {
                        mr.material = MatPlains;
                        h.TerrainType = Hex.TERRAIN_TYPE.PLAINS;
                    }
                    else 
                    {
                        mr.material = MatDesert;
                        h.TerrainType = Hex.TERRAIN_TYPE.DESERT;
                    }
                }

                if(h.Elevation >= HeightMountain)
                {
                    mr.material = MatMountains;
                    mf.mesh = MeshMountain;
                    h.ElevationType = Hex.ELEVATION_TYPE.MOUNTAIN;
                }
                else if(h.Elevation >= HeightHill)
                {
                    h.ElevationType = Hex.ELEVATION_TYPE.HILL;
                    mf.mesh = MeshHill;
                    hexComp.VerticalOffset = 0.25f;
                }
                else if(h.Elevation >= HeightFlat)
                {
                    h.ElevationType = Hex.ELEVATION_TYPE.FLAT;
                    mf.mesh = MeshFlat;
                }
                else
                {
                    h.ElevationType = Hex.ELEVATION_TYPE.WATER;
                    mr.material = MatOcean;
                    mf.mesh = MeshWater;
                }

                hexGO.GetComponentInChildren<TextMesh>().text = 
                    string.Format("{0},{1}\n{2}", column, row, h.BaseMovementCost( false, false, false ));
                
            }
        }
    }

    public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();

        for (int dx = -range; dx < range-1; dx++)
        {
            for (int dy = Mathf.Max(-range+1, -dx-range); dy < Mathf.Min(range, -dx+range-1); dy++)
            {
                results.Add( GetHexAt(centerHex.Q + dx, centerHex.R + dy) );
            }
        }

        return results.ToArray();
    }

    public void SpawnUnitAt( Unit unit, GameObject prefab, int q, int r )
    {
        if(units == null)
        {
            units = new HashSet<Unit>();
            unitToGameObjectMap = new Dictionary<Unit, GameObject>();
        }

        Hex myHex = GetHexAt(q, r);
        GameObject myHexGO = hexToGameObjectMap[myHex];
        unit.SetHex(myHex);

        GameObject unitGO = (GameObject)Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);
        unit.OnObjectMoved += unitGO.GetComponent<UnitView>().OnUnitMoved;

        units.Add(unit);
        unitToGameObjectMap.Add(unit, unitGO);
    }

    public void SpawnCityAt( City city, GameObject prefab, int q, int r )
    {
        Debug.Log("SpawnCityAt");
        if(cities == null)
        {
            cities = new HashSet<City>();
            cityToGameObjectMap = new Dictionary<City, GameObject>();
        }

        Hex myHex = GetHexAt(q, r);
        GameObject myHexGO = hexToGameObjectMap[myHex];

        try
        {
            city.SetHex(myHex);
        }
        catch(UnityException e)
        {
            Debug.LogError(e.Message);
            return;
        }

        GameObject cityGO = (GameObject)Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);

        cities.Add(city);
        cityToGameObjectMap.Add(city, cityGO);

        if(OnCityCreated != null)
        {
            OnCityCreated(city, cityGO);
        }
    }
}
