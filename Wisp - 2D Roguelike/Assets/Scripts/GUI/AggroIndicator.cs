using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroIndicator : MonoBehaviour {

    private SpriteRenderer icon = default;

    private void Awake() {
        icon = gameObject.GetComponent<SpriteRenderer>();
        icon.enabled = false;
    }

    public void ShowAggro() {
        icon.enabled = true;
        StartCoroutine(IconDuration());
    }

    private IEnumerator IconDuration() {
        yield return new WaitForSeconds(1f);
        icon.enabled = false;
    }

}
