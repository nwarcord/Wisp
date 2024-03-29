﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileSystem {

    public const float oneTileMaxDistance = 1.42f;

    // Returns the transform of the object at the given coordinates
    // If no object hit, returns null
    // Does not use layermask, so will return first object hit regardless of layer
    public static Transform ObjectAtTile(Vector3 tileCoords) {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(tileCoords));
        return hit.transform;
    }

    // Override of ObjectAtTile
    // Takes a layermask as an additional parameter
    public static Transform ObjectAtTile(Vector3 tileCoords, LayerMask layer) {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(tileCoords), layer);
        return hit.transform;
    }

    public static int TileDistance(Vector2 tileOne, Vector2 tileTwo) {
        Vector2 toFrom = tileOne - tileTwo;
        float distance = toFrom.magnitude;
        if (distance <= oneTileMaxDistance) {
            return 1;
        }
        return Mathf.FloorToInt(distance);
    }

    public static int TileDistance(Vector3 tileOne, Vector3 tileTwo) {
        Vector2 tileOneConverted = new Vector2(tileOne.x, tileOne.y);
        Vector2 tileTwoConverted = new Vector2(tileTwo.x, tileTwo.y);
        return TileDistance(tileOneConverted, tileTwoConverted);
    }

    public static Vector3 AdjacentTile(Vector3Int target, Vector3Int home, Vector3 actor) {
        Vector3 adjacentTile = actor;
        if (target.x != home.x) {
            if (target.x < home.x) adjacentTile.x--;
            else adjacentTile.x ++;
        }
        if (target.y != home.y) {
            if (target.y < home.y) adjacentTile.y--;
            else adjacentTile.y++;
        }
        return adjacentTile;
    }

}
