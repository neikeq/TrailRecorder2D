using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Neikeq.TrailRecorder2D
{
	[CustomEditor(typeof(TrailRecorder))]
	public class TrailRecorderEditor : Editor
	{
		void OnEnable ()
		{
			// Add a handler to check the transitions between the play mode state
			EditorApplication.playmodeStateChanged = OnPlayModeStateChanged;
		}

		public override void OnInspectorGUI ()
		{
			TrailRecorder recorder = (TrailRecorder)target;

			EditorGUILayout.LabelField ("Base Settings", EditorStyles.boldLabel);
			EditorGUILayout.LabelField ("Snapshots: " + TrailInstances.objSnapshots.Count.ToString ());
            
			recorder.snapshotsLimit = EditorGUILayout.IntField ("Limit", recorder.snapshotsLimit);
            
			recorder.recording = EditorGUILayout.Toggle ("Recording", recorder.recording);
            
			recorder.Visible = EditorGUILayout.Toggle ("Visible", recorder.Visible);

			if (GUILayout.Button ("Destroy objects"))
				recorder.DestroySnapshots ();
            
			if (GUILayout.Button ("Clear all"))
			{
				TrailInstances.snapshots.Clear ();
				recorder.DestroySnapshots ();
			}
		}

		void OnPlayModeStateChanged ()
		{
			TrailRecorder recorder = (TrailRecorder)target;

			if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			{
				// Destroy old snapshots instances before entering play mode
				recorder.DestroySnapshots ();
			}
			else if (!EditorApplication.isPlaying && !EditorApplication.isPaused)
			{
				// All objects instantiated in play mode are destroyed when exiting,
				// so we must set the snapshots as non-instantiated
				foreach (Snapshot snapshot in TrailInstances.snapshots)
					snapshot.Instantiated = false;

				// If play mode is stopped, and we need to instantiate or snapshots
				if (TrailInstances.objContainer == null && recorder.Visible)
					recorder.InstantiateSnapshots ();
			}
		}
	}
}
