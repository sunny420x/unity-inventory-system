using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetail : MonoBehaviour
{
    public int obj_id;
    public string type;
    public bool readable;

    [TextAreaAttribute]
    public string read_contents;
    // public string OnItemPickUpActivateState;

    // public void OnItemPickUp() {
    //     if(OnItemPickUpActivateState) {
            
    //     }
    // }
}
