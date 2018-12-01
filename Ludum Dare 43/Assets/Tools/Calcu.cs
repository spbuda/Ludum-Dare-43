using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
	public static class Calcu {
		private const float epsilon = .001f;
		public static bool Close(Vector3 v1, Vector3 v2, float distance) {
			float magSquare = (v2 - v1).sqrMagnitude;
			return (magSquare < (distance*distance));
		}

		public static bool Ish(Vector2 v1, Vector2 v2) {
			return Ish (v1.x, v2.x) && Ish (v1.y, v2.y);
		}

		public static float Mod(float a, float b) {
			if(b == 0) {
				return 0f;
			}
			return ((a % b) + b) % b;
		}

		public static int Mod(int a, int b) {
			if (b == 0) {
				return 0;
			}
			return ((a % b) + b) % b;
		}

		public static bool GreaterOrIsh(float a, float b) {
			return Ish (a, b) || a > b;
		}

		public static bool LessOrIsh(float a, float b) {
			return Ish (a, b) || a < b;
		}

		public static bool Ish(float a, float b) {
			//Handle NaN case
			if (float.IsNaN (a) && float.IsNaN (b)) {
				return true;
			} else if (float.IsNaN (a) || float.IsNaN (b)) {
				return false;
			}

			float absA = Mathf.Abs (a);
			float absB = Mathf.Abs (b);
			float diff = Mathf.Abs (a - b);

			if (a == b) { // shortcut, handles infinities
				return true;
			} else if (a == 0 || b == 0 || diff < epsilon) {
				// a or b is zero or both are extremely close to it
				// relative error is less meaningful here
				return diff < epsilon;
			} else { // use relative error
				return diff / (absA + absB) < epsilon;
			}
		}

		public static bool Ish(Vector3 v1, Vector3 v2) {
			return Ish (v1.x, v2.x) && Ish (v1.y, v2.y) && Ish (v1.z, v2.z);
		}

		public static bool Ish(Edge2D e1, Edge2D e2) {
			return Ish (e1.Point1, e2.Point1) && Ish (e1.Point2, e2.Point2);
		}

		public static bool Ish(Triangle2D e1, Triangle2D e2) {
			return Ish (e1.Point1, e2.Point1) && Ish (e1.Point2, e2.Point2) && Ish (e1.Point3, e2.Point3);
		}

		public static bool ZeroIsh(float v) {
			return Ish (v, 0f);
		}

		public static bool ZeroIsh(Vector3 v) {
			return Ish (v, Vector3.zero);
		}

		public static Vector2 Compare(bool xMax, bool yMax, params Vector2[] points) {
			Vector2 extremePoint = points[0];
			for (int i = 1; i < points.Length; i++) {
				Vector2 currentPoint = points[i];
				if ((xMax && currentPoint.x > extremePoint.x) || (!xMax && currentPoint.x < extremePoint.x)) {
					extremePoint = currentPoint;
				} else if (currentPoint.x == extremePoint.x && ((yMax && currentPoint.y > extremePoint.y) || (!yMax && currentPoint.y < extremePoint.y))) {
					extremePoint = currentPoint;
				}
			}
			return extremePoint;
		}

		public static Vector2 Max(params Vector2[] points) {
			return Compare (true, true, points);
		}

		public static Vector2 Min(params Vector2[] points) {
			return Compare (false, false, points);
		}

		public static Vector2 Max(out Vector2[] otherPoints, params Vector2[] points) {
			Vector2 maxPoint = Max(points);
			otherPoints = new Vector2[points.Length - 1];
			int offset = 0;
			for (int i = 0; i < points.Length; i++) {
				if (offset == 0 && Ish (points[i], maxPoint)) {
					offset++;
				} else {
					otherPoints[i - offset] = points[i];
				}
			}
			return maxPoint;
		}

		public static Vector2 Min(out Vector2[] otherPoints, params Vector2[] points) {
			Vector2 minPoint = Min (points);
			otherPoints = new Vector2[points.Length - 1];
			int offset = 0;
			for (int i = 0; i < points.Length; i++) {
				if (offset == 0 && Ish (points[i], minPoint)) {
					offset++;
				} else {
					otherPoints[i - offset] = points[i];
				}
			}
			return minPoint;
		}

		public static float Area(List<Vector2> points) {
			float area = 0f;
			int j = points.Count - 1;
			for(int i=0; i<points.Count; i++) {
				area += (points[j].x + points[i].x) * (points[j].y - points[i].y);
				j = i;
			}
			return area / 2f;
		}

		public static bool IsReal(float f) {
			return !float.IsInfinity (f) && !float.IsNaN (f);
		}

		public static bool IsReal(Vector3 v) {
			return IsReal (v.x) && IsReal (v.y) && IsReal (v.z);
		}

		/**
		* This is an implementation of Welzl's Algorithm to determine the minimum circle enclosing a set of points.
		*	This is a recursive solver for this problem. More information can be found https://en.wikipedia.org/wiki/Smallest-circle_problem
		*	Specific algorithm description was found: http://www.sunshine2k.de/coding/java/Welzl/Welzl.html
		*	
		*	In the typical definition of this algorithm, P = allPoints, R = boundaryPoints.
		*/
		public static Circle2D WelzlMinimumCircle(List<Vector2> allPoints, List<Vector2> boundaryPoints) {
			Circle2D circle;
			if (allPoints.Count <= 0 || boundaryPoints.Count >= 3) {
				circle = new Circle2D (boundaryPoints.ToArray ());
			} else {
				List<Vector2> bisection = new List<Vector2> (allPoints);
				List<Vector2> boundary = new List<Vector2> (boundaryPoints);
				Vector2 point = bisection[0];
				bisection.RemoveAt (0);
				circle = WelzlMinimumCircle (bisection, boundary);
				if (!circle.Contains (point)) {
					boundary.Add (point);
					circle = WelzlMinimumCircle (bisection, boundary);
				}
			}
			return circle;
		}

		public static Circle2D ExtremeCircle(List<Vector2> allPoints) {
			Vector2 xyMax = Calcu.Max (allPoints.ToArray ());
			Vector2 xyMin = Calcu.Min (allPoints.ToArray ());
			Vector2 xyMixed = Calcu.Compare (false, true, allPoints.ToArray ());
			return new Circle2D (xyMax, xyMin, xyMixed);
		}
	}

	public struct IntersectingEdge3D {
		public static IntersectingEdge3D None { get { return new IntersectingEdge3D (IntersectingEdgeType.None); } }

		public Intersection3D Start { get; }
		public Intersection3D End { get; }
		public IntersectingEdgeType Type { get; }

		private IntersectingEdge3D(IntersectingEdgeType type) {
			Start = Intersection3D.None;
			End = Intersection3D.None;

			Type = type;
		}

		public IntersectingEdge3D(Intersection3D start, Intersection3D end) {
			Start = start;
			End = end;

			if(Start.Type == IntersectionType.Complete || End.Type == IntersectionType.Complete) {
				Type = IntersectingEdgeType.Edge;
			} else if (Start.Type == IntersectionType.Point && End.Type == IntersectionType.Point) {
				if(Calcu.Ish (Start.Point, End.Point)) {
					Type = IntersectingEdgeType.Corner;
				} else {
					Type = IntersectingEdgeType.Face;
				}
			} else {
				Type = IntersectingEdgeType.None;
			}
		}
	}

	public enum IntersectingEdgeType {
		None = 0, Corner = 1, Edge = 2, Face = 3
	}

	public enum IntersectionType {
		None = 0, Point = 1, Complete = 2
	}

	public struct Intersection3D {
		public static Intersection3D None { get { return new Intersection3D (IntersectionType.None, Line3D.None); } }

		public IntersectionType Type { get; }
		public Vector3 Point { get; }

		public Line3D InputEdge { get; }

		public Intersection3D(IntersectionType type, Line3D intersectedEdge) {
			Type = type;
			Point = Vector3.positiveInfinity;

			InputEdge = intersectedEdge;
		}

		public Intersection3D(Vector3 point, Line3D intersectedEdge) {
			Type = IntersectionType.Point;
			Point = point;

			InputEdge = intersectedEdge;
		}

		public Intersection3D(Line3D intersectedEdge) {
			Type = IntersectionType.Complete;
			Point = intersectedEdge.Point1;

			InputEdge = intersectedEdge;
		}

		public override string ToString() {
			return Type + ": " + Point;
		}
	}

	public struct Plane3D {
		public Vector3 Point { get; }
		public Vector3 Normal { get; }

		public Plane3D(Vector3 point, Vector3 normal) {
			Point = point;
			Normal = normal.normalized;
		}

		private float NormalDot(Vector3 vector) {
			return Vector3.Dot (Normal, vector);
		}

		public Intersection3D Intersection(Line3D line, bool segment = false) {
			Intersection3D intersection = new Intersection3D(IntersectionType.None, line);
			Vector3 direction = line.Direction ();

			//float directionDot = NormalDot(direction);
			//bool parallel = Calcu.ZeroIsh (directionDot);

			//if (parallel) {
			//	bool onPlane = Calcu.ZeroIsh (NormalDot (line.Point1 - Point));
			//	if (onPlane) {
			//		intersection = line.Point1;
			//	}
			//} else {
			//	float intersectDot = NormalDot (Point - line.Point1) / directionDot;
			//	if(0 <= intersectDot && intersectDot <= 1) {
			//		float h = (Normal.x * line.Point1.x) + (Normal.y * line.Point1.y) + (Normal.z * line.Point1.z);
			//		float g = (Normal.x * Point.x) + (Normal.y * Point.y) + (Normal.z * Point.z);
			//		float f = (Normal.x * direction.x) + (Normal.y * direction.y) + (Normal.z * direction.z);

			//		float t = (g - h) / f;
			//		Debug.LogFormat ("h: {0}\tg: {1}\tf: {2}\n t: {3}", h, g, f, t);

			//		float x = line.Point1.x + (direction.x * t);
			//		float y = line.Point1.y + (direction.y * t);
			//		float z = line.Point1.z + (direction.z * t);
			//		Vector3 potentialIntersection = new Vector3 (x, y, z);

			//		if (segment) {
			//			if (line.OnSegment (potentialIntersection)) {
			//				intersection = potentialIntersection;
			//			}
			//		} else {
			//			intersection = potentialIntersection;
			//		}
			//	}
			//}

			float h = (Normal.x * line.Point1.x) + (Normal.y * line.Point1.y) + (Normal.z * line.Point1.z);
			float g = (Normal.x * Point.x) + (Normal.y * Point.y) + (Normal.z * Point.z);
			float f = (Normal.x * direction.x) + (Normal.y * direction.y) + (Normal.z * direction.z);

			float t = (g - h) / f;
			//Debug.LogFormat ("h: {0}\tg: {1}\tf: {2}\n t: {3}", h, g, f, t);

			if (float.IsNaN (t)) {
				bool onPlane = Calcu.ZeroIsh (NormalDot (line.Point1 - Point));
				if (onPlane) {
					intersection = new Intersection3D(line);
				}
			} else {
				float x = line.Point1.x + (direction.x * t);
				float y = line.Point1.y + (direction.y * t);
				float z = line.Point1.z + (direction.z * t);
				Vector3 potentialIntersection = new Vector3 (x, y, z);

				if (segment) {
					float check = float.NaN;
					float tx = (x - line.Point1.x) / (line.Point2.x - line.Point1.x);
					if (!float.IsNaN (tx)) {
						check = tx;
					} else {
						float ty = (y - line.Point1.y) / (line.Point2.y - line.Point1.y);
						if (!float.IsNaN (ty)) {
							check = ty;
						} else {
							float tz = (z - line.Point1.z) / (line.Point2.z - line.Point1.z);
							if (!float.IsNaN (tz)) {
								check = tz;
							}
						}
					}
					//Debug.Log ("Checking segment: " + check + ")");

					if (check >= 0f && check <= 1f) {
						intersection = new Intersection3D (potentialIntersection, line);
					}
				} else {
					intersection = new Intersection3D (potentialIntersection, line);
				}
			}



			return intersection;
		}
	}

	public struct Line3D {
		public static Line3D None { get { return new Line3D (Vector3.positiveInfinity, Vector3.positiveInfinity); } }

		public Vector3 Point1 { get; }
		public Vector3 Point2 { get; }

		/// <summary>
		/// Simple wrapper for Point1
		/// </summary>
		public Vector3 Start { get { return Point1; } }
		/// <summary>
		/// Simple wrapper for Point2
		/// </summary>
		public Vector3 End { get { return Point2; } }

		public Line3D(Vector3 point1, Vector3 point2) {
			Point1 = point1;
			Point2 = point2;
		}

		public bool OnSegment(Vector3 check) {
			float cross = (check.y - Point1.y) * (Point2.x - Point1.x) - (check.x - Point1.x) * (Point2.y - Point1.y);
			if (!Calcu.ZeroIsh (cross)) {
				return false;
			}

			float dot = (check.x - Point1.x) * (Point2.x - Point1.x) + (check.y - Point2.y) * (Point2.y - Point1.y);
			if (dot < 0f) {
				return false;
			}

			float lengthSqr = (Point2.x - Point1.x) * (Point2.x - Point1.x) + (Point2.y - Point1.y) * (Point2.y - Point1.y);
			if (lengthSqr < dot) {
				return false;
			}

			return true;
		}

		public Vector3 Direction() {
			return Point2 - Point1;
		}

		public override string ToString() {
			return "1:(" + Point1.x + "," + Point1.y + "," + Point1.z + ") -> 2:(" + Point2.x + "," + Point2.y + "," + Point2.z + ")";
		}
	}

	public struct Circle2D {
		public Vector2 EdgePoint1 { get; }
		public Vector2 EdgePoint2 { get; }
		public Vector2 EdgePoint3 { get; }
		public Vector2 Center { get; }
		public float Radius { get; }

		/**
		 *  vectors = 3 points. Any more points won't be used, any less and you get a kinda iffy circle.
		 *  3 is the magic number for generating a good circle.
		 *	The code was borrowed heavily from http://csharphelper.com/blog/2016/09/draw-a-circle-through-three-points-in-c/
		 */
		public Circle2D(params Vector2[] vectors) {
			//In theory, 3 pa
			Vector2 a = vectors.Length > 0 ? vectors[0] : Vector2.zero;
			Vector2 b = vectors.Length > 1 ? vectors[1] : a;
			Vector2 c = vectors.Length > 2 ? vectors[2] : a;

			EdgePoint1 = a;
			EdgePoint2 = b;
			EdgePoint3 = c;

			//Perpendicular bisector of a and b
			Vector2 v1 = (b + a) / 2f;
			Vector2 d1 = v1 + new Vector2 (-(b.y - a.y), b.x - a.x);

			//Perpendicular bisector of b and c
			Vector2 v2 = (c + b) / 2f;
			Vector2 d2 = v2 + new Vector2 (-(c.y - b.y), c.x - b.x);

			//Find intersects
			bool linesIntersect, segmentsIntersect;
			Vector2 intersection, close1, close2;

			Circle2D.FindIntersection (v1, d1, v2, d2, out linesIntersect, out segmentsIntersect, out intersection, out close1, out close2);
			if (!linesIntersect) {
				Center = Vector2.zero;
				Radius = 0f;
			} else {
				Center = intersection;
				Radius = (Center - a).magnitude;
			}
		}

		public bool Contains(Vector2 point) {
			return (Center - point).magnitude <= Radius;
		}

		public static void FindIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
			out bool linesIntersect, out bool segmentsIntersect, out Vector2 intersection,
			out Vector2 closeP1, out Vector2 closeP2) {
			//Get segment's parameters.
			Vector2 d12 = p2 - p1;
			Vector2 d34 = p4 - p3;

			//Sove for t1 and t2
			float denominator = ((d12.y * d34.x) - (d12.x * d34.y));

			float t1 = ((p1.x - p3.x) * d34.y + (p3.y - p1.y) * d34.x) / denominator;
			if (float.IsInfinity (t1)) {
				//The lines are ~ parallel
				linesIntersect = false;
				segmentsIntersect = false;
				intersection = new Vector2 (float.NaN, float.NaN);
				closeP1 = new Vector2 (float.NaN, float.NaN);
				closeP2 = new Vector2 (float.NaN, float.NaN);
			} else {
				linesIntersect = true;

				float t2 = ((p3.x - p1.x) * d12.y + (p1.y - p3.y) * d12.x) / (-denominator);

				//Find the point of intersection
				intersection = new Vector2 (p1.x + (d12.x * t1), p1.y + (d12.y * t1));

				//The segments intersect if t1 and t2 are between 0 and 1.
				segmentsIntersect = (t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1);

				//Find the closest points on the segments.
				t1 = Mathf.Clamp (t1, 0f, 1f);
				t2 = Mathf.Clamp (t2, 0f, 1f);

				closeP1 = new Vector2 (p1.x + (d12.x * t1), p1.y + (d12.y * t1));
				closeP2 = new Vector2 (p3.x + (d34.x * 21), p3.y + (d34.y * t2));
			}
		}

		public override string ToString() {
			return Center + " r=" + Radius;
		}
	}

	public struct Edge2D : IEquatable<Edge2D> {
		public Vector2 Point1 { get; }
		public Vector2 Point2 { get; }

		public static Edge2D NaN {
			get {
				return new Edge2D (new Vector2 (float.NaN, float.NaN));
			}
		}

		private Edge2D(Vector2 value) {
			Point1 = value;
			Point2 = value;
		}

		public Edge2D(Vector3 p1, Vector3 p2) {
			//Sorts points by x
			Vector2[] remaining;
			this.Point1 = Calcu.Min (out remaining, p1, p2);
			this.Point2 = Calcu.Min (out remaining, remaining);
		}

		public bool IsNaN() {
			Vector2 nanVector = new Vector2 (float.NaN, float.NaN);
			return Calcu.Ish (nanVector, Point1) || Calcu.Ish (nanVector, Point2);
		}

		public bool HasPoint(Vector2 point) {
			return Calcu.Ish (point, Point1) || Calcu.Ish (point, Point2);
		}

		public bool IsConnected(Edge2D e) {
			return this != e
				&& (Calcu.Ish (e.Point1, Point1)
					|| Calcu.Ish (e.Point1, Point2)
					|| Calcu.Ish (e.Point2, Point1)
					|| Calcu.Ish (e.Point2, Point2));
		}

		public bool IsValid() {
			return Point1 != Point2;
		}

		public Vector2 OtherPoint(Vector2 point) {
			if (Calcu.Ish (point, Point1)) {
				return Point2;
			} else if (Calcu.Ish (point, Point2)) {
				return Point1;
			} else {
				throw new UnityException ("No opposite point to " + point + " in: " + this);
			}
		}

		public static bool operator ==(Edge2D e1, Edge2D e2) {
			if (object.ReferenceEquals (e2, null)) {
				return object.ReferenceEquals (e1, null);
			}
			return Calcu.Ish (e1, e2);
		}

		public static bool operator !=(Edge2D e1, Edge2D e2) {
			return !(e1 == e2);
		}

		public override bool Equals(object edge) {
			return edge is Edge2D && Equals ((Edge2D) edge);
		}

		public bool Equals(Edge2D other) {
			return this == other;
		}

		public override int GetHashCode() {
			unchecked { //Integer overflow isn't a worry since this just needs to be a deterministic value.
				int hash = 11;
				hash = hash * 59 + Mathf.Round (Point1.x * 100).GetHashCode ();
				hash = hash * 109 + Mathf.Round (Point1.y * 100).GetHashCode ();
				hash = hash * 557 + Mathf.Round (Point2.x * 100).GetHashCode ();
				hash = hash * 1009 + Mathf.Round (Point2.y * 100).GetHashCode ();

				return hash;
			}
		}

		public override string ToString() {
			return Point1 + " -> " + Point2;
		}

	}

	public struct Triangle2D : IEquatable<Triangle2D> {
		public enum VectorPoint {
			X, Y, Z, NX, NY, NZ
		}

		public Vector2 Point1 { get; }
		public Vector2 Point2 { get; }
		public Vector2 Point3 { get; }

		public static Triangle2D NaN {
			get {
				return new Triangle2D (new Vector2 (float.NaN, float.NaN));
			}
		}

		private Triangle2D(Vector2 value) {
			Point1 = value;
			Point2 = value;
			Point3 = value;
		}

		private static float GetPoint(Vector3 point, VectorPoint newPoint) {
			float transformed = 0;
			switch (newPoint) {
			case (VectorPoint.X):
			case (VectorPoint.NX):
				transformed = point.x;
				break;
			case (VectorPoint.Y):
			case (VectorPoint.NY):
				transformed = point.y;
				break;
			case (VectorPoint.Z):
			case (VectorPoint.NZ):
				transformed = point.z;
				break;
			}
			if(newPoint == VectorPoint.NX || newPoint == VectorPoint.NY || newPoint == VectorPoint.NZ) {
				transformed *= -1;
			}
			return transformed;
		}

		private static Vector2 GetVector2 (Vector3 point, VectorPoint newX, VectorPoint newY) {
			return new Vector2 (GetPoint (point, newX), GetPoint (point, newY));
		}

		public Triangle2D(Vector3 point1, Vector3 point2, Vector3 point3, VectorPoint xFacing, VectorPoint yFacing) {
			Vector2 p1 = GetVector2 (point1, xFacing, yFacing), 
				p2 = GetVector2 (point2, xFacing, yFacing), 
				p3 = GetVector2 (point3, xFacing, yFacing);

			//Sorts points by x
			Vector2[] remaining;
			this.Point1 = Calcu.Min (out remaining, p1, p2, p3);
			this.Point2 = Calcu.Min (out remaining, remaining);
			this.Point3 = Calcu.Min (out remaining, remaining);
		}

		public bool HasPoint(Vector2 point) {
			return Calcu.Ish (point, Point1) || Calcu.Ish (point, Point2) || Calcu.Ish (point, Point3);
		}

		public bool IsNaN() {
			Vector2 nanVector = new Vector2 (float.NaN, float.NaN);
			return Calcu.Ish (nanVector, Point1) || Calcu.Ish (nanVector, Point2) || Calcu.Ish (nanVector, Point3);
		}

		public Vector2 PointInsideOf(List<Triangle2D> triangles) {
			foreach (Triangle2D triangle in triangles) {
				Vector2 insidePoint = this.PointInsideOf (triangle);
				if (!Calcu.Ish (insidePoint, Vector2.positiveInfinity)) {
					return insidePoint;
				}
			}
			return Vector2.positiveInfinity;
		}

		public Vector2 PointInsideOf(Triangle2D triangle) {
			if (!triangle.HasPoint (Point1) && triangle.Contains (Point1)) {
				return Point1;
			}
			if (!triangle.HasPoint (Point2) && triangle.Contains (Point2)) {
				return Point2;
			}
			if (!triangle.HasPoint (Point3) && triangle.Contains (Point3)) {
				return Point3;
			}
			return Vector2.positiveInfinity;
		}

		public bool Contains(Vector2 point) {
			bool b1 = Sign (point, Point1, Point2) < float.Epsilon;
			bool b2 = Sign (point, Point2, Point3) < float.Epsilon;
			bool b3 = Sign (point, Point3, Point1) < float.Epsilon;

			return ((b1 == b2) && (b2 == b3));
		}

		private float Sign(Vector2 p1, Vector2 p2, Vector2 p3) {
			return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
		}

		public static bool operator ==(Triangle2D e1, Triangle2D e2) {
			if (object.ReferenceEquals (e2, null)) {
				return object.ReferenceEquals (e1, null);
			}
			return Calcu.Ish (e1, e2);
		}

		public static bool operator !=(Triangle2D e1, Triangle2D e2) {
			return !(e1 == e2);
		}

		public override bool Equals(object edge) {
			return edge is Triangle2D && Equals ((Triangle2D) edge);
		}

		public bool Equals(Triangle2D other) {
			return this == other;
		}

		public bool IsValid() {
			return Point1 != Point2 && Point2 != Point3 && Point3 != Point1;
		}

		public override int GetHashCode() {
			unchecked { //Integer overflow isn't a worry since this just needs to be a deterministic value.
				int hash = 11;
				hash = hash * 59 + Mathf.Round(Point1.x * 100).GetHashCode ();
				hash = hash * 109 + Mathf.Round (Point1.y * 100).GetHashCode ();
				hash = hash * 557 + Mathf.Round (Point2.x * 100).GetHashCode ();
				hash = hash * 1009 + Mathf.Round (Point2.y * 100).GetHashCode ();
				hash = hash * 5437 + Mathf.Round (Point3.x * 100).GetHashCode ();
				hash = hash * 10061 + Mathf.Round (Point3.y * 100).GetHashCode ();

				return hash;
			}
		}

		public override string ToString() {
			return Point1 + " -> " + Point2 + " -> " + Point3;
		}
	}

	public class Edge2DComparer : IEqualityComparer<Edge2D> {
		public bool Equals(Edge2D x, Edge2D y) {
			return x == y;
		}

		public int GetHashCode(Edge2D obj) {
			return obj.GetHashCode();
		}
	}

	public class Triangle2DComparer : IEqualityComparer<Triangle2D> {
		public bool Equals(Triangle2D x, Triangle2D y) {
			return x == y;
		}

		public int GetHashCode(Triangle2D obj) {
			return obj.GetHashCode ();
		}
	}
}
