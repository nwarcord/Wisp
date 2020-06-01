using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
	//[SerializeField]
	private Transform targetTransform;

	[SerializeField]
	private Camera thisCamera = default;

	[SerializeField]
	private Collider2D minBoundaryX = default, maxBoundaryX = default, minBoundaryY = default, maxBoundaryY = default;

	private float xMin, xMax, yMin, yMax;

	private float xOffset, yOffset;

	private void Awake() {
		if (GameObject.FindGameObjectsWithTag("MainCamera").Length > 1)
			// Destroy any extra cameras.
			Destroy(transform.parent.gameObject);

		CustomHelpers.CheckForInitializedFields(this);

		// DontDestroyOnLoad(gameObject);
		xOffset = thisCamera.orthographicSize * Screen.width / Screen.height;
		yOffset = thisCamera.orthographicSize;

		xMin = minBoundaryX.bounds.min.x + xOffset;
		xMax = maxBoundaryX.bounds.max.x - xOffset;
		yMin = minBoundaryY.bounds.min.y + yOffset;
		yMax = maxBoundaryY.bounds.max.y - yOffset;
	}

	void Start() {

        targetTransform = GameObject.FindWithTag("Player").transform;
		//Debug.Log(targetTransform.position);
    }

    void LateUpdate() {
		Follow();
    }

	private void Follow() {

		Vector3 newPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.localPosition.z);

		float xClamped = Mathf.Clamp(newPosition.x, xMin, xMax);
		float yClamped = Mathf.Clamp(newPosition.y, yMin, yMax);

		transform.localPosition = new Vector3(xClamped, yClamped, transform.localPosition.z);

		// transform.localPosition = newPosition;
	}
}
