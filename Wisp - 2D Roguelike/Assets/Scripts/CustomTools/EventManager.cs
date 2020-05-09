using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public static class EventManager {

    // public static event Action playerEntersCombat;
    // public static event Action playerLeftCombat;
    public static event Action actorTurnOver;
    public static event Action combatOver;
    public static event Action<ITurnAct> combatSpawn;
    // public static event Action<ITurnAct> aggroPlayer;
    public static event Action aggroPlayer;
    public static event Action enemyDeath;
    public static event Action<ITurnAct> enemySpawn;
    public static event Action combatStart;
    public static event Action playerDamaged;
    public static event Action playerMoving;
    public static event Action playerStopped;

    // ----------------------------------------------------------------
    // Combat system events
    // ----------------------------------------------------------------

    public static void RaisePlayerMoving() => playerMoving?.Invoke();
    public static void RaisePlayerStopped() => playerStopped?.Invoke();

    // ----------------------------------------------------------------
    // Turn system events
    // ----------------------------------------------------------------

    public static void RaiseActorTurnOver() => actorTurnOver?.Invoke();
    public static void RaiseCombatOver() => combatOver?.Invoke();
    public static void RaiseCombatSpawn(ITurnAct spawn) => combatSpawn?.Invoke(spawn);
    public static void RaiseCombatStart() => combatStart?.Invoke();

    // ----------------------------------------------------------------
    // Enemy combat events
    // ----------------------------------------------------------------

    // public static void RaiseAggroPlayer() => aggroPlayer?.Invoke();
    // public static void RaiseAggroPlayer(ITurnAct enemy) => aggroPlayer?.Invoke(enemy);
    public static void RaiseAggroPlayer() => aggroPlayer?.Invoke();
    public static void RaiseEnemyDeath() => enemyDeath?.Invoke();
    public static void RaiseEnemySpawn(ITurnAct enemy) => enemySpawn?.Invoke(enemy);

    // ----------------------------------------------------------------
    // Player combat events
    // ----------------------------------------------------------------

    public static void RaisePlayerDamaged() => playerDamaged?.Invoke();

    // public static void RaisePlayerEntersCombat() => playerEntersCombat?.Invoke();
    // public static void RaisePlayerLeftCombat() => playerLeftCombat?.Invoke();

}
