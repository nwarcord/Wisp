using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayLinecastTools {

    public static bool ObjectVisible(BoxCollider2D boxCollider, Vector3 origin, Transform target, LayerMask layer) {
        
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(origin, target.position, layer);
        boxCollider.enabled = true;

        if (hit.transform == target) {
            return true;
        }
        return false;

    }

    public static bool ObjectVisible(BoxCollider2D boxCollider, CircleCollider2D circleCollider, Vector3 origin, Transform target, LayerMask layer) {
        
        boxCollider.enabled = false;
        circleCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(origin, target.position, layer);
        boxCollider.enabled = true;
        circleCollider.enabled = true;

        if (hit.transform == target) {
            return true;
        }
        return false;

    }

    // Returns the transform of the object at the given coordinates
    // If no object hit, returns null
    // Does not use layermask, so will return first object hit regardless of layer
    public static Transform ObjectAtCoords(Vector3 targetCoords) {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(targetCoords));
        return hit.transform;
    }

    // Override of ObjectAtTile
    // Takes a layermask as an additional parameter
    public static Transform ObjectAtCoords(Vector3 targetCoords, LayerMask layer) {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(targetCoords), layer);
        return hit.transform;
    }

}
