using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedRoom : MonoBehaviour {

    // [SerializeField]
    // private bool runOnce = true;
    [SerializeField]
    private RoomTriggers startTriggerType = default;
    [SerializeField]
    private RoomTriggers endTriggerType = default;

    [SerializeField]
    private List<Collider2D> pathBlocks = default;

    // [SerializeField]
    // private List<List<Enemy>> spawnRound = default;

    [SerializeField]
    private List<Enemy> spawns = default;

    // private bool activated = false;
    private int startTriggers = 0;
    private int endTriggers = 0;

    private void Start() {
        
    }
    
    public void RegisterTrigger(RoomTriggers triggerType) {
        if (startTriggerType == triggerType) startTriggers++;
        else if (endTriggerType == triggerType) endTriggers++;
        else Debug.LogError("Invalid attempt to register RoomTrigger to ActivatedRoom.");
    }

    public void TriggerActivated(RoomTriggers triggerType) {
        if (startTriggerType == triggerType) StartTriggerActivated();
        else if (endTriggerType == triggerType) EndTriggerActivated();
        else Debug.LogError("Invalid RoomTrigger activated.");
    }

    private void StartTriggerActivated() {
        if (startTriggerType == RoomTriggers.EnemyDefeated) startTriggers--;
        else if (startTriggerType == RoomTriggers.Switch) startTriggers = 0;
        if (startTriggers <= 0) {
            SetListActive(pathBlocks, true);
            SetListActive(spawns, true);
        }
    }

    private void EndTriggerActivated() {
        if (endTriggerType == RoomTriggers.EnemyDefeated) endTriggers--;
        else if (endTriggerType == RoomTriggers.Switch) endTriggers = 0;
        if (endTriggers <= 0) {
            foreach (Collider2D block in pathBlocks) {
                block.GetComponent<PathBlock>().enabled = false;
            }
        }
    }

    private void SetListActive<T>(List<T> objects, bool state) where T : Component {
        foreach (T obj in objects) {
            obj.gameObject.SetActive(state);
        }
    }


}