using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Tile
{
    // building pos
    public int x, y;
    // building size
    public float sizeX, sizeY;
    // building id
    public int id;

    public int rot;

    public int veinId;
    public GameObject building;
    public Tile(int py = -1, int px = -1, float psizeY = -1, float psizeX = -1, int pid = -1, int rot = -1, GameObject building = null, int veinId = -1)
    {
        SetTile(py, px, psizeY, psizeX, pid, rot , building, veinId);
        this.building = building;
    }

    public void SetTile(int py, int px, float psizeY, float psizeX, int pid, int rot, GameObject factory = null, int veinId = -1)
    {
        x = px; y = py; sizeX = psizeX; sizeY = psizeY; id = pid; this.rot = rot; this.building = factory;
        this.veinId = veinId;
    }

    public void DeepCopy(Tile t)
    {
        x = t.x; y = t.y; sizeX = t.sizeX; sizeY = t.sizeY; id = t.id; rot = t.rot; building = t.building;
    }

    public void EraseBuilding()
    {
        x = -1; y = -1; sizeX = -1; sizeY = -1; id = -1; rot = -1;
        building = null;
    }
};

public class MapManager
{
    private int mapSizeX;
    private int mapSizeY;
    private Tile[,] usingArea;
    public Tile[,] UsingArea { get => usingArea; }

    private Vector2 start = new Vector2(0, 0);
    private Vector2 end = new Vector2(0, 0);

    public void Init()
    {
        mapSizeX = mapSizeY = 10;

        usingArea = new Tile[mapSizeY, mapSizeX];

        for (int i = 0; i < mapSizeY; i++)
            for (int j = 0; j < mapSizeX; j++)
                usingArea[i, j] = new Tile();

        // Vein 생성 필요

        GenerateVeinsOnMap();
    } 



    public void Build(int id, Vector2 pos, Vector2 size, Vector2 buildPos, int rot)
    {
        if (!BoundCheck(pos, size, rot)) return;

        GameObject tmpGo = GameObject.Instantiate(Managers.Resource.GetBuildingData(id).Prefab, buildPos, Quaternion.Euler(0, 0, -1 * rot * 90));
        for (int i = (int)start.y; i < (int)end.y; i++)
        {
            for (int j = (int)start.x; j < (int)end.x; j++)
            {
                usingArea[i, j] = new Tile((int)pos.y, (int)pos.x, size.y, size.x, id, rot, tmpGo, usingArea[i, j].veinId);
            }
        }
        
        //usingArea[Mathf.Abs((int)pos.y), (int)pos.x].building = 
        // pooling 구현



        return;

    }

    public void Unbuild(Vector2 pos)
    {
        if (pos.x >= mapSizeX || Mathf.Abs(pos.y) >= mapSizeY) return;
        if (pos.x < 0 || pos.y > 0) return;

        Tile tile = new Tile();
        tile.DeepCopy(usingArea[Mathf.Abs((int)pos.y), (int)pos.x]);

        if (tile.id == -1) return;

        CalDir(new Vector2(tile.x, tile.y), new Vector2(tile.sizeX, tile.sizeY), tile.rot);


        //pooling 구현
        GameObject.Destroy(usingArea[Mathf.Abs(tile.y), tile.x].building);
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
        if(end.x > mapSizeX || end.y > mapSizeY) return false;

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
                end.y = -1* pos.y + 1; end.x = pos.x + 1;
                break;
            case 3:
                start.y = -1 *  pos.y - size.x + 1; start.x = pos.x;
                end.y = -1* pos.y + 1; end.x = pos.x + size.y;
                break;
            default:
                break;
        }
        
        
        return;
    }

    readonly int[] DY = { -1, 0, 1, 0 };
    readonly int[] DX = { 0, 1, 0, -1 };
    int tmpy, tmpx;
    public GameObject FindBuildingFromBelt(Vector2 pos, int id, ref int outDir)
    {
        pos = new Vector2(Mathf.Floor(pos.x), Mathf.Ceil(pos.y));
        for (int i= 0; i<4; i++)
        {

            tmpy = Mathf.Abs((int)pos.y) + DY[i]; tmpx = (int)pos.x + DX[i];
            
            // 방향 반대면 제외
            if (usingArea[Mathf.Abs((int)pos.y), (int)pos.x].rot == (i+2)%4) continue;
            if (tmpy < 0 || tmpx < 0 || tmpy >= mapSizeY || tmpx >= mapSizeX) continue;
            if (usingArea[tmpy, tmpx].id == -1) continue;

            if (IsCanConnect(tmpy, tmpx, i)) {
                outDir = i;    
                return usingArea[tmpy, tmpx].building; 

            }

        }
        return null;
    }

    public GameObject FindBeltFromBuilding(Vector2 pos, int dir = 0)
    {
        pos = new Vector2(Mathf.Floor(pos.x), Mathf.Ceil(pos.y));
        dir = (usingArea[Mathf.Abs((int)pos.y), (int)pos.x].rot + dir) % 4;

        tmpy = Mathf.Abs((int)pos.y) + DY[dir]; tmpx = (int)pos.x + DX[dir];
        if (tmpy < 0 || tmpx < 0 || tmpy >= mapSizeY || tmpx >= mapSizeX) return null;


        if (usingArea[tmpy, tmpx].id == 101 && dir == usingArea[tmpy, tmpx].rot)
        {
            return usingArea[tmpy, tmpx].building;
        }

        return null;
    }

    private bool IsCanConnect(int y, int x, int com)
    {
        BuildingData bd = Managers.Resource.GetBuildingData(usingArea[y, x].id);

        for (int i = 0; i < bd.Inputs.Count; i++)
        {
            if ((bd.Inputs[i] + 2 + usingArea[y, x].rot)%4 == com)
            {
                return true;
            }
        }


        return false;
    }

    private void GenerateVeinsOnMap()
    {

        usingArea[1, 0].veinId = 13;
        usingArea[1, 1].veinId = 11;
        usingArea[0, 1].veinId = 15;

        // TODO : Sprite 생성

    }

}
