using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile
{
    // building pos
    public int x, y;
    // building id
    public int id;

    public int rot;

    // -1 : unbuildable, 0:buildable, >0 : veinId
    public int terrainInfo;

    public GameObject building;
    public Tile(int py = -1, int px = -1, int pid = -1, int rot = -1, GameObject building = null, int terrainInfo = 0)
    {
        SetTile(py, px, pid, rot , building, terrainInfo);
        this.building = building;
    }

    public void SetTile(int py, int px, int pid, int rot, GameObject factory = null, int terrainInfo = 0)
    {
        x = px; y = py; id = pid; this.rot = rot; this.building = factory;
        this.terrainInfo = terrainInfo;
    }

    public void DeepCopy(Tile t)
    {
        x = t.x; y = t.y; id = t.id; rot = t.rot; building = t.building;
    }

    public void EraseBuilding()
    {
        x = -1; y = -1; id = -1; rot = -1;
        building = null;
    }

    public void ClearBuilding()
    {
        if (building != null) GameObject.Destroy(building);
        EraseBuilding();
        terrainInfo = 0;
    }
};



public class MapManager
{
    private int mapSizeX;
    private int mapSizeY;
    public int MapSizeX { get => mapSizeX; }
    public int MapSizeY { get => mapSizeY; }
    private Tile[,] usingArea;
    private Tile nullTile = null;

    private Vector2 start = new Vector2(0, 0);
    private Vector2 end = new Vector2(0, 0);

    private readonly bool[,,] VEINPOS = new bool[4, 3, 3]
    {
        {
            {true, true, false},
            {true, true, false},
            {false, false, false},
        },
        {

            {true, true, false},
            {false, true, false},
            {false, false, true},
        },
        {
            {false, false, false},
            {true, false, false},
            {true, true, true},
        },
        {
            {true, false, false},
            {true, true, false},
            {false, true, false},
        }
    };

    public void Init(int mapSize)
    {
        mapSizeX = mapSizeY = mapSize;

        usingArea = new Tile[mapSizeY, mapSizeX];

        for (int i = 0; i < mapSizeY; i++)
            for (int j = 0; j < mapSizeX; j++)
                usingArea[i, j] = new Tile();


    }


    public void Build(int id, Vector2 pos, Vector2 size, int rot)
    {
        if (!BoundCheck(pos, size, rot)) return;
        GameObject tmpGo;
        Vector2 buildPos = new Vector2(pos.x + size.x / 2, pos.y - size.y / 2);

        if (id == 101)
        {
            tmpGo = Managers.Pool.Pop(buildPos, new Vector3(0, 0, -1 * rot * 90));
        }
        else if(Managers.Resource.GetBuildingData(id).Prefab.GetComponentInChildren<Transport>() != null)
        {
            tmpGo = GameObject.Instantiate(Managers.Resource.GetBuildingData(id).Prefab, buildPos, Quaternion.Euler(0, 0, -1 * rot * 90));
        }
        else
        {
            tmpGo = GameObject.Instantiate(Managers.Resource.GetBuildingData(id).Prefab, buildPos, Quaternion.Euler(0, 0, 0));
        }

        for (int i = (int)start.y; i < (int)end.y; i++)
        {
            for (int j = (int)start.x; j < (int)end.x; j++)
            {
                usingArea[i, j] = new Tile((int)pos.y, (int)pos.x, id, rot, tmpGo, usingArea[i, j].terrainInfo);
            }
        }



        InvokeNearbyBelts(pos);

        return;

    }


    public void Unbuild(Vector2 pos)
    {
        if (pos.x >= mapSizeX || Mathf.Abs(pos.y) >= mapSizeY) return;
        if (pos.x < 0 || pos.y > 0) return;

        Tile tile = new Tile();
        tile.DeepCopy(usingArea[Mathf.Abs((int)pos.y), (int)pos.x]);

        if (tile.id == -1) return;

        CalDir(new Vector2(tile.x, tile.y), Managers.Resource.GetBuildingData(tile.id).Size, tile.rot);


        //pooling 구현
        if (tile.id == 101)
        {
            Managers.Pool.Push(tile.building);
            tile.building = null;
        }
        else
        {
            GameObject.Destroy(usingArea[Mathf.Abs(tile.y), tile.x].building);
        }
        //

        for (int i = (int)start.y; i < end.y; i++)
        {
            for (int j = (int)start.x; j < end.x; j++)
            {
                usingArea[i, j].EraseBuilding();
            }
        }

    }

