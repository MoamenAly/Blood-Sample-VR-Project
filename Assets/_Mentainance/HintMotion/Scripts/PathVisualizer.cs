using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scivr.Path
{
    public class PathVisualizer : MonoBehaviour
    {
        public Path[] paths;
        public int selectedPathIndex = 0;

        [SerializeField, HideInInspector]
        public int selectedPointIndex = -1;

        protected virtual void OnDrawGizmos()
        {
            if (paths == null || paths.Length == 0)
                return;

            Path selectedPathHolder = paths[selectedPathIndex];
            if (selectedPathHolder == null || selectedPathHolder.pathPoints.Count == 0)
                return;

            Gizmos.color = Color.green;
            for (int i = 0; i < selectedPathHolder.pathPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(transform.TransformPoint(selectedPathHolder.pathPoints[i].point), transform.TransformPoint(selectedPathHolder.pathPoints[i + 1].point));
            }

            for (int i = 0; i < selectedPathHolder.pathPoints.Count; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(selectedPathHolder.pathPoints[i].point), 0.1f);
            }
        }

#if UNITY_EDITOR
        public virtual void OnSceneGUI()
        {
            if (paths == null || paths.Length == 0)
                return;

            Path selectedPathHolder = paths[selectedPathIndex];
            if (selectedPathHolder == null || selectedPathHolder.pathPoints.Count == 0)
                return;

            for (int i = 0; i < selectedPathHolder.pathPoints.Count; i++)
            {
                Handles.color = Color.red;
                Handles.Label(transform.TransformPoint(selectedPathHolder.pathPoints[i].point), $"Point {i}");
                if (Handles.Button(transform.TransformPoint(selectedPathHolder.pathPoints[i].point), Quaternion.identity, 0.1f, 0.1f, Handles.SphereHandleCap))
                {
                    selectedPointIndex = i;
                    RepaintSceneView();
                }
            }

            if (selectedPointIndex >= 0 && selectedPointIndex < selectedPathHolder.pathPoints.Count)
            {
                Handles.color = Color.blue;
                PathPoint selectedPoint = selectedPathHolder.pathPoints[selectedPointIndex];

                // Ensure the quaternion is valid
                if (!IsValidQuaternion(selectedPoint.rotation))
                {
                    selectedPoint.rotation = Quaternion.identity;
                }

                EditorGUI.BeginChangeCheck();

                Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? selectedPoint.rotation : Quaternion.identity;

                Vector3 newPointPosition = Handles.PositionHandle(transform.TransformPoint(selectedPoint.point), handleRotation);
                Quaternion newPointRotation = Handles.RotationHandle(handleRotation, transform.TransformPoint(selectedPoint.point));

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Move or Rotate Path Point");
                    selectedPoint.point = transform.InverseTransformPoint(newPointPosition);
                    selectedPoint.rotation = Quaternion.Inverse(transform.rotation) * newPointRotation;

                    // Normalize the quaternion to ensure it's valid
                    selectedPoint.rotation = Quaternion.Normalize(selectedPoint.rotation);

                    EditorUtility.SetDirty(this);
                }
            }
        }

        public void RepaintSceneView()
        {
            if (SceneView.lastActiveSceneView != null)
            {
                SceneView.lastActiveSceneView.Repaint();
            }
        }

        [CustomEditor(typeof(PathVisualizer))]
        public class PathVisualizerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                PathVisualizer visualizer = (PathVisualizer)target;

                if (visualizer.paths != null && visualizer.paths.Length > 0)
                {
                    Path selectedPathHolder = visualizer.paths[visualizer.selectedPathIndex];

                    if (selectedPathHolder != null && selectedPathHolder.pathPoints.Count > 0)
                    {
                        EditorGUILayout.LabelField("Select Point");
                        visualizer.selectedPointIndex = EditorGUILayout.IntSlider(visualizer.selectedPointIndex, 0, selectedPathHolder.pathPoints.Count - 1);
                        visualizer.RepaintSceneView();
                    }
                }
            }

            void OnSceneGUI()
            {
                PathVisualizer visualizer = (PathVisualizer)target;
                visualizer.OnSceneGUI();
            }
        }

        protected bool IsValidQuaternion(Quaternion q)
        {
            return !float.IsNaN(q.x) && !float.IsNaN(q.y) && !float.IsNaN(q.z) && !float.IsNaN(q.w) &&
                   (q.x != 0 || q.y != 0 || q.z != 0 || q.w != 0);
        }
#endif
    }

    [System.Serializable]
    public class Path
    {
        [OdinSerialize, ListDrawerSettings(CustomAddFunction = nameof(CreateNewPath), DefaultExpandedState = true, Expanded = true)]
        public List<PathPoint> pathPoints = new List<PathPoint>();

        protected virtual PathPoint CreateNewPath()
        {
            var visualizer = GameObject.FindObjectOfType<PathVisualizer>();

            Vector3 newPointPosition = visualizer.transform.position;

            if (pathPoints.Count > 0)
            {
                newPointPosition = pathPoints[pathPoints.Count - 1].point;
            }

            var newPathPoint = new PathPoint()
            {
                point = newPointPosition,
                rotation = Quaternion.identity // Initialize rotation as identity
            };
            pathPoints.Add(newPathPoint);

#if UNITY_EDITOR
            if (visualizer != null)
            {
                visualizer.selectedPointIndex = pathPoints.Count - 1;
                visualizer.RepaintSceneView();
            }
#endif

            return newPathPoint;
        }
    }

    [System.Serializable]
    public class PathPoint
    {
        public Vector3 point;
        public Quaternion rotation = Quaternion.identity; // Initialize rotation as identity
    }
}
