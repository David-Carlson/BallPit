﻿using System;
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
		public static BasicEffect basicEffect;
		private static GraphicsDevice g;


		public Quad(
			Vector3 topLeft, Vector3 topRight, 
			Vector3 bottomRight, Vector3 bottomLeft)
		{

			VertexPositionColor[] vertices = new VertexPositionColor[4];

			vertices[0] = new VertexPositionColor(topLeft, Color.Green);
			vertices[1] = new VertexPositionColor(topRight, Color.Black);
			vertices[2] = new VertexPositionColor(bottomRight, Color.Orange);
			vertices[3] = new VertexPositionColor(bottomLeft, Color.Yellow);

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

		public static void SetQuad(GraphicsDevice g, BasicEffect basicEffect)
		{
			Quad.g = g;
			Quad.basicEffect = basicEffect;
		}

		public void Draw()
		{
			g.SetVertexBuffer(vertexBuffer);
			g.Indices = indexBuffer;

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			g.RasterizerState = rasterizerState;

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				//GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
				g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
				//GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12, 0, 20);
			}
		}
	}
}
