using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack {

    bool ExecuteAttack(Vector3 tileCoords);

}

// public abstract class Weapon : MonoBehaviour {

//     protected string name;
//     protected int damage;
//     protected int range;
//     protected Sprite sprite;

//     private void Awake() {
//         InitVariables();
//     }

//     protected abstract void InitVariables();

// }

// public class SimpleDagger : Weapon {

//     protected override void InitVariables() {
//         this.name = "Simple Dagger";
//         this.damage = 2;
//         this.range = 1;
//     }

// }

// public class Unarmed : Weapon {

//     protected override void InitVariables() {
//         this.name = "Unarmed";
//         this.damage = 1;
//         this.range = 1;
//     }

// }

public class Weapon {

    private string name { get; private set; }
    private int damage { get; private set; }
    private int range { get; private set; }
    private Sprite sprite { get; private set; }

    Weapon(string name, int damage, int range) {
        this.name = name;
        this.damage = damage;
        this.range = range;
    }

}

public static class Weapons {

    public static Dictionary<string, Weapon> weaponList {
        
    }

}

public class MeleeAttack : MonoBehaviour, IAttack {

    private int range = 1;
    private int damage = 1;
    private Weapon weapon;


    private void Awake() {
        InitVariables();
    }

    private void InitVariables() {
        this.damage = 1;
        this.range = 1;
    }

    public override bool ExecuteAttack(Vector3 tileCoords, int modifier) {
    
        Transform actorPosition = gameObject.transform;
        Transform target = TileSystem.ObjectAtTile(tileCoords);
        if (target != null && TileSystem.TileDistance(target.position, actorPosition.position) <= range) {
            ICanBeDamaged victim = target.GetComponent<ICanBeDamaged>();
            if (victim != null) {
                victim.TakeDamage(damage * modifier);
                return true;
            }   

        }
        return false;

    }

}
