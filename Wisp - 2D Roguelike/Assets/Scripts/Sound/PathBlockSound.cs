using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBlockSound : MonoBehaviour {

    [SerializeField]
    private AudioClip closeSound = default;
    [SerializeField]
    private AudioClip openSound = default;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        audioSource.PlayOneShot(closeSound);
    }

    public void DoorDisable() {
        StartCoroutine(WaitForSound());
    }

    IEnumerator WaitForSound() {
        audioSource.PlayOneShot(openSound);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
