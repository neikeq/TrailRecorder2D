using UnityEngine;
using System.Collections;

namespace Neikeq.TrailRecorder2D
{
	public class Snapshot
	{
		private const float opacity = 0.25f;

		public Vector2 Position { get; set; }

		public Quaternion Rotation { get; set; }

		public Vector3 LocalScale { get; set; }

		public Sprite Sprite { get; set; }

		public bool Instantiated { get; set; }

		public GameObject ToGameObject (string name, GameObject parent)
		{
			GameObject obj = new GameObject ();
            
			obj.name = name;

			if (parent != null)
				obj.transform.parent = parent.transform;
            
			// Set the transform values
			obj.transform.position = Position;
			obj.transform.localScale = LocalScale;
			obj.transform.rotation = Rotation;
            
			// Set the sprite
			SpriteRenderer renderer = obj.AddComponent<SpriteRenderer> ();
			renderer.sprite = Sprite;
            
			// Set the sprite opacity
			Color sharedColor = renderer.color;
			Color color = new Color (sharedColor.r, sharedColor.g, sharedColor.b, opacity);
			renderer.color = color;

			return obj;
		}

		public static Snapshot FromGameObject (GameObject gameObject)
		{
			Snapshot snapshot = new Snapshot ();
            
			snapshot.Position = gameObject.transform.position;
			snapshot.LocalScale = gameObject.transform.localScale;
			snapshot.Rotation = gameObject.transform.rotation;
			snapshot.Sprite = gameObject.GetComponent<SpriteRenderer> ().sprite;

			return snapshot;
		}

		public Snapshot ()
		{
			Instantiated = false;
		}
	}
}