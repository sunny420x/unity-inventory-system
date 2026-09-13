using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public Texture icon;
    public string title;
    [TextArea]
    public string description;
    public bool usable;
    public bool equipable;
    public GameObject prefab;
}

public class Items : MonoBehaviour
{
    [SerializeField] public Item[] items;
}
