using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBlock : MonoBehaviour {

    [SerializeField]
    private List<GameObject> blocks = default;

    private void BlockPath(bool state) {
        foreach(GameObject block in blocks) {
            block.SetActive(state);
        }
    }


}