    // true: buildable, false: occupied
    public bool BoundCheck(Vector2 pos, Vector2 size, int rot)
    {
        CalDir(pos, size, rot);

        if (start.x < 0 || start.y < 0) return false;
        if (end.x > mapSizeX || end.y > mapSizeY) return false;

        for (int i = (int)start.y; i < (int)end.y; i++)
        {
            for (int j = (int)start.x; j < (int)end.x; j++)
            {
                if (usingArea[i, j].id != -1) return false;
            }
        }

        return true;
    }


    private void CalDir(Vector2 pos, Vector2 size, int rot)
    {
        switch (rot)
        {
            case 0:
                start.y = -1 * pos.y; start.x = pos.x;
                end.y = -1 * pos.y + size.y; end.x = pos.x + size.x;
                break;
            case 1:
                start.y = -1 * pos.y; start.x = pos.x - size.y + 1;
                end.y = -1 * pos.y + size.x; end.x = pos.x + 1;
                break;
            case 2:
                start.y = -1 * pos.y + size.y - 1; start.x = pos.x - size.x + 1;
                end.y = -1 * pos.y + 1; end.x = pos.x + 1;
                break;
            case 3:
                start.y = -1 * pos.y - size.x + 1; start.x = pos.x;
                end.y = -1 * pos.y + 1; end.x = pos.x + size.y;
                break;
            default:
                break;
        }


        return;
    }

    readonly int[] DY = { -1, 0, 1, 0 };
    readonly int[] DX = { 0, 1, 0, -1 };
    int tmpy, tmpx;
    public GameObject FindBuildingFromBelt(BuildingBase bbase, Vector2 pos, int id, ref int outDir)
    {
        pos = new Vector2(Mathf.Floor(pos.x), Mathf.Ceil(pos.y));
        int tmpDir = usingArea[Mathf.Abs((int)pos.y), (int)pos.x].rot;
        tmpDir = (outDir + 3) % 4;

        for (int i = 0; i < 4; i++)
        {
            tmpDir = (tmpDir + 1) % 4;
            tmpy = Mathf.Abs((int)pos.y) + DY[tmpDir]; tmpx = (int)pos.x + DX[tmpDir];

            // 방향 반대면 제외
            if (usingArea[Mathf.Abs((int)pos.y), (int)pos.x].rot == (tmpDir + 2) % 4) continue;
            if (tmpy < 0 || tmpx < 0 || tmpy >= mapSizeY || tmpx >= mapSizeX) continue;
            if (usingArea[tmpy, tmpx].id == -1) continue;

            if (IsCanConnect(tmpy, tmpx, tmpDir))
            {
                outDir = tmpDir;
                if (usingArea[tmpy, tmpx].building.GetComponent<Belt>() != null)
                {
                    usingArea[tmpy, tmpx].building.GetComponent<Belt>().SetPrevBuilding(bbase.GetComponent<BuildingBase>());
                }
                return usingArea[tmpy, tmpx].building;

            }
        }
        return null;
    }

    public GameObject FindBeltFromBuilding(BuildingBase bbase, Vector2 pos, int dir = 0)
    {
        pos = new Vector2(Mathf.Floor(pos.x), Mathf.Ceil(pos.y));
        dir = (usingArea[Mathf.Abs((int)pos.y), (int)pos.x].rot + dir) % 4;

        tmpy = Mathf.Abs((int)pos.y) + DY[dir]; tmpx = (int)pos.x + DX[dir];
        if (tmpy < 0 || tmpx < 0 || tmpy >= mapSizeY || tmpx >= mapSizeX) return null;


        if (usingArea[tmpy, tmpx].id == 101 && dir == usingArea[tmpy, tmpx].rot)
        {
            if(usingArea[tmpy, tmpx].building.GetComponent<Belt>() != null) {
                usingArea[tmpy, tmpx].building.GetComponent<Belt>().SetPrevBuilding(bbase.GetComponent<BuildingBase>());
            }
            return usingArea[tmpy, tmpx].building;
        }

        return null;
    }

