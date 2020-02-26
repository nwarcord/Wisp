using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GridLocator {

    private static Grid grid;

    public static void InitGrid(Grid newGrid) {
        grid = newGrid;
    }

    public static void FindGrid() {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    public static Grid GetGrid() {
        return grid;
    }

}
