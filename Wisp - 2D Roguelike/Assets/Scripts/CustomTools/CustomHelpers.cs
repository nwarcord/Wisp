using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CustomHelpers {

    /// <summary>
	/// Helper function for testing if Unity Object is destroyed or marked for destruction
	/// </summary>
    public static bool IsNullOrDestroyed(this System.Object obj) {
            
        if (object.ReferenceEquals(obj, null)) return true;
        
        if (obj is UnityEngine.Object) return (obj as UnityEngine.Object) == null;

        return false;
    }

	/// <summary>
	/// Helper function for this class to throw errors from the CheckForInitializedFields() function
	/// </summary>
	private static void ThrowInitError(string objectName, System.Type classObj, string gameObjectName) {
#if UNITY_EDITOR
		string message = string.Format("{0} not assigned in {1} on game object {2}", objectName, classObj.Name, gameObjectName);
		Debug.LogError(message);
		EditorApplication.isPlaying = false;
#endif
	}

    public static void CheckForInitializedFields(UnityEngine.Object scriptToCheck) {

#if UNITY_EDITOR
		SerializedObject scriptSerialized = new SerializedObject(scriptToCheck);

		SerializedProperty prop = scriptSerialized.GetIterator();

		prop.NextVisible(true); // Need to pass the m_script component first.

		while (prop.NextVisible(false)) {

			//Debug.Log(prop.type);
			if (prop.propertyType != SerializedPropertyType.ObjectReference) {
				// Ignores built-in C# types like string, int, etc.
				continue;
			}
			//Debug.Log(prop.propertyType);

			if (!prop.objectReferenceValue) {
				ThrowInitError(prop.name, scriptToCheck.GetType
					(), scriptToCheck.name);
			}
		}
#endif
	}


}
