using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedRoom : MonoBehaviour {

    // [SerializeField]
    // private bool runOnce = true;
    [SerializeField]
    private RoomTriggers startTrigger = default;
    [SerializeField]
    private RoomTriggers endTrigger = default;

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
    
    public void RegisterTrigger(RoomTriggers trigger, RoomTriggerType triggerType) {
        if (startTrigger == trigger && triggerType == RoomTriggerType.StartTrigger) startTriggers++;
        else if (endTrigger == trigger && triggerType == RoomTriggerType.EndTrigger) endTriggers++;
        else Debug.LogError("Invalid attempt to register RoomTrigger to ActivatedRoom.");
    }

    public void TriggerActivated(RoomTriggers trigger, RoomTriggerType triggerType) {
        if (startTrigger == trigger && triggerType == RoomTriggerType.StartTrigger) StartTriggerActivated();
        else if (endTrigger == trigger && triggerType == RoomTriggerType.EndTrigger) EndTriggerActivated();
        else Debug.LogError("Invalid RoomTrigger activated.");
    }

    private void StartTriggerActivated() {
        if (startTrigger == RoomTriggers.EnemyDefeated) startTriggers--;
        else if (startTrigger == RoomTriggers.Switch) startTriggers = 0;
        if (startTriggers <= 0) {
            SetListActive(pathBlocks, true);
            SetListActive(spawns, true);
            SetPathBlockActive(true);
        }
    }

    private void EndTriggerActivated() {
        if (endTrigger == RoomTriggers.EnemyDefeated) endTriggers--;
        else if (endTrigger == RoomTriggers.Switch) endTriggers = 0;
        if (endTriggers <= 0) {
            // foreach (Collider2D block in pathBlocks) {
            //     if (block != null) block.GetComponent<PathBlock>().enabled = false;
            // }
            SetPathBlockActive(false);
            // Debug.Log("Path Cleared.");
        }
    }

    private void SetPathBlockActive(bool state) {
        foreach (Collider2D block in pathBlocks) {
            if (block != null) block.GetComponent<PathBlock>().enabled = state;
        }
    }

    private void SetListActive<T>(List<T> objects, bool state) where T : Component {
        foreach (T obj in objects) {
            if (obj != null) obj.gameObject.SetActive(state);
        }
    }


}