using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public class TexturedQuad : IQuad 
	{
		#region Declarations
		public VertexBuffer VertexBuffer;
		public IndexBuffer IndexBuffer;
		public Texture2D Texture;

		public Vector3 Position;

		public Effect effect;
		private static GraphicsDevice g;

		#endregion

		#region Constructor
		public TexturedQuad(
			Texture2D texture, Effect effect, 
			Vector3 topLeft, Vector3 topRight, 
			Vector3 bottomRight, Vector3 bottomLeft)
		{
			this.Texture = texture;
			this.effect = effect;

			VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[4];
			
			Vector3 normal = Vector3.Cross((bottomLeft - topLeft), (topRight - topLeft));

			vertices[0] = new VertexPositionNormalTexture(topLeft, normal, new Vector2(0, 0));
			vertices[1] = new VertexPositionNormalTexture(topRight, normal, new Vector2(1, 0));
			vertices[2] = new VertexPositionNormalTexture(bottomRight, normal, new Vector2(1, 1));
			vertices[3] = new VertexPositionNormalTexture(bottomLeft, normal, new Vector2(0, 1));

			VertexBuffer = new VertexBuffer(g, typeof(VertexPositionNormalTexture), 4, BufferUsage.WriteOnly);
			VertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

			short[] indices = new short[6];
			indices[0] = 0; // TL
			indices[1] = 1; // TR
			indices[2] = 2; // BR
			indices[3] = 0; // TL
			indices[4] = 2; // BR
			indices[5] = 3; // BL
			IndexBuffer = new IndexBuffer(
				g, typeof(short), indices.Length, BufferUsage.WriteOnly);
			IndexBuffer.SetData(indices);
		}

		#endregion

		#region Public Methods
		public static void SetQuad(GraphicsDevice g)
		{
			TexturedQuad.g = g;	
		}


		public void Draw()
		{
			g.SetVertexBuffer(VertexBuffer);
			g.Indices = IndexBuffer;

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			g.RasterizerState = rasterizerState;
			

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				
				pass.Apply();							
				g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);				
			}
		}
		#endregion
	}
}
