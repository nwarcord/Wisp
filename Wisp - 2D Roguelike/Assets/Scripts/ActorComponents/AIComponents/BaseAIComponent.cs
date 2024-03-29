﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// ----------------------------------------------------------------
// Not currently in use
// ----------------------------------------------------------------

// Enum for state choice? Patrol - Move - Attack (with type)

public abstract class BaseAIComponent : MonoBehaviour {

    protected Transform position;
    protected Transform target;
    protected List<AttackType> attacks;
    protected BaseCombatComponent combat;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb2D;

    private void Awake() {
        position = gameObject.transform;
        seeker = gameObject.GetComponent<Seeker>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start() {
        SetTarget();
        SetCombat();
        seeker.StartPath(rb2D.position, target.position, OnPathComplete);
    }

    protected void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb2D.position, target.position, OnPathComplete);
        }
    }

    private void FixedUpdate() {
        // if (path == null) return;

        // if (currentWaypoint >= path.vectorPath.Count) {
        //     reachedEndOfPath = true;
        //     return;
        // }
        // else {
        //     reachedEndOfPath = false;
        // }

        // Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2D.position).normalized;
        // Vector2 force = direction * speed * Time.deltaTime;

        // rb2D.AddForce(force);

        // float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWaypoint]);

        // if (distance < nextWaypointDistance) {
        //     currentWaypoint++;
        // }

    }

    protected void MoveOnPath() {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        }
        else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb2D.AddForce(force);

        float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
        if (reachedEndOfPath) {
            return;
        }
    }

    protected abstract void SetCombat();

    protected abstract void SetTarget();

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void Think(bool inCombat) {
        if (!inCombat) Patrol();
        else Engage();
    }

    public void UpdateTarget(Transform target) {
        this.target = target;
    }

    // Non-combat behavior
    // private void Patrol() { movement.AttemptMove(DecideMove(false)); }

    protected virtual void Patrol() {
        // Walk around without player as target
    }

    // Combat behavior
    // Path toward player, or to a designated spot, and choose an attack to perform
    protected abstract void Engage();

    // Choosing where to move to
    // protected abstract Vector3 DecideMove(bool inCombat);

    // If Attack, decide attack type
    protected abstract void DecideAttack();

}

/*

If not in combat, move along path with arbitrary target
    If player in sight, aggro

If in combat, check if should attack player
    If shouldn't attack, check if should move toward player
    If shouldn't move toward player, pick appropriate target

*/