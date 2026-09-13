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
        public int[] inventory_data;
        public int[] inventory_items_amounts;
        public float health;

        public SaveGameData(float player_posx, float player_posy, float player_posz, int[] inventory_data, int[] inventory_items_amounts, float health)
        {
            this.player_posx = player_posx;
            this.player_posy = player_posy;
            this.player_posz = player_posz;
            this.inventory_data = inventory_data;
            this.inventory_items_amounts = inventory_items_amounts;
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
            Player.GetComponent<Inventory>().inventory_data,
            Player.GetComponent<Inventory>().inventory_items_amounts,
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
            "Inventory Data:" + LoadedData.inventory_data+
            "Inventory_Amounts: " + LoadedData.inventory_items_amounts+
            "health: " + LoadedData.health +
        "");

        //Player.transform.position = new Vector3(LoadedData.player_posx, LoadedData.player_posy, LoadedData.player_posz);
        Player.GetComponent<Inventory>().inventory_data = LoadedData.inventory_data;
        Player.GetComponent<Inventory>().inventory_items_amounts = LoadedData.inventory_items_amounts;
        Player.GetComponent<PlayerStatus>().health = LoadedData.health;
    }
}
