using UnityEngine;
using System.Collections;

namespace Neikeq.TrailRecorder2D
{
	public class TrailRecorder : MonoBehaviour
	{
		private Vector3 lastPosition;
		// Determines if we are recording for new snapshots
		public bool recording = true;
		// The maximum number of snapshots instances to start destroying the oldest
		// If equals zero, limitation will be ignored
		public int snapshotsLimit = 0;
		// Determines if the recorded snapshots must be instantiated
		private bool visible = true;
		// If enabled, old snapshots instances will be destroyed when entering play mode
		private bool autoDestroy;
		// Minimum distance traveled to add a new snapshots
		private float minDist = 0.25f;
		// Current distance traveled
		private float curDist = 0f;

		void Start ()
		{
			if (recording)
			{
				// First snapshot
				lastPosition = transform.position;
				AddNewSnapshot ();
			}
		}

		void FixedUpdate ()
		{
			if (recording)
			{
				// Update the current traveled distance
				curDist += Vector3.Distance (transform.position, lastPosition);

				// If our target traveled a minimum distance, add a new snapshot
				if (curDist > minDist)
				{
					AddNewSnapshot ();
					curDist = 0f;
				}
                
				lastPosition = transform.position;
			}
		}

		void AddNewSnapshot ()
		{
			CheckSnapshotsLimit ();

			Snapshot snapshot = Snapshot.FromGameObject (gameObject);

			TrailInstances.snapshots.Add (snapshot);
            
			OnNewSnapshot (snapshot);
		}

		void OnNewSnapshot (Snapshot snapshot)
		{
			if (visible)
				InstantiateSnapshot (snapshot);
		}

		void CheckSnapshotsLimit ()
		{
			if (snapshotsLimit != 0 && TrailInstances.snapshots.Count >= snapshotsLimit)
			{
				// Depending of when the limit was set, there can be one or more snapshots out of it
				int limitDif = TrailInstances.snapshots.Count - snapshotsLimit;

				// For each of these and atleast once
				int i = 0;
				do
				{
					// Remove the snapshot information
					TrailInstances.snapshots.RemoveAt (0);

					// If the snapshot is instantiated, destroy it
					if (TrailInstances.objSnapshots.Count > 0)
					{
						DestroyImmediate (TrailInstances.objSnapshots [0]);
						TrailInstances.objSnapshots.RemoveAt (0);
					}
					i++;
				}
				while (i < limitDif);
			}
		}

		public void InstantiateSnapshots ()
		{
			foreach (Snapshot snapshot in TrailInstances.snapshots)
				InstantiateSnapshot (snapshot);
		}

		void InstantiateSnapshot (Snapshot snapshot)
		{
			if (!snapshot.Instantiated)
			{
				// If there is no container, create it
				if (TrailInstances.objContainer == null)
				{
					TrailInstances.objContainer = new GameObject ();
					TrailInstances.objContainer.name = "snapshots-" + gameObject.name;
					TrailInstances.objContainer.transform.position = Vector3.zero;
				}
            
				// Instantiate the snapshot with objContainer as parent
				GameObject objNew = snapshot.ToGameObject ("snapshot#" + TrailInstances.objSnapshots.Count.ToString (),
				                                           TrailInstances.objContainer);
            
				TrailInstances.objSnapshots.Add (objNew);
            
				snapshot.Instantiated = true;
			}
		}

		public void DestroySnapshots ()
		{
			if (TrailInstances.objContainer != null)
			{
				// By destroying the container, we destroy all its snapshots childs
				DestroyImmediate (TrailInstances.objContainer);

				// Clear the list of snapshots (which should be all null now)
				TrailInstances.objSnapshots.Clear ();

				foreach (Snapshot snapshot in TrailInstances.snapshots)
					snapshot.Instantiated = false;
			}
		}

		public bool Visible {
			get { return visible; }
			set {
				bool activated = visible != value && value;
                
				visible = value;
                
				if (activated)
				{
					if (TrailInstances.objContainer != null)
					{
						foreach (Snapshot snapshot in TrailInstances.snapshots)
						{
							if (!snapshot.Instantiated)
								InstantiateSnapshot (snapshot);
						}
                        
						// Ensure our snapshots are visible
						TrailInstances.objContainer.SetActive (true);
					}
					else
					{
						InstantiateSnapshots ();
					}
				}
				else if (!value)
				{
					// If visible is set to false, hide the snapshots
					if (TrailInstances.objContainer != null)
						TrailInstances.objContainer.SetActive (false);
				}
			}
		}
	}
}
