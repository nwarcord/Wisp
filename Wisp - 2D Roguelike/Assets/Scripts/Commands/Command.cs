using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public abstract class Command<T> {

//     protected abstract void Execute(T component, Vector3 vector);

// }

// public class Attack : Command<CombatComponent> {

//     protected override void Execute(CombatComponent component, Vector3 target) {
//         component.OneTileAttack(target);
//     }

// }

// public class Move : Command<MovementComponent> {

//     protected override void Execute(MovementComponent component, Vector3 moveTo) {
//         component.AttemptMove(moveTo);
//     }

// }

public static class MoveCommand {

    // private MovementComponent movement;

    // public MoveCommand(MovementComponent movement) {
    //     this.movement = movement;
    // }

    public static void Execute(MovementComponent movement, Vector3 moveTo) {
        movement.AttemptMove(moveTo);
    }

}

public abstract class Attack {

    protected CombatComponent combat;

    public Attack(CombatComponent combat) {
        this.combat = combat;
    }

    public abstract void ExecuteAttack(Vector3 target);

}

public class MeleeOneTile : Attack {

    public MeleeOneTile(CombatComponent combat) : base(combat){}

    public override void ExecuteAttack(Vector3 target) {
        combat.OneTileAttack(target);
    }

}

public class ProjectileAttack : Attack {

    public ProjectileAttack(CombatComponent combat) : base(combat){}

    public override void ExecuteAttack(Vector3 target) {
        return;
    }

}