using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using System.Runtime.CompilerServices;

public static class AESManager {
    private static byte[] key = Encoding.UTF8.GetBytes("1234qwer5678asdf");
    private static byte[] iv = Encoding.UTF8.GetBytes("1234qwer5678asdf");

    public static string EncryptString(string s)
    {
        using (Aes aesALg = Aes.Create())
        {
            aesALg.Key = key;
            aesALg.IV = iv;

            ICryptoTransform encryptor = aesALg.CreateEncryptor(aesALg.Key, aesALg.IV);
            byte[] encrypted = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(s), 0, s.Length);

            return System.Convert.ToBase64String(encrypted);
        }
    }

    public static string DecryptString(string cipherText)
    {
        byte[] buffer = System.Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}


[Serializable]
public class TileData {
    public int x, y;
    public int id;
    public int rot;
    public int terrainInfo;

    public int itemId;
    public int outDir;
}

[Serializable]
public class TileDatas {
    public List<TileData> tileData;
}

public class InvenItemDatas {
    public int[] invenItemData;
}
public class InvenBuildingDatas {
    public int[] invenBuildingData;
}

public class QuestProgressDatas {
    public int successId;
    public int inProgressId;
    public int[] questItems;
    public float[] populations;
}


public class DataManager
{

    private TileDatas tileDatas;
    private string path;
    private string mapFileName = "/UserMapData.json";
    private string invenItemFileName = "/UserInvenItemData.json";
    private string invenBuildingFileName = "/UserInvenBuildingData.json";
    private string questProgressFileName = "/UserQuestProgressData.json";

    InvenItemDatas invenItemDatas = new InvenItemDatas();
    InvenBuildingDatas invenBuildingDatas = new InvenBuildingDatas();
    QuestProgressDatas questProgressDatas = new QuestProgressDatas();
    public ref QuestProgressDatas QuestProgress { get => ref questProgressDatas; }

    public delegate void UpdateDelegate<T1>(T1 a);
    public UpdateDelegate<int> itemUpdateDelegate;
    public UpdateDelegate<int> questUpdateDelegate;
    public UpdateDelegate<int> questPopulationDelegate;

    public void Init()
    {
        tileDatas = new TileDatas();
        tileDatas.tileData = new List<TileData>();


        path = Application.persistentDataPath;

        Debug.Log(path);

        if (Managers.Scene.CurScene.GetComponent<GameScene>() != null)
        {
            LoadMap();
            LoadQuestProgress();
        }
        else if (Managers.Scene.CurScene.GetComponent<TutorialScene>() != null)
        {
            LoadTutorialMap();
        }


    }

    public void SaveAll()
    {
        SaveMap();
        SaveInven();
        SaveQuestProgress();
    }
    public void DeleteAll()
    {
        DeleteMap();
        DeleteInvenData();
        DeleteQuestProgress();
    }

    private void LoadTutorialMap()
    {
        Managers.Map.GenerateTutorialMap();
    }

    // save on UserMapData.json
    // posx, posy, buildingid, rot,
    // if belt -> outdir, itemIdOnBelt

    public void SaveMap()
    {
        tileDatas = new TileDatas();
        tileDatas.tileData = new List<TileData>();

        for (int i = 0; i < Managers.Map.MapSizeX; i++)
        {
            for (int j = 0; j < Managers.Map.MapSizeY; j++)
            {
                Tile tmp = Managers.Map.GetTileOnPoint(new Vector2(i, j));

                if (tmp == null) continue;
                if (tmp.id == -1 && tmp.terrainInfo == 0) continue;

                if (tmp.id == 101)
                {
                    tileDatas.tileData.Add(new TileData
                    {
                        x = i,
                        y = j,
                        id = tmp.id,
                        rot = tmp.rot,
                        terrainInfo = tmp.terrainInfo,
                        itemId = tmp.building.GetComponent<Belt>().BeltItemId,
                        outDir = tmp.building.GetComponent<Belt>().OutDir
                    });
                }
                else if (tmp.building != null && tmp.building.GetComponent<Production>() != null)
                {
                    tileDatas.tileData.Add(new TileData
                    {
                        x = i,
                        y = j,
                        id = tmp.id,
                        rot = tmp.rot,
                        terrainInfo = tmp.terrainInfo,
                        itemId = tmp.building.GetComponent<Production>().OutputItemId,
                        outDir = 0
                    });
                }
                else
                {
                    tileDatas.tileData.Add(new TileData
                    {
                        x = i,
                        y = j,
                        id = tmp.id,
                        rot = tmp.rot,
                        terrainInfo = tmp.terrainInfo,
                        itemId = -1,
                        outDir = 0
                    }) ;
                }

            }
        }

        DeleteMap();

        File.AppendAllText(path + mapFileName, AESManager.EncryptString(JsonUtility.ToJson(tileDatas)));

        tileDatas.tileData.Clear();

    }

