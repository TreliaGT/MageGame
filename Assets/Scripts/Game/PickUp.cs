using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Pickup")]
    public int pickupitem = 5;
    public GameObject DestoryPrefab;

    

    public void pickup(out int ToAdd)
    {
        ToAdd = 5;
        Destroy(gameObject);
    }


}
