using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SaveLoadMenu : MonoBehaviour
{
    public Text menuLabel, actionButtonLabel;
    public InputField nameInput;
    public RectTransform listContent;
    public SaveLoadItem itemPrefab;
    public HexGrid hexGrid;

    //string mapDataFilePath = Application.persistentDataPath;

    bool saveMode;

    //////////////////////////////////////////////////////////////
    /////////////////// MapEditor Event Method ///////////////////
    /// //////////////////////////////////////////////////////////
    public void Open(bool saveMode)
    {
        this.saveMode = saveMode;
        if (saveMode)
        {
            menuLabel.text = "Save Map";
            actionButtonLabel.text = "Save";
        }
        else
        {
            menuLabel.text = "Load Map";
            actionButtonLabel.text = "Load";
        }

        FillList();
        gameObject.SetActive(true);

        HexMapCamera.Locked = true;
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
        HexMapCamera.Locked = false;
    }

    public void SaveOrLoad()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }

        if (saveMode)
        {
            Save(path);
        }
        else
        {
            Load(path);
        }
        Close();
    }

    public void Delete()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        nameInput.text = "";
        FillList();
    }
    //////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////

    public void SelectItem(string name)
    {
        nameInput.text = name;
    }

    void FillList()
    {
        for (int i = 0; i < this.listContent.childCount; i++)
        {
            Destroy(this.listContent.GetChild(i).gameObject);
        }

        string[] paths =
            Directory.GetFiles(Application.persistentDataPath, "*.map");
        Array.Sort(paths);

        for (int i = 0; i < paths.Length; i++)
        {
            SaveLoadItem item = Instantiate(itemPrefab);
            {
                item.menu = this;
                item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
                item.transform.SetParent(this.listContent, false);
            }
        }
    }

    string GetSelectedPath()
    {
        string mapName = nameInput.text;
        if (mapName.Length == 0)
        {
            return null;
        }

        return Path.Combine(Application.persistentDataPath, mapName + ".map");
    }

    void Save(string path)
    {
        Debug.Log(path);
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(2); // ?
            hexGrid.Save(writer);
        }
    }

    /// <summary>
    /// GameLoad 로직 !!!!
    /// </summary>
    /// <param name="path"></param>
    void Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist " + path);
            return;
        }

        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header <= 2)
            {
                hexGrid.Load(reader, header);
                HexMapCamera.ValidatePosition();
            }
            else
            {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
    }
}