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
			this.texture = texture;
			VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[4];
			
			Vector3 normal = Vector3.Cross((bottomLeft - topLeft), (topRight - topLeft));

			vertices[0] = new VertexPositionNormalTexture(topLeft, normal, new Vector2(0, 0));
			vertices[1] = new VertexPositionNormalTexture(topRight, normal, new Vector2(1, 0));
			vertices[2] = new VertexPositionNormalTexture(bottomRight, normal, new Vector2(1, 1));
			vertices[3] = new VertexPositionNormalTexture(bottomLeft, normal, new Vector2(0, 1));

			vertexBuffer = new VertexBuffer(g, typeof(VertexPositionNormalTexture), 4, BufferUsage.WriteOnly);
			vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

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
