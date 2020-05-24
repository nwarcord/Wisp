using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPool : MonoBehaviour {

    [SerializeField]
    private GameObject commonDrop = default;
    [SerializeField]
    private GameObject uncommonDrop = default;

    // private void OnDisable() {
    //     Vector3 spawnLocation = transform.position;
    //     spawnLocation.z = 0;
    //     System.Random rand = new System.Random();
    //     int chance = rand.Next(0, 100);
    //     if (chance < 30) GameObject.Instantiate(commonDrop, spawnLocation, new Quaternion());
    //     else if (chance < 40) GameObject.Instantiate(uncommonDrop, spawnLocation, new Quaternion());
    // }

    public void DropLoot() {
        Vector3 spawnLocation = transform.position;
        spawnLocation.z = 0;
        System.Random rand = new System.Random();
        int chance = rand.Next(0, 100);
        if (chance < 30) GameObject.Instantiate(commonDrop, spawnLocation, new Quaternion());
        else if (chance < 40) GameObject.Instantiate(uncommonDrop, spawnLocation, new Quaternion());
    }

}
