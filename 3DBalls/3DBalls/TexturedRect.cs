using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public class TexturedRect : TexturedQuad
	{
		private BoundingBox boundingShape;
		private Vector3 normal;

		private TexturedRect(
			Texture2D texture, Effect effect,
			Vector3 topLeft, Vector3 topRight,
			Vector3 bottomRight, Vector3 bottomLeft) : base(texture, effect, topLeft, topRight, bottomRight, bottomLeft)
		{
			normal = Vector3.Cross(bottomLeft - topLeft, topRight - topLeft);
			normal.Normalize();

			//I extruded the corners out in the opposite direction of the normal so I have all 8 corners saved in memory
			Vector3 tL, tR, bR, bL;
			tL = topLeft - normal;tR = topRight - normal; bR = bottomRight - normal; bL = bottomLeft - normal;

			// Only bottom corners could logically be min points, so I only check those. 
			// Wait, fuck. If the normal faces upward this isn't true. Better check em all, you inelegant fuck
			Vector3 min = Vector3.Min(
				Vector3.Min(
					Vector3.Min(bottomLeft, bottomRight), Vector3.Min(bL, bR)), 
				Vector3.Min(
					Vector3.Min(topLeft, topRight), Vector3.Min(tL, tR)));
			Vector3 max = Vector3.Max(
				Vector3.Max(
					Vector3.Max(bottomLeft, bottomRight), Vector3.Max(bL, bR)),
				Vector3.Max(
					Vector3.Max(topLeft, topRight), Vector3.Max(tL, tR)));
			boundingShape = new BoundingBox(min, max);
		}
	}
}
