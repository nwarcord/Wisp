using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackSprite : MonoBehaviour {

    private SpriteRenderer sprite;
    private const float midPoint = 0.707f;

    private void Awake() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    public void SpawnOrientation(Vector3 actorCoords, Vector3 attackCoords) {
        Vector3 attackModified = attackCoords;
        attackModified.z = 0;
        // Vector3 attack = attackModified.normalized;
        // attack += actorCoords;
        Vector3 difference = (attackModified - actorCoords).normalized;
        if (difference.y > 0 && (difference.x <= midPoint && difference.x >= -midPoint)) {
            transform.localPosition = new Vector3(0, 0.75f, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (difference.y < 0 && ((difference.x <= midPoint && difference.x >= -midPoint))) {
            transform.localPosition = new Vector3(0, -1, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            // transform.SetPositionAndRotation(new Vector3(0, -1, 0), Quaternion.Euler(0, 0, 180));
        }
        else if (difference.x > 0 && (difference.y <= midPoint && difference.y >= -midPoint)) {
            transform.localPosition = new Vector3(0.75f, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            // transform.SetPositionAndRotation(new Vector3(1, 0, 0), Quaternion.Euler(0, 0, 270));
        }
        else {
            transform.localPosition = new Vector3(-0.75f, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            // transform.SetPositionAndRotation(new Vector3(-1, 0, 0), Quaternion.Euler(0, 0, 90));
        }
        sprite.enabled = true;
        // Debug.Log("Direction vector: " + difference + " | Actor coords: " + actorCoords + " | Attack coords: " + attackModified);
        StartCoroutine(MeleeAnimation());
    }

    private IEnumerator MeleeAnimation() {
        yield return new WaitForSeconds(0.2f);
        sprite.enabled = false;
    }

}
