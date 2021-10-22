
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bridge))]
public class BridgeEditor : Editor
{
    string[] _choices = new[] { "Right", "Down", "Left", "Up" };
    int _choiceIndex = -1;
    Vector3 LEFT = new Vector3(0, 0, 0), UP = new Vector3(0, 0, -90f), RIGHT = new Vector3(0, 0, 180f), DOWN = new Vector3(0, 0, 90f);

    public override void OnInspectorGUI()
    {
        Bridge myTarget = (Bridge)target;

        DrawDefaultInspector();

        if (_choiceIndex < 0)
            _choiceIndex = Mathf.Max(System.Array.FindIndex(_choices, type => string.Equals(type, myTarget.direction)), 0);

        _choiceIndex = EditorGUILayout.Popup("Direction", _choiceIndex, _choices);
        switch (_choices[_choiceIndex])
        {
            case "Right":
                myTarget.gameObject.transform.eulerAngles = RIGHT;
                break;
            case "Down":
                myTarget.gameObject.transform.eulerAngles = DOWN;
                break;
            case "Left":
                myTarget.gameObject.transform.eulerAngles = LEFT;
                break;
            case "Up":
                myTarget.gameObject.transform.eulerAngles = UP;
                break;
                default:
                break;
        }
        // Update the selected choice in the underlying object
        myTarget.direction = _choices[_choiceIndex];
        EditorUtility.SetDirty(myTarget);
    }
}

