using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {

    protected int damage;
    protected Sprite attackIcon;
    protected string attackName;
    protected string description;
    protected int range;
    // Placeholder for animation?
    // Effects?
    // Range

    private void Awake() {
        InitVariables();
    }

    protected abstract void InitVariables();

    public abstract bool ExecuteAttack(Vector3 tileCoords, int modifier);

}
