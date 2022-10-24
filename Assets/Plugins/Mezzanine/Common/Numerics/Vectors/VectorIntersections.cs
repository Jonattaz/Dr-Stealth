namespace Mz.Numerics
{
    public partial class Vectors
    {
        /// <summary>
		/// Returns if the point is 'left' of the line p1-p2
		/// </summary>        
		public static bool IsLeftOf(MzVector p1, MzVector p2, MzVector point) {
			var cross = (p2.X - p1.X) * (point.Y - p1.Y) - (p2.Y - p1.Y) * (point.X - p1.X);
			return cross < 0;
		}

        /// <summary>
		/// Find the point of intersection between the lines p1 --> p2 and p3 --> p4.
		/// </summary>
		public static void Intersection(
			MzVector p1,
			MzVector p2,
			MzVector p3,
			MzVector p4,
			out bool isLinesIntersect,
			out bool isSegmentIntersectedP1P2,
			out bool isSegmentIntersectedP3P4,
			out MzVector intersectionPoint,
			//out Vector2 closeP1,
			//out Vector2 closeP2,
			out float t1,
			out float t2
		) {
			/*
			 * See: http://csharphelper.com/blog/2014/08/determine-where-two-lines-intersect-in-c/
			 * 
			 * This method takes as parameters the points that define the segments and 
			 * the following output parameters to return results:
			 * 
			 * lines_intersect – True if the lines containing the segments intersect
			 * segments_intersect – True if the segments intersect
			 * intersection – The point where the lines intersect
			 * close_p1 – The point on the first segment that is closest to the point of intersection
			 * close_p2 – The point on the second segment that is closest to the point of intersection
			 * 		 
			 * First the code calculates dx12, dy12, and the other values that define the lines.
			 * It then plugs the values and the points’ coordinates into the equation shown earlier 
			 * to calculate t1.If the results is infinity, then the denominator is 0 so the lines are parallel.
			 * 
			 * Next the code uses the values of t1 and t2 to find the points of intersection between the two lines.
			 * 
			 * If t1 and t2 are both between 0 and 1, thenu the line segments intersect.
			 * 
			 * The code then adjusts t1 and t2 so they are between 0 and 1.Those values generate the points on the 
			 * two segments that are closest to the point of intersection. Finally the code uses the adjusted values 
			 * of t1 and t2 to find those closest points.
			 */

			var epsilon = 0.01f;

			// Get the segments' parameters.
			var dx12 = p2.X - p1.X;
			var dy12 = p2.Y - p1.Y;
			var dx34 = p4.X - p3.X;
			var dy34 = p4.Y - p3.Y;

			// Solve for t1 and t2
			var denominator = (dy12 * dx34 - dx12 * dy34);
			t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;
			if (float.IsInfinity(t1)) {
				// The lines are parallel (or close enough to it).
				isLinesIntersect = false;
				isSegmentIntersectedP1P2 = false;
				isSegmentIntersectedP3P4 = false;
				intersectionPoint = new MzVector(float.NaN, float.NaN);
				//closeP1 = new Vector2(float.NaN, float.NaN);
				//closeP2 = new Vector2(float.NaN, float.NaN);
				t2 = t1;
				return;
			}
			isLinesIntersect = true;

			t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

			// Find the point of intersection.
			intersectionPoint = new MzVector(p1.X + dx12 * t1, p1.Y + dy12 * t1);

			// The segments intersect if t1 and t2 are between 0 and 1.
			isSegmentIntersectedP1P2 = true;
			if (t1 < (0 - epsilon) || t1 > (1 + epsilon)) isSegmentIntersectedP1P2 = false;
			isSegmentIntersectedP3P4 = true;
			if (t2 < (0 - epsilon) || t2 > (1 + epsilon)) isSegmentIntersectedP3P4 = false;


			// Find the closest points on the segments.
			//if (t1 < 0) t1 = 0;
			//else if (t1 > 1) t1 = 1;

			//if (t2 < 0) t2 = 0;
			//else if (t2 > 1) t2 = 1;

			//closeP1 = new Vector2(p1.X + dx12 * t1, p1.Y + dy12 * t1);
			//closeP2 = new Vector2(p3.X + dx34 * t2, p3.Y + dy34 * t2);
		}

		/// <summary>
		/// If the result is greater than 0 and less than Numbers.Infinity,
		/// the ray intersected the box. Otherwise, the return value will be
		/// -1. If the ray did intersect the box at some point, the returned 
		/// float will match the T value of the side that was hit. For each 
		/// T value, a negative value indicates that the ray is pointed away 
		/// from that side of the box, while a positive value, that is less 
		/// the Numbers.Infinity, indicates that the corresponding side was 
		/// hit by the ray. Smaller positive T values indicate that the 
		/// intersection point was closer to the  origin of the ray than 
		/// larger values. A T value of NaN is returned when the ray is 
		/// exactly aligned with a side. If th T value of one side is 
		/// positive, while the opposite side is negative, the origin of 
		/// the ray is inside the box.
		/// </summary>
		/// <returns>The box intersect.</returns>
		/// <param name="rayOrigin">Ray origin.</param>
		/// <param name="rayDirection">Ray direction.</param>
		/// <param name="boxMin">Box minimum.</param>
		/// <param name="boxMax">Box max.</param>
		/// <param name="tLeft">T left.</param>
		/// <param name="tRight">T right.</param>
		/// <param name="tTop">T top.</param>
		/// <param name="tBottom">T bottom.</param>
		public static float IntersectionRayBox(
			MzVector rayOrigin, 
			MzVector rayDirection,
			MzVector boxMin, 
			MzVector boxMax,
			out float tLeft,
			out float tRight,
			out float tTop,
			out float tBottom
		) {
			tLeft = (boxMin.X - rayOrigin.X) / rayDirection.X;
			tRight = (boxMax.X - rayOrigin.X) / rayDirection.X;
			tTop = (boxMin.Y - rayOrigin.Y) / rayDirection.Y;
			tBottom = (boxMax.Y - rayOrigin.Y) / rayDirection.Y;

			var aMin = tLeft < tRight ? tLeft : tRight;
			var bMin = tTop < tBottom ? tTop : tBottom;

			var aMax = tLeft > tRight ? tLeft : tRight;
			var bMax = tTop > tBottom ? tTop : tBottom;

			var fMax = aMin > bMin ? aMin : bMin;
			var fMin = aMax < bMax ? aMax : bMax;

			var t7 = fMax;
			var t8 = fMin;

			var tResult = (fMin < 0 || fMax > fMin) ? -1 : fMax;

			return tResult;
		}
		
		// Infinite Line Intersection (line1 is p1-p2 and line2 is p3-p4)
        internal static bool IsInfiniteLineIntersection(
            MzVector p1,
            MzVector p2,
            MzVector p3,
            MzVector p4,
            ref MzVector result
        )
        {
            var bx = p2.X - p1.X;
            var by = p2.Y - p1.Y;
            var dx = p4.X - p3.X;
            var dy = p4.Y - p3.Y;
            var bDotDPerp = bx * dy - by * dx;
            if (Numbers.Abs(bDotDPerp) < Numbers.Epsilon) return false;

            var cx = p3.X - p1.X;
            var cy = p3.Y - p1.Y;
            var t = (cx * dy - cy * dx) / bDotDPerp;

            result = new MzVector(p1.X + t * bx, p1.Y + t * by);
            return true;
        }

        // Line Segment Intersection (line1 is p1-p2 and line2 is p3-p4)
        internal static bool IsLineSegmentIntersection(
            MzVector p1,
            MzVector p2,
            MzVector p3,
            MzVector p4,
            ref MzVector result
        )
        {
            var bx = p2.X - p1.X;
            var by = p2.Y - p1.Y;
            var dx = p4.X - p3.X;
            var dy = p4.Y - p3.Y;
            var bDotDPerp = bx * dy - by * dx;
            if (Numbers.Abs(bDotDPerp) < Numbers.Epsilon) return false;

            var cx = p3.X - p1.X;
            var cy = p3.Y - p1.Y;
            var t = (cx * dy - cy * dx) / bDotDPerp;
            if (t < 0 || t > 1) return false;

            var u = (cx * by - cy * bx) / bDotDPerp;
            if (u < 0 || u > 1) return false;

            result = new MzVector(p1.X + t * bx, p1.Y + t * by);
            return true;
        }
    }
}