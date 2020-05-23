using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    private List<Sprite> images = default;

    private Image imageComponent;
    private PlayerController player;
    private int currentHealth = 0;

    private void OnEnable() {
        EventManager.playerHealthUpdate += UpdateHealth;
    }

    private void OnDisable() {
        EventManager.playerHealthUpdate -= UpdateHealth;
    }

    private void Awake() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        currentHealth = player.GetPlayerHealth();
        imageComponent = gameObject.GetComponent<Image>();
    }

    private void UpdateHealth() {
        currentHealth = player.GetPlayerHealth();
        if (currentHealth <= 0) imageComponent.sprite = images[0];
        else imageComponent.sprite = images[currentHealth];
    }

}
