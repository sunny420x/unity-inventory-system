using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    [System.Serializable]
    public struct SaveGameData
    {
        public float player_posx;
        public float player_posy;
        public float player_posz;
        public int[] inventoryData;
        public int[] inventoryItemsAmounts;
        public float health;

        public SaveGameData(float player_posx, float player_posy, float player_posz, int[] inventoryData, int[] inventoryItemsAmounts, float health)
        {
            this.player_posx = player_posx;
            this.player_posy = player_posy;
            this.player_posz = player_posz;
            this.inventoryData = inventoryData;
            this.inventoryItemsAmounts = inventoryItemsAmounts;
            this.health = health;
        }
    }

    public GameObject Player;

    public void SaveGameClick()
    {
        SaveGame();
    }

    public void LoadGameClick()
    {
        LoadGame();
    }

    private void SaveGame()
    {
        SaveGameData SaveGameData_Struct;
        SaveGameData_Struct = new SaveGameData(
            Player.transform.position.x,
            Player.transform.position.y,
            Player.transform.position.z,
            Player.GetComponent<Inventory>().inventoryData,
            Player.GetComponent<Inventory>().inventoryItemsAmounts,
            Player.GetComponent<PlayerStatus>().health
         );

        string path = Application.persistentDataPath + Path.DirectorySeparatorChar;

        if (!Directory.Exists(path+"Saves")) {
            Directory.CreateDirectory(path+"Saves");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream SaveFile = File.Create(path+ @"Saves\gamesave.bin");
        formatter.Serialize(SaveFile, SaveGameData_Struct);
        SaveFile.Close();

        Debug.Log("Saved Game! to "+path);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + Path.DirectorySeparatorChar;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream SaveFile = File.Open(path + @"Saves\gamesave.bin", FileMode.Open);
        SaveGameData LoadedData = (SaveGameData)formatter.Deserialize(SaveFile);

        SaveFile.Close();

        Debug.Log("Loaded Data: "+
            "PosX: " + LoadedData.player_posx+
            "PosY: " + LoadedData.player_posy+
            "PoSZ: " + LoadedData.player_posz+
            "Inventory Data:" + LoadedData.inventoryData+
            "Inventory Amounts: " + LoadedData.inventoryItemsAmounts+
            "health: " + LoadedData.health +
        "");

        //Player.transform.position = new Vector3(LoadedData.player_posx, LoadedData.player_posy, LoadedData.player_posz);
        Player.GetComponent<Inventory>().inventoryData = LoadedData.inventoryData;
        Player.GetComponent<Inventory>().inventoryItemsAmounts = LoadedData.inventoryItemsAmounts;
        Player.GetComponent<PlayerStatus>().health = LoadedData.health;
    }
}