    public bool LoadMap()
    {


        if(!File.Exists(path+mapFileName)) {
            Managers.Map.GenerateVeinsOnMap();
            return false;
        }
        tileDatas = JsonUtility.FromJson<TileDatas>(AESManager.DecryptString(File.ReadAllText(path + mapFileName)));

        if (tileDatas == null || tileDatas.tileData == null) return false;


        for (int i = 0; i < tileDatas.tileData.Count; i++)
        {
            var tile = Managers.Map.GetTileOnPoint(new Vector2(tileDatas.tileData[i].x, tileDatas.tileData[i].y));

            tile.terrainInfo = tileDatas.tileData[i].terrainInfo;

            if(tile.terrainInfo >= 1 && tile.terrainInfo <= 7)
            {
                Managers.Map.BuildVein(tileDatas.tileData[i].y, tileDatas.tileData[i].x, tile.terrainInfo);
            }

            if (tileDatas.tileData[i].id != -1)
            {
                Managers.Map.Build(tileDatas.tileData[i].id, new Vector2(tileDatas.tileData[i].x, -tileDatas.tileData[i].y),
                    new Vector2(1, 1), tileDatas.tileData[i].rot);
            }
        }

        for (int i = 0; i < tileDatas.tileData.Count; i++)
        {
            var tile = Managers.Map.GetTileOnPoint(new Vector2(tileDatas.tileData[i].x, tileDatas.tileData[i].y));

            if (tile.building == null) continue;

            if (tile.building.GetComponent<Belt>() != null)
            {
                tile.building.GetComponent<Belt>().SetBeltId(tileDatas.tileData[i].itemId);
                tile.building.GetComponent<Belt>().SetOutdir(tileDatas.tileData[i].outDir);
            }
            else if (tile.building.GetComponent<Production>() != null)
            {
                tile.building.GetComponent<Production>().Init(tileDatas.tileData[i].itemId);
            }
        }

        tileDatas.tileData.Clear();

        return true;
    }

    private void DeleteMap()
    {
        if (File.Exists(path + mapFileName))
        {
            File.Delete(path + mapFileName);
        }
    }

