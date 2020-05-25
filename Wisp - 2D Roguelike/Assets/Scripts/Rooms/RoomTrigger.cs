using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomTriggers {
    EnemyDefeated,
    Switch
}

public class RoomTrigger : MonoBehaviour {

    [SerializeField]
    private RoomTriggers triggerType = default;
    [SerializeField]
    private ActivatedRoom activatedRoom = default;

    private void OnEnable() {
        activatedRoom.RegisterTrigger(triggerType);
    }

    private void OnDisable() {
        activatedRoom.TriggerActivated(triggerType);
    }

}
