using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public class TexturedRect : TexturedQuad, IQuad 
	{
		#region Declaration Amendment
		public BoundingBox BoundingShape;
		public Vector3 Normal;
		private Texture2D texture;
		private Vector3 vector31;
		private Vector3 vector32;
		private Vector3 vector33;
		private Vector3 vector34;
		#endregion

		#region Facade Constructor
		public TexturedRect(
			Texture2D texture, Effect effect,
			Vector3 topLeft, Vector3 topRight,
			Vector3 bottomRight, Vector3 bottomLeft) : base(texture, effect, topLeft, topRight, bottomRight, bottomLeft)
		{
			Normal = Vector3.Cross(bottomLeft - topLeft, topRight - topLeft);
			Normal.Normalize();

			//I extruded the corners out in the opposite direction of the normal so I have all 8 corners saved in memory
			Vector3 tL, tR, bR, bL;
			tL = topLeft - Normal;tR = topRight - Normal; bR = bottomRight - Normal; bL = bottomLeft - Normal;

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
			BoundingShape = new BoundingBox(min, max);
		}

		
		#endregion
	}
}
