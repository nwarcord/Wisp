using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICanBeDamaged {

    private const int maxHealth = 10;
    private int health;
    private PlayerCombatComponent combat;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip meleeSound = default;
    [SerializeField]
    private AudioClip hitSound = default;
    [SerializeField]
    private AudioClip rangedSound = default;
    [SerializeField]
    private AudioClip healSound = default;

    // ----------------------------------------------------------------
    // Event subscribe and unsubscribe
    // ----------------------------------------------------------------

    private void OnEnable() {
        EventManager.combatStart += PlayerEnterCombat;
        EventManager.combatOver += PlayerLeaveCombat;
    }

    private void OnDisable() {
        EventManager.combatStart -= PlayerEnterCombat;
        EventManager.combatOver -= PlayerLeaveCombat;
    }

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    void Awake() {
        health = 10;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        combat = gameObject.GetComponent<PlayerCombatComponent>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public PlayerCombatComponent Combat() {
        return this.combat;
    }

    public int GetPlayerHealth() {
        return this.health;
    }

    // ----------------------------------------------------------------
    // Combat mechanics
    // ----------------------------------------------------------------

    public void PlayerEnterCombat() {
        combat.EnterCombat();
    }

    public void PlayerLeaveCombat() {
        combat.ExitCombat();
        Debug.Log("Player left combat");
    }

    // public void TakeDamage(int damage) {
    //     health -= damage;
    //     if (gameObject.activeInHierarchy) audioSource.PlayOneShot(hitSound);
    //     StartCoroutine(TakeDamageAnim());
    //     if (health < 0) health = 0;
    //     EventManager.RaisePlayerHealthUpdate();
    //     if (health == 0) EventManager.RaisePlayerDied();
    // }
    
    public void TakeDamage(AttackInfo attackInfo) {
        health -= attackInfo.damage;
        if (gameObject.activeInHierarchy) audioSource.PlayOneShot(hitSound);
        StartCoroutine(TakeDamageAnim());
        if (health < 0) health = 0;
        EventManager.RaisePlayerHealthUpdate();
        if (health == 0) EventManager.RaisePlayerDied();
    }

    public bool IsAlive() {
        return health > 0;
    }

    public void Heal(int amount) {
        this.health += amount;
        PlayPlayerHeal();
        if (this.health > maxHealth) this.health = maxHealth;
        EventManager.RaisePlayerHealthUpdate();
    }

    // ----------------------------------------------------------------
    // Turn mechanics
    // ----------------------------------------------------------------

    public AudioSource GetAudioSource() {
        return audioSource;
    }

    public void PlayMeleeAttack() {
        audioSource.PlayOneShot(meleeSound, 0.4f);
    }

    public void PlayRangedAttack() {
        audioSource.PlayOneShot(rangedSound);
    }

    private void PlayPlayerHeal() {
        audioSource.PlayOneShot(healSound, 0.7f);
    }

    private IEnumerator TakeDamageAnim() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        rend.color = Color.white;
    }

}
