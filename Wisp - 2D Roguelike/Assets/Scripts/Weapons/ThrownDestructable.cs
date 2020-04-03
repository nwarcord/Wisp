using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownDestructable : MonoBehaviour {
    
    [SerializeField]
    private AreaOfEffect aoeSpawn;
    

    private void OnDisable() {
        GameObject.Instantiate(aoeSpawn, transform.position, new Quaternion());
    }

}
