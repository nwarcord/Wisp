using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownDestructable : MonoBehaviour {
    
    [SerializeField]
    private AreaOfEffect aoeSpawn = default;
    

    private void OnDisable() {
        AreaOfEffect aoe = GameObject.Instantiate(aoeSpawn, transform.position, new Quaternion());
        // EventManager.RaiseCombatSpawn(aoe);
    }

}
