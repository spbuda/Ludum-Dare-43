using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Tools {
	public static class Tags {
		public static bool IsDefault(Component obj) {
			return obj.tag == Untagged;
		}
		public static bool IsPlayer(Component obj) {
			return obj.tag == Player;
		}
		public static bool IsActor(Component obj) {
			return obj.tag == Actor;
		}
		public static readonly string Untagged = "Untagged";
		public static readonly string Player = "Player";
		public static readonly string Actor = "Actor";
		public static readonly string VulnerablePoint = "VulnerablePoint";
		public static readonly string RoomActive = "RoomActive";
		public static readonly string PlayerActive = "PlayerActive";
	}

	public static class Layers {
		public static bool IsDefault(Component obj) {
			LayerMask mask = LayerMask.NameToLayer (Default);
			return obj.gameObject.layer == mask;
		}
		public static bool IsActors(Component obj) {
			LayerMask mask = LayerMask.NameToLayer (Actors);
			return obj.gameObject.layer == mask;
		}
		public static readonly string Default = "Default";
		public static readonly string Collisions = "Collisions";
		public static readonly string Actors = "Actors";
		public static readonly string Interactable = "Interactable";
		public static readonly string CollisionsView = "CollisionsView";
		public static readonly string RoomBounds = "RoomBounds";
		public static readonly string CollisionsParent = "CollisionsParent";
		public static readonly string VulnerablePoint = "VulnerablePoint";
	}

	public static class Teyrutils {
		public static T GetAsset<T>(string name = null, bool forceGetByName = false) where T : ScriptableObject {
			T _inst = Resources.FindObjectsOfTypeAll<T> ().FirstOrDefault ();
			if (!_inst || forceGetByName) {
				if (name != null || forceGetByName) {
					_inst = AssetDatabase.LoadAssetAtPath<T> (name);
					Debug.Assert (_inst != null, "No asset found at path '" + name + "'. Make sure name is correct.");
				} else {
					_inst = ScriptableObject.CreateInstance<T> ();
					Debug.Assert (_inst != null, "No asset of type '" + typeof (T) + "' could be created.");
				}
				_inst.hideFlags = HideFlags.HideAndDontSave;
			}
			return _inst;
		}

		public static T GetEvent<T>(string name) where T : ScriptableObject {
			T _inst = null;
			_inst = AssetDatabase.LoadAssetAtPath<T> ("Assets/Events/" + name + ".asset");
			Debug.Assert (_inst != null, "No asset found at path '" + name + "'. Make sure name is correct.");
			_inst.hideFlags = HideFlags.HideAndDontSave;
			return _inst;
		}

		public static string CapitalizeWord(string s) {
			// Check for empty string.
			if (string.IsNullOrEmpty (s)) {
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper (s[0]) + s.Substring (1);
		}

		public static bool IsVulnerable(Component obj) {
			return HasTag (obj, Tags.VulnerablePoint);
		}

		public static bool HasTag(Component obj, string tag) {
			return obj.tag.Contains (tag);
		}

		public static bool HasTag(Component obj, params string[] tags) {
			foreach(string tag in tags) {
				if(obj.tag.Contains(tag)) {
					return true;
				}
			}
			return false;
		}

		public static bool IsVulnerable(GameObject obj) {
			return IsVulnerable (obj.transform);
		}

		public static bool HasTag(GameObject obj, string tag) {
			return HasTag (obj.transform, tag);
		}

		public static bool HasTag(GameObject obj, params string[] tags) {
			return HasTag (obj.transform, tags);
		}

		public static T FindObjectUpwards<T> (Component obj) where T : MonoBehaviour {
			T parent = null;
			Transform t = obj.transform;
			parent = t.GetComponent<T> ();
			while (t.parent != null && parent == null) {
				t = t.parent.transform;
				parent = t.GetComponent<T> ();
			}

			return parent;
		}
	}

	public enum PauseType {
		Pause, Menu, SecondaryMenu
	}
}
