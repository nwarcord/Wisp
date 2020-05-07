using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoundaryColliderConnector : MonoBehaviour {

    [SerializeField]
    private EdgeCollider2D XMin = default, XMax = default, YMin = default, YMax = default;


    void Update() {
        XMin.points[0].y = YMax.transform.localPosition.y;
        XMin.points[1].y = YMin.transform.localPosition.y;
        XMax.points[0].y = YMax.transform.localPosition.y;
        XMax.points[1].y = YMin.transform.localPosition.y;
        YMin.points[0].x = XMax.transform.localPosition.x;
        YMin.points[1].x = XMin.transform.localPosition.x;
        YMax.points[0].x = XMax.transform.localPosition.x;
        YMax.points[1].x = XMin.transform.localPosition.x;
    }

}
