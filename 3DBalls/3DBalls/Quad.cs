using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShapeTest
{
	public class Quad
	{
		public VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;
		public static Effect effect;
		private static GraphicsDevice g;


		public Quad(
			Color color,
			Vector3 topLeft, Vector3 topRight, 
			Vector3 bottomRight, Vector3 bottomLeft)
		{

			VertexPositionColor[] vertices = new VertexPositionColor[4];

			vertices[0] = new VertexPositionColor(topLeft, color);
			vertices[1] = new VertexPositionColor(topRight, color);
			vertices[2] = new VertexPositionColor(bottomRight, color);
			vertices[3] = new VertexPositionColor(bottomLeft, color);

			vertexBuffer = new VertexBuffer(g, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly); 
			vertexBuffer.SetData<VertexPositionColor>(vertices);

			short[] indices = new short[6];
			indices[0] = 0; // TL
			indices[1] = 1; // TR
			indices[2] = 2; // BR
			indices[3] = 0; // TL
			indices[4] = 2; // BR
			indices[5] = 3; // BL
			indexBuffer = new IndexBuffer(
				g, typeof(short), indices.Length, BufferUsage.WriteOnly);
			indexBuffer.SetData(indices);
		}

		public static void SetQuad(GraphicsDevice g, Effect effect)
		{
			Quad.g = g;
			Quad.effect = effect;
		}

		public void Draw()
		{
			g.SetVertexBuffer(vertexBuffer);
			g.Indices = indexBuffer;

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			g.RasterizerState = rasterizerState;

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();				
				g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);				
			}
		}
	}
}
