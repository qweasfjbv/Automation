using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class Tile
{
    // building pos
    public int x, y;
    // building size
    public int sizeX, sizeY;
    // building id
    public int id;

    public Tile(int py = -1, int px = -1, int psizeY = -1, int psizeX = -1, int pid = -1)
    {
        SetTile(py, px, psizeY, psizeX, pid);
    }

    public void SetTile(int py, int px, int psizeY, int psizeX, int pid)
    {
        x = px; y = py; sizeX = psizeX; sizeY = psizeY; id = pid;
    }

    public void DeepCopy(Tile t)
    {
        x = t.x; y = t.y; sizeX = t.sizeX; sizeY = t.sizeY; id = t.id;
    }
};

public class MapManager
{
    private int buildID;

    private int mapSizeX;
    private int mapSizeY;
    private Tile[,] usingArea;

    public void Init()
    {
        buildID = 1;
        mapSizeX = mapSizeY = 4;

        usingArea = new Tile[mapSizeY, mapSizeX];

        for (int i = 0; i < mapSizeY; i++)
            for (int j = 0; j < mapSizeX; j++)
                usingArea[i, j] = new Tile();
    } 


    public void Build(int id, Vector2 pos, Vector2 size)
    {
        if (!BoundCheck(pos, size)) return;

        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                usingArea[Mathf.Abs((int)pos.y - i), (int)pos.x + j].SetTile((int)pos.y, (int)pos.x, (int)size.y, (int)size.x, id);
            }
        }
        buildID++;
        return;

    }

    public void Unbuild(Vector2 pos)
    {
        if (pos.x >= mapSizeX || Mathf.Abs(pos.y) >= mapSizeY) return;
        if (pos.x < 0 || pos.y > 0) return;

        Tile tile = new Tile();
        tile.DeepCopy(usingArea[Mathf.Abs((int)pos.y), (int)pos.x]);

        if (tile.id == -1) return;

        for (int i = 0; i < tile.sizeY; i++)
        {
            for (int j = 0; j < tile.sizeX; j++)
            {
                usingArea[Mathf.Abs(tile.y - i), tile.x + j].SetTile(-1, -1, -1, -1, -1);
            }
        }

    }

    // true: buildable, false: occupied
    public bool BoundCheck(Vector2 pos, Vector2 size)
    {
        if (pos.x + size.x > mapSizeX || Mathf.Abs(pos.y - size.y) > mapSizeY) return false;
        if (pos.x < 0 || pos.y > 0) return false;

        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (usingArea[Mathf.Abs((int)pos.y - i), (int)pos.x + j].id != -1) return false;
            }
        }

        return true;
    }
    
}
