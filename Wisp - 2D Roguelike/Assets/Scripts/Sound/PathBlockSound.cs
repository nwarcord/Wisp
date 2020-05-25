using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBlockSound : MonoBehaviour {

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        audioSource.Play();
    }

    public void DoorDisable() {
        StartCoroutine(WaitForSound());
    }

    IEnumerator WaitForSound() {
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
