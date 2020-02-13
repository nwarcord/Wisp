using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {

    public static event Action onCombat;
    public static event Action combatExit;
    public static event Action playerEntersCombat;
    public static event Action playerLeftCombat;
    public static event Action actorTurnOver;
    public static event Action combatOver;
    public static event Action<ITurnAct> combatSpawn;
    public static event Action aggroPlayer;
    public static event Action enemyDeath;

    public static void RaiseOnCombat() => onCombat?.Invoke();
    public static void RaiseOnCombatExit() => combatExit?.Invoke();
    public static void RaisePlayerEntersCombat() => playerEntersCombat?.Invoke();
    public static void RaisePlayerLeftCombat() => playerLeftCombat?.Invoke();
    public static void RaiseActorTurnOver() => actorTurnOver?.Invoke();
    public static void RaiseCombatOver() => combatOver?.Invoke();
    public static void RaiseCombatSpawn(ITurnAct spawn) => combatSpawn?.Invoke(spawn);
    public static void RaiseAggroPlayer() => aggroPlayer?.Invoke();
    public static void RaiseEnemyDeath() => enemyDeath?.Invoke();

    public static void CheckActorTurnOver() {
        Debug.Log(actorTurnOver == null);
    }

}
