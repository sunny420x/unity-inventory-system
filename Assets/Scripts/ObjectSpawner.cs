using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private Item[] Items;
    public GameObject Player;
    public GameObject itemsManager;

    public void Start() {
        Items = itemsManager.GetComponent<Items>().items;
    }

    public void Spawner(int obj_id)
    {
        Vector3 SpawnPoint = transform.position;
        Instantiate(Items[obj_id].prefab, SpawnPoint, Quaternion.identity);
    }
}