    public ref int[] LoadInvenItem() {

        if (!File.Exists(path + invenItemFileName))
        {

            invenItemDatas.invenItemData = new int[Managers.Resource.GetItemCount()];
            for (int i = 0; i < Managers.Resource.GetItemCount(); i++)
            {
                invenItemDatas.invenItemData[i] = 0;
            }

        }
        else
        {
            invenItemDatas = JsonUtility.FromJson<InvenItemDatas>(AESManager.DecryptString(File.ReadAllText(path + invenItemFileName)));
        }

        if (invenItemDatas == null) invenItemDatas = new InvenItemDatas();

        if (invenItemDatas.invenItemData == null || invenItemDatas.invenItemData.Length <= 0)
        {
            invenItemDatas.invenItemData = new int[Managers.Resource.GetItemCount()];
            for (int i = 0; i < Managers.Resource.GetItemCount(); i++)
            {
                invenItemDatas.invenItemData[i] = 0;
            }


        }

        return ref invenItemDatas.invenItemData;
    }
    public ref int[] LoadInvenBuilding()
    {

        if (!File.Exists(path + invenBuildingFileName))
        {

            invenBuildingDatas.invenBuildingData = new int[Managers.Resource.GetBuildingCount()];
            for (int i = 0; i < Managers.Resource.GetBuildingCount(); i++)
            {
                invenBuildingDatas.invenBuildingData[i] = 0;
            }

        }
        else
        {
            invenBuildingDatas = JsonUtility.FromJson<InvenBuildingDatas>(AESManager.DecryptString(File.ReadAllText(path + invenBuildingFileName)));
        }

        if (invenBuildingDatas == null) invenItemDatas = new InvenItemDatas();

        if (invenBuildingDatas.invenBuildingData == null || invenBuildingDatas.invenBuildingData.Length <= 0)
        {
            invenBuildingDatas.invenBuildingData = new int[Managers.Resource.GetBuildingCount()];
            for (int i = 0; i < Managers.Resource.GetBuildingCount(); i++)
            {
                invenBuildingDatas.invenBuildingData[i] = 0;
            }


        }

        return ref invenBuildingDatas.invenBuildingData;
    }
    public void AddInvenItem(int id, int cnt)
    {
        if (id >= 100)
        {
            invenBuildingDatas.invenBuildingData[id - ResourceManager.BUILDINGOFFSET] += cnt;
        }
        else
        {
            invenItemDatas.invenItemData[id - ResourceManager.ITEMOFFSET] += cnt;
        }
        itemUpdateDelegate(id);
    }
    public void AddQuestItem(int id, int cnt)
    {

        if (questProgressDatas.inProgressId == -1) return;

        int idx = -1;
        for (int i = 0; i < Managers.Resource.GetQuestData(questProgressDatas.inProgressId).Ingredients.Count; i++)
        {
            if (Managers.Resource.GetQuestData(questProgressDatas.inProgressId).Ingredients[i].id == id)
            {
                idx = i; break;
            }
        }

        if (idx == -1)
        {
            GameManagerEx.Instance.ThrowItemsToSpace(id);
            return;
        }

        questProgressDatas.questItems[idx] += cnt;

        questUpdateDelegate(idx);
    }
    public void AddQuestPopulation(int id, int cnt)
    {
        if (questProgressDatas.inProgressId == -1) return;

        questProgressDatas.populations[id] += cnt;
        questPopulationDelegate(questProgressDatas.inProgressId);
    }

    public void SaveInven()
    {
        DeleteInvenData();

        File.AppendAllText(path + invenItemFileName, AESManager.EncryptString(JsonUtility.ToJson(invenItemDatas)));
        File.AppendAllText(path + invenBuildingFileName, AESManager.EncryptString(JsonUtility.ToJson(invenBuildingDatas)));
    }
    private void DeleteInvenData()
    {
        if (File.Exists(path + invenBuildingFileName))
        {
            File.Delete(path + invenBuildingFileName);
        }
        if (File.Exists(path + invenItemFileName))
        {
            File.Delete(path + invenItemFileName);
        }
    }

    public void LoadQuestProgress()
    {
        if (!File.Exists(path + questProgressFileName))
        {
            questProgressDatas.inProgressId = -1;
            questProgressDatas.successId = -1;
            questProgressDatas.questItems = new int[3];
            questProgressDatas.populations = new float[Managers.Resource.GetQuestCount()];

            for(int i=0; i<3; i++)
            {
                questProgressDatas.questItems[i] = 0;
            }

            for(int i=0; i< Managers.Resource.GetQuestCount(); i++)
            {
                questProgressDatas.populations[i] = 0;
            }
        }
        else
        {
            questProgressDatas = JsonUtility.FromJson<QuestProgressDatas>(AESManager.DecryptString(File.ReadAllText(path + questProgressFileName)));
        }


    }

    public ref QuestProgressDatas GetQPDatas()
    {
        return ref questProgressDatas;
    }

    private void DeleteQuestProgress()
    {

        if (File.Exists(path + questProgressFileName))
        {
            File.Delete(path + questProgressFileName);
        }

    }
    public void SaveQuestProgress()
    {
        DeleteQuestProgress();
        File.AppendAllText(path + questProgressFileName, AESManager.EncryptString(JsonUtility.ToJson(questProgressDatas)));
    }


}
