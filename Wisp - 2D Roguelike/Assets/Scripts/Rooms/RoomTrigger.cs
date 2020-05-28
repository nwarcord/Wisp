using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomTriggers {
    EnemyDefeated,
    Switch
}

public enum RoomTriggerType {
    StartTrigger,
    EndTrigger
}

public class RoomTrigger : MonoBehaviour {

    [SerializeField]
    private RoomTriggers trigger = default;
    [SerializeField]
    private RoomTriggerType triggerType = default;
    [SerializeField]
    private ActivatedRoom activatedRoom = default;

    private void OnEnable() {
        activatedRoom.RegisterTrigger(trigger, triggerType);
    }

    private void OnDisable() {
        activatedRoom.TriggerActivated(trigger, triggerType);
    }

}
