using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
	public abstract class Ease : Enumeration {
		#region Enumeration
		public static readonly Ease Constant = new EaseConstant ();
		public static readonly Ease Linear = new EaseLinear ();
		public static readonly Ease Smooth = new EaseSmooth ();
		public static readonly Ease Fall = new EaseFall ();

		private Ease() { }
		private Ease(int value, string displayName) : base (value, displayName) { }

		public abstract float From(float start, float end, float currentTime, float maxTime);
		public virtual Vector2 From(Vector2 start, Vector2 end, float currentTime, float maxTime) {
			return new Vector2 () {
				x = From (start.x, end.x, currentTime, maxTime),
				y = From (start.y, end.y, currentTime, maxTime)
			};
		}
		public virtual Vector3 From(Vector3 start, Vector3 end, float currentTime, float maxTime) {
			return new Vector3 () {
				x = From (start.x, end.x, currentTime, maxTime),
				y = From (start.y, end.y, currentTime, maxTime),
				z = From (start.z, end.z, currentTime, maxTime)
			};
		}
		public virtual Color From(Color start, Color end, float currentTime, float maxTime) {
			return new Color () {
				r = From (start.r, end.r, currentTime, maxTime),
				g = From (start.g, end.g, currentTime, maxTime),
				b = From (start.b, end.b, currentTime, maxTime),
				a = From (start.a, end.a, currentTime, maxTime)
			};
		}
		#endregion

		#region Constant
		private class EaseConstant : Ease {
			public EaseConstant() : base (0, "Constant") { }
			public override float From(float start, float end, float currentTime, float maxTime) {
				bool finished = currentTime >= maxTime;
				float current;

				if (finished) {
					current = end;
				} else {
					current = start;
				}
				return current;
			}
		}
		#endregion

		#region Linear
		private class EaseLinear : Ease {
			public EaseLinear() : base (0, "Linear") { }
			public override float From(float start, float end, float currentTime, float maxTime) {
				bool finished = currentTime >= maxTime;
				float current;

				if (finished) {
					current = end;
				} else {
					float diff = end - start;
					float percentPosition = currentTime / maxTime;

					current = start + (diff * percentPosition);
				}

				return current;
			}
		}
		#endregion

		#region Smooth
		private class EaseSmooth : Ease {
			public EaseSmooth() : base (0, "Smooth") { }
			public override float From(float start, float end, float currentTime, float maxTime) {
				bool finished = currentTime >= maxTime;
				float current;

				if (finished) {
					current = end;
				} else {
					float avg = (start + end) * .5f;
					float percentPosition = currentTime / maxTime;

					current = avg + (end - avg) * Mathf.Cos (Mathf.PI + Mathf.PI * percentPosition);
				}
				return current;
			}
		}
		#endregion

		#region Fall
		private class EaseFall : Ease {
			public EaseFall () : base (0, "Fall") { }
			public override float From(float start, float end, float currentTime, float maxTime) {
				bool finished = currentTime >= maxTime;
				float current;

				if (finished) {
					current = end;
				} else {
					float diff = end - start;
					float percentPosition = currentTime / maxTime;

					current = end + (diff * Mathf.Cos (Mathf.PI + .5f * Mathf.PI * percentPosition));
				}
				return current;
			}
		}
		#endregion
	}
}
