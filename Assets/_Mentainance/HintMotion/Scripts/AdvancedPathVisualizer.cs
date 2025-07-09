using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Scivr.Path
{
    public class AdvancedPathVisualizer : PathVisualizer
    {
        public Transform target;

        private void Awake()
        {
            InitializePath();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (paths == null || paths.Length == 0)
                return;

            Path selectedPathHolder = paths[selectedPathIndex];
            if (selectedPathHolder == null)
                return;

            Gizmos.color = Color.green;

            if (selectedPathHolder.pathPoints.Count == 0)
            {
                if (transform != null && target != null)
                {
                    Gizmos.DrawLine(transform.position, target.position);
                }
            }
            else
            {
                if (transform != null)
                {
                    Gizmos.DrawLine(transform.position, transform.TransformPoint(selectedPathHolder.pathPoints[0].point));
                }

                if (target != null)
                {
                    Gizmos.DrawLine(transform.TransformPoint(selectedPathHolder.pathPoints[selectedPathHolder.pathPoints.Count - 1].point), target.position);
                }
            }

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
        public override void OnSceneGUI()
        {
            base.OnSceneGUI();
        }

        [CustomEditor(typeof(AdvancedPathVisualizer))]
        public new class PathVisualizerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                AdvancedPathVisualizer visualizer = (AdvancedPathVisualizer)target;

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
                AdvancedPathVisualizer visualizer = (AdvancedPathVisualizer)target;
                visualizer.OnSceneGUI();
            }
        }
#endif

        private void InitializePath()
        {
            if (paths == null || paths.Length == 0)
            {
                paths = new Path[1];
                paths[0] = new AdvancedPath();
            }

            var path = paths[0] as AdvancedPath;

            if (path != null)
            {
                path.pathPoints.Clear();

                var startPoint = new PathPoint
                {
                    point = transform.InverseTransformPoint(transform.position),
                    rotation = Quaternion.identity
                };

                path.pathPoints.Add(startPoint);

                if (target != null)
                {
                    var endPoint = new PathPoint
                    {
                        point = transform.InverseTransformPoint(target.position),
                        rotation = Quaternion.identity
                    };

                    path.pathPoints.Add(endPoint);
                }

                selectedPointIndex = 0;
#if UNITY_EDITOR
                RepaintSceneView();
#endif
            }
        }
    }

    [System.Serializable]
    public class AdvancedPath : Path
    {
        protected override PathPoint CreateNewPath()
        {
            var visualizer = GameObject.FindObjectOfType<AdvancedPathVisualizer>();

            Vector3 newPointPosition;

            if (pathPoints.Count == 0)
            {
                newPointPosition = visualizer.transform.InverseTransformPoint(visualizer.transform.position);
            }
            else if (pathPoints.Count == 1)
            {
                newPointPosition = (pathPoints[0].point + visualizer.transform.InverseTransformPoint(visualizer.target.position)) / 2;
            }
            else
            {
                // Calculate midpoint between the last point and the target
                Vector3 lastPoint = pathPoints[pathPoints.Count - 1].point;
                Vector3 endPoint = visualizer.target != null ? visualizer.transform.InverseTransformPoint(visualizer.target.position) : visualizer.transform.position;

                newPointPosition = (lastPoint + endPoint) / 2;
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
}
