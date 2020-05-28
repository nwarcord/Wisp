using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBlock : MonoBehaviour {

    private SpriteRenderer sprite;
    private Collider2D coll;
    private PathBlockSound pathBlockSound;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        pathBlockSound = GetComponent<PathBlockSound>();
    }

    private void OnDisable() {
        // sprite.enabled = false;
        // coll.enabled = false;
        if (pathBlockSound.gameObject.activeSelf) pathBlockSound.DoorDisable();
    }


}
