using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	class TexturedQuad
	{
		public VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;
		private Texture2D texture;
		public static Effect effect;
		private static GraphicsDevice g;


		public TexturedQuad(
			Texture2D texture,
			Vector3 topLeft, Vector3 topRight, 
			Vector3 bottomRight, Vector3 bottomLeft)
		{
			VertexPositionTexture[] vertices = new VertexPositionTexture[4];
			//VertexPositionColor[] vertices = new VertexPositionColor[4];

			vertices[0] = new VertexPositionTexture(topLeft, new Vector2(0, 0));
			vertices[1] = new VertexPositionTexture(topRight, new Vector2(1, 0));
			vertices[2] = new VertexPositionTexture(bottomRight, new Vector2(1, 1));
			vertices[3] = new VertexPositionTexture(bottomLeft, new Vector2(0, 1));

			vertexBuffer = new VertexBuffer(g, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly); 
			vertexBuffer.SetData<VertexPositionTexture>(vertices);

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
			TexturedQuad.g = g;
			TexturedQuad.effect = effect;
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
