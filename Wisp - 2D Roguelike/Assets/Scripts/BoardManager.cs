using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour {

    private const int MaxGridSearch = 15;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Grid grid = default;

    // Holds which object (if any) are at any given coordinates
    private Dictionary<Vector3Int, GameObject> worldObjects;

    // Holds location of all objects present on board
    private Dictionary<GameObject, Vector3Int> objectLocations;

    private List<ITurnAct> combatActors;

    private void Awake() {
        worldObjects = new Dictionary<Vector3Int, GameObject>();
        objectLocations = new Dictionary<GameObject, Vector3Int>();
        combatActors = new List<ITurnAct>();
        InitBoard();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void InitBoard() {

        // Create a dictionary with cell positions
        for (int i = -MaxGridSearch; i < MaxGridSearch; i++) {
            for (int j = -MaxGridSearch; j < MaxGridSearch; j++) {
                worldObjects[new Vector3Int(i, j, 0)] = null;
            }
        }

    }

    public bool PlaceObjectOnBoard(GameObject newObject, Vector3 objectPosition) {

        // Convert to cell coords
        Vector3Int convertedCoords = grid.WorldToCell(objectPosition);

        // Check if object already on board
        if (objectLocations.ContainsKey(newObject)) {
            worldObjects[objectLocations[newObject]] = null;
        }

        // If space is available, move object there
        if (worldObjects[convertedCoords] == null) {
            worldObjects[convertedCoords] = newObject;
            objectLocations[newObject] = convertedCoords;
            return true;
        }

        // Action was unsuccessful
        return false;
    }

}
