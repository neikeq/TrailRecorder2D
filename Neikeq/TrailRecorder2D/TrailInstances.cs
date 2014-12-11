using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neikeq.TrailRecorder2D
{
	public static class TrailInstances
	{
		// Snapshots data
		public static List<Snapshot> snapshots = new List<Snapshot> ();
		// Snapshots container
		public static GameObject objContainer;
		// Snapshots intances
		public static List<GameObject> objSnapshots = new List<GameObject> ();
	}
}
