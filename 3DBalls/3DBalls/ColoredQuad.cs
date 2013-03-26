using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DBalls;

namespace ShapeTest
{
	public class ColoredQuad : IQuadCollidable 
	{
		#region Declarations
		public VertexBuffer VertexBuffer
		{
			get;
			set;
		}
		public IndexBuffer IndexBuffer
		{
			get;
			private set;
		}
		public Effect effect;

		public BoundingBox BoundingShape
		{
			get;
			set;
		}
		public Vector3 Normal
		{
			get;
			set;
		}
		//private static GraphicsDevice g; // TODO: Remove this
		#endregion

		#region Constructors
		private ColoredQuad(
			Color color,
			Vector3 topLeft, Vector3 topRight, 
			Vector3 bottomRight, Vector3 bottomLeft)
		{
			VertexPositionColor[] vertices = new VertexPositionColor[4];

			vertices[0] = new VertexPositionColor(topLeft, color);
			vertices[1] = new VertexPositionColor(topRight, color);
			vertices[2] = new VertexPositionColor(bottomRight, color);
			vertices[3] = new VertexPositionColor(bottomLeft, color);

			VertexBuffer = new VertexBuffer(DrawHelper.g, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly); 
			VertexBuffer.SetData<VertexPositionColor>(vertices);

			short[] indices = new short[6];
			indices[0] = 0; // TL
			indices[1] = 1; // TR
			indices[2] = 2; // BR
			indices[3] = 0; // TL
			indices[4] = 2; // BR
			indices[5] = 3; // BL
			IndexBuffer = new IndexBuffer(DrawHelper.g, typeof(short), indices.Length, BufferUsage.WriteOnly);
			IndexBuffer.SetData(indices);
		}

		public ColoredQuad(
			Color color, Effect effect,
			Vector3 topLeft, Vector3 topRight,
			Vector3 bottomRight, Vector3 bottomLeft) : this(color,topLeft, topRight, bottomRight, bottomLeft)
		{
			this.effect = effect;
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

		#region Public Methods

		/// <summary>
		/// Draws the quad
		/// </summary>
		public void Draw()
		{
			/*
			//TODO: Move code to DrawHelper?
			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;

			DrawHelper.g.SetVertexBuffer(VertexBuffer);
			DrawHelper.g.Indices = IndexBuffer;
			DrawHelper.g.RasterizerState = rasterizerState;

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				
				pass.Apply();
				DrawHelper.g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);				
			}*/
			DrawHelper.Draw(this);
		}
		#endregion
	}
}
