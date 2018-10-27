﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditor {
    [CreateAssetMenu(fileName = "Prefab direction brush", menuName = "Brushes/Prefab direction brush")]
    [CustomGridBrush(false, true, false, "Prefab Brush")]
    public class PrefabDirectionBrush: GridBrushBase {
        private const float k_PerlinOffset = 100000f;
        public GameObject[] m_Prefabs;
        public float z_rotation;
        public bool upsideDown = false;
        public float m_PerlinScale = 0.5f;
        public int m_Z;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, k_PerlinOffset) * m_Prefabs.Length), 0, m_Prefabs.Length - 1);
            GameObject prefab = m_Prefabs[index];
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (instance != null) {
                Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
                Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
                instance.transform.SetParent(brushTarget.transform);
                instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, m_Z) + new Vector3(.5f, .5f, .5f)));
                instance.transform.rotation = new Quaternion(instance.transform.rotation.x, instance.transform.rotation.y, z_rotation, instance.transform.rotation.w);
                if (upsideDown) {
                    instance.transform.localScale = new Vector3(instance.transform.localScale.x, -instance.transform.localScale.y, instance.transform.localScale.z);
                }
            }
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, m_Z));
            if (erased != null)
                Undo.DestroyObjectImmediate(erased.gameObject);
        }

        private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position) {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Bounds bounds = new Bounds((max + min) * .5f, max - min);

            for (int i = 0; i < childCount; i++) {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position))
                    return child;
            }
            return null;
        }

        private static float GetPerlinValue(Vector3Int position, float scale, float offset) {
            return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
        }
    }
    
}
