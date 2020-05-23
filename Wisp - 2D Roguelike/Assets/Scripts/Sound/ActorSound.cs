using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType {
    walking,
    death,
    takeDamage
};

// Make this class abstract
public class ActorSound : MonoBehaviour {

    [SerializeField]
    private AudioSource walking = default;
    [SerializeField]
    private Dictionary<AttackType, AudioSource> attackSounds = default;
    [SerializeField]
    private AudioSource death = default;
    [SerializeField]
    private AudioSource takeDamage = default;


    public void PlaySound(SoundType soundType) {
        if (soundType == SoundType.walking && walking != null) walking.Play();
        else if (soundType == SoundType.death && death != null) death.Play();
        else if (soundType == SoundType.takeDamage && takeDamage != null) takeDamage.Play();
        else Debug.Log("Sound of type " + soundType.ToString() + " not found."); 
    }

    public void PlaySound(AttackType attack) {
        if (attackSounds[attack] != null) {
            attackSounds[attack].Play();
        }
        else Debug.Log("Attack sound of type " + attack.ToString() + " not found.");
    }


}
