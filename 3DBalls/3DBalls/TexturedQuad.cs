using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public class TexturedQuad 
	{
		#region Declarations
		public VertexBuffer VertexBuffer;
		public IndexBuffer IndexBuffer;
		public Texture2D Texture;

		public Vector3 Position;
		public Effect effect;

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

			VertexBuffer = new VertexBuffer(DrawHelper.g, typeof(VertexPositionNormalTexture), 4, BufferUsage.WriteOnly);
			VertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

			short[] indices = new short[6];
			indices[0] = 0; // TL
			indices[1] = 1; // TR
			indices[2] = 2; // BR
			indices[3] = 0; // TL
			indices[4] = 2; // BR
			indices[5] = 3; // BL
			IndexBuffer = new IndexBuffer(
				DrawHelper.g, typeof(short), indices.Length, BufferUsage.WriteOnly);
			IndexBuffer.SetData(indices);
		}

		#endregion

		#region Public Methods

		public void Draw()
		{

			DrawHelper.g.SetVertexBuffer(VertexBuffer);
			DrawHelper.g.Indices = IndexBuffer;

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			DrawHelper.g.RasterizerState = rasterizerState;			

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				
				pass.Apply();
				DrawHelper.g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);				
			}
		}
		#endregion
	}
}
