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

}
