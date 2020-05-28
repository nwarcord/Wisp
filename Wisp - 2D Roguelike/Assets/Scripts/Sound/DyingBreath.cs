using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingBreath : MonoBehaviour {

    [SerializeField]
    private AudioClip deathSound = default;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void StartDeathBreath() {
        StartCoroutine(DeathBreath());
    }

    IEnumerator DeathBreath() {
        audioSource.PlayOneShot(deathSound);
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

}
