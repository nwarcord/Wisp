using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour {

    [SerializeField]
    private Texture2D swordIcon = default;
    [SerializeField]
    private Texture2D shardIcon = default;
    [SerializeField]
    private Texture2D bottleIcon = default;
    [SerializeField]
    private Texture2D healthBar = default;

    private void OnGUI() {
        
        GUI.Box(new Rect(4, Screen.height - 24, 14, 20), swordIcon);
        GUI.Box(new Rect(4, Screen.height - 41, 25, 20), shardIcon);
        GUI.Box(new Rect(4, Screen.height - 24, 22, 20), bottleIcon);
        GUI.Box(new Rect(4, 4, 80, 16), healthBar);

    }

}
