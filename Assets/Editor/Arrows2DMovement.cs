using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Arrows2DMovement {

    static Arrows2DMovement() {
        //avoid registering twice to the SceneGUI delegate
        SceneView.onSceneGUIDelegate -= OnSceneView;
        SceneView.onSceneGUIDelegate += OnSceneView;
    }

    static void OnSceneView(SceneView sceneView) {
        if (Tools.current != Tool.Move) { return; }

        Event currentEvent = Event.current;
        bool keyDown = currentEvent.isKey && currentEvent.type == EventType.KeyDown;
        bool arrowKeys = (currentEvent.modifiers == EventModifiers.None
                         || currentEvent.modifiers == EventModifiers.FunctionKey); //arrow keys are function keys
        //if the event is a keyDown on an orthographic camera
        if (keyDown && arrowKeys && sceneView.camera.orthographic) {
            Vector3 movement = Vector3.zero;
            float xStep = EditorPrefs.GetFloat("MoveSnapX");
            float yStep = EditorPrefs.GetFloat("MoveSnapY");
            Debug.Log("MOVE STEP: " + xStep);

            switch (currentEvent.keyCode) {
                case KeyCode.RightArrow:
                    movement.x = xStep;
                    break;
                case KeyCode.LeftArrow:
                    movement.x = -xStep;
                    break;
                case KeyCode.UpArrow:
                    movement.y = yStep;
                    break;
                case KeyCode.DownArrow:
                    movement.y = -yStep;
                    break;
            }

            moveSelectedObjects(movement, xStep, yStep);
        }

    }

    static void clampPosition(ref Vector3 position, float xStep, float yStep) {
        position.x = Mathf.Round(position.x / xStep) * xStep;
        position.y = Mathf.Round(position.y / yStep) * yStep;
    }

    static void moveSelectedObjects(Vector3 movement, float xStep, float yStep) {
        UnityEngine.Object[] selectedObjects = Selection.GetFiltered(typeof(GameObject),
                                                 SelectionMode.Editable | SelectionMode.ExcludePrefab);
        for (int i = 0; i < selectedObjects.Length; i++) {
            Transform objectTransform = (selectedObjects[i] as GameObject).transform;
            Undo.RecordObject(objectTransform, "Move Step"); //allow undo of the movements
            Vector3 newPosition = objectTransform.position + movement;
            clampPosition(ref newPosition, xStep, yStep);
            objectTransform.position = newPosition;
        }

        // only consume the event if there was at least one gameObject selected, otherwise the camera will move as usual
        if (selectedObjects.Length > 0) {
            Event.current.Use();
        }

    }

}