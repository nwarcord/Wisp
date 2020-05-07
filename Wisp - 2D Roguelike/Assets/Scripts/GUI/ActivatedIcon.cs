using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedIcon : MonoBehaviour {

    [SerializeField]
    private Sprite defaultIcon = default;
    [SerializeField]
    private Sprite activeIcon = default;
    [SerializeField]
    private KeyCode key = default;

    private Image imageComponent;

    private void Awake() {
        imageComponent = gameObject.GetComponent<Image>();
        if (!imageComponent.sprite) {
            imageComponent.sprite = defaultIcon;
        }
    }

    void Update() {
        if (Input.GetKey(key)) {
            imageComponent.sprite = activeIcon;
        }
        else {
            imageComponent.sprite = defaultIcon;
        }
    }
}
