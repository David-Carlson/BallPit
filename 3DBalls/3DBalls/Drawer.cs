using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public static class DrawHelper
	{
		#region Static Members
		public static GraphicsDevice g;
		public static SpriteBatch spriteBatch;
		public static Matrix view, projection;
		public static Vector3 viewVector;
		#endregion

		#region Initializer
		/// <summary>
		/// Start of game initialization to get draw commands to work
		/// </summary>
		/// <param name="g"></param>
		/// <param name="spriteBatch"></param>
		/// <param name="view"></param>
		/// <param name="projection"></param>
		/// <param name="viewVector"></param>
		public static void Initialize(
			GraphicsDevice g, SpriteBatch spriteBatch,
			Matrix view, Matrix projection, Vector3 viewVector)
		{
			DrawHelper.g = g;
			DrawHelper.spriteBatch = spriteBatch;
			DrawHelper.view = view;
			DrawHelper.projection = projection;
			DrawHelper.viewVector = viewVector;
		}
		#endregion

		#region Static Draw Methods

		public static void Draw(TexturedQuad quad)
		{
			Effect effect = quad.effect;
			g.SetVertexBuffer(quad.VertexBuffer);
			g.Indices = quad.IndexBuffer;

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			g.RasterizerState = rasterizerState;

			foreach (EffectPass pass in quad.effect.CurrentTechnique.Passes)
			{
				effect.Parameters["World"].SetValue(Matrix.Identity);
				effect.Parameters["View"].SetValue(view);
				effect.Parameters["Projection"].SetValue(projection);
				effect.Parameters["ViewVector"].SetValue(viewVector);
				effect.Parameters["ModelTexture"].SetValue(quad.Texture);

				Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.Identity));
				effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
				pass.Apply();
				g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
			}

		}


		/// <summary>
		/// Draws the given model w/given matrix at given position (world matrix)
		/// Assumes the view, proj, and viewVector are set
		/// </summary>
		/// <param name="model">The given model to be drawn</param>
		/// <param name="texture">The texture to texture the model with</param>
		/// <param name="world">The position via world Matrix</param>
		/// <param name="effect">The effect shader to shade with</param>
		public static void DrawModelWithEffect(
			Model model, Texture2D texture,
			Matrix world,Effect effect)
		{
			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					part.Effect = effect;
					effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
					effect.Parameters["View"].SetValue(view);
					effect.Parameters["Projection"].SetValue(projection);
					effect.Parameters["ViewVector"].SetValue(viewVector);
					effect.Parameters["ModelTexture"].SetValue(texture);

					Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
					effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
				}
				mesh.Draw();
			}
		}

		#endregion
	}
}