    private void InvokeNearbyBelts(Vector2 pos)
    {
        int ty, tx;
        for(int i=0; i<4; i++)
        {
            ty = -(int)pos.y + DY[i]; tx = (int)pos.x + DX[i];

            if (ty < 0 || tx < 0 || ty >= mapSizeY || tx >= mapSizeX) continue;
            if (usingArea[ty, tx].id == -1 || usingArea[ty, tx].building.GetComponent<Belt>() == null) continue;

            usingArea[ty, tx].building.GetComponent<Belt>().InvokeBelt();
        }
    }

    private bool IsCanConnect(int y, int x, int com)
    {
        BuildingData bd = Managers.Resource.GetBuildingData(usingArea[y, x].id);

        for (int i = 0; i < bd.InputDirs.Count; i++)
        {
            if ((bd.InputDirs[i] + 2 + usingArea[y, x].rot) % 4 == com)
            {
                return true;
            }
        }


        return false;
    }

    //  BoundCheck;

    public ref Tile GetTileOnPoint(Vector2 pos)
    {
        var tmpV = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.Abs(Mathf.CeilToInt(pos.y)));

        if (tmpV.x >= mapSizeX || tmpV.y >= mapSizeY || tmpV.x < 0 || tmpV.y < 0) return ref nullTile;

        return ref usingArea[tmpV.y, tmpV.x];
    }


    public void GenerateVeinsOnMap()
    {
        const int veinStart = 1;
        const int veinEnd = 6;
        const int veinCount = 3;
        int rdIdx, rdY, rdX;
        int tmpY, tmpX;
        bool isCanBuild = true;
        GameObject veinPrefab = Managers.Resource.GetVeinData(1).Prefab;

        for (int i = veinStart; i <= veinEnd; i++)
        {
            for(int j=0; j< veinCount; j++)
            {
                rdIdx = UnityEngine.Random.Range(0, 4);

                while (true)
                {
                    rdY = UnityEngine.Random.Range(0, mapSizeY - 3);
                    rdX = UnityEngine.Random.Range(0, mapSizeX - 3);

                    isCanBuild = true;
                    for (int r = 0; r < 3; r++)
                    {
                        for (int s = 0; s < 3; s++)
                        {
                            tmpY = rdY + r; tmpX = rdX + s;
                            if (usingArea[tmpY, tmpX].terrainInfo != 0 && !VEINPOS[rdIdx, r, s])
                            {
                                isCanBuild = false;
                                break;
                            }
                        }
                        if (!isCanBuild) break;
                    }

                    if (isCanBuild)
                    {
                        for (int r = 0; r < 3; r++)
                        {
                            for (int s = 0; s < 3; s++)
                            {
                                tmpY = rdY + r; tmpX = rdX + s;
                                if (VEINPOS[rdIdx, r, s])
                                {
                                    BuildVein(veinPrefab, tmpY, tmpX, i);
                                }
                            }
                        }

                        break;
                    }
                }

            }
        }

    }

    private void BuildVein(GameObject obj, int y, int x, int veinId)
    {
        var tmp = GameObject.Instantiate(obj, new Vector3(x + 0.5f, -y - 0.5f, 0), Quaternion.identity);
        tmp.GetComponent<SpriteRenderer>().sprite = Managers.Resource.GetVeinData(veinId).VeinImage;
        usingArea[y, x].terrainInfo = veinId;
    }

    public void GenerateTutorialMap()
    {

        usingArea[2, 2].terrainInfo = 1;

        usingArea[4, 2].terrainInfo = 4;
        usingArea[5, 2].terrainInfo = 4;

        usingArea[7, 2].terrainInfo = 2;

        usingArea[9, 2].terrainInfo = 3;

        usingArea[11, 2].terrainInfo = 4;
        usingArea[12, 2].terrainInfo = 4;

        usingArea[14, 2].terrainInfo = 5;

        usingArea[16, 2].terrainInfo = 6;
        usingArea[17, 2].terrainInfo = 6;
    }

    public void Clear() {
        if (usingArea != null)
        {
            for (int i = 0; i < mapSizeX; i++)
            {
                for (int j = 0; j < mapSizeY; j++)
                {
                    usingArea[j, i].ClearBuilding();
                }
            }
            usingArea = null;
        }

        GC.Collect();
    }

}
