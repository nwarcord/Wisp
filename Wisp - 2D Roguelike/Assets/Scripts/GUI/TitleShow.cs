using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleShow : MonoBehaviour {

    private Image title;

    private void Awake() {
        title = GetComponent<Image>();
        title.enabled = false;
    }

    private void Start() {
        StartCoroutine(ShowTitle());
    }

    IEnumerator ShowTitle() {
        yield return new WaitForSeconds(1.25f);
        title.enabled = true;
    }

}
