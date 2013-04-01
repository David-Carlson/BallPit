using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapeTest;
namespace _3DBalls
{
	public static class DrawHelper
	{
		#region Static Members
		public static GraphicsDevice g;
		public static SpriteBatch spriteBatch;
		public static Matrix View, Projection;
		public static Vector3 ViewVector;
		public static Random rand;
		private static Vector3 cameraLoc, cameraTarget, upVector;
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
			Matrix view, Matrix projection, Vector3 viewVector, Random rand)
		{
			DrawHelper.g = g;
			DrawHelper.spriteBatch = spriteBatch;
			DrawHelper.View = view;
			DrawHelper.Projection = projection;
			DrawHelper.ViewVector = viewVector;
			DrawHelper.rand = rand;
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
				effect.Parameters["World"].SetValue(Matrix.CreateTranslation(quad.Position));
				effect.Parameters["View"].SetValue(View);
				effect.Parameters["Projection"].SetValue(Projection);
				effect.Parameters["ViewVector"].SetValue(ViewVector);
				effect.Parameters["ModelTexture"].SetValue(quad.Texture);

				Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateTranslation(quad.Position)));
				effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
				pass.Apply();
				g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
			}
		}

		public static void Draw(ColoredQuad quad)
		{
			/*
			Effect effect = quad.Effect;
			g.SetVertexBuffer(quad.VertexBuffer);
			g.Indices = quad.IndexBuffer;

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			g.RasterizerState = rasterizerState;

			foreach (EffectPass pass in quad.Effect.CurrentTechnique.Passes)
			{
				effect.Parameters["World"].SetValue(Matrix.CreateTranslation(quad.Position));
				effect.Parameters["View"].SetValue(View);
				effect.Parameters["Projection"].SetValue(Projection);
				effect.Parameters["ViewVector"].SetValue(ViewVector);
				effect.Parameters["ModelTexture"].SetValue(quad.Texture);

				Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateTranslation(quad.Position)));
				effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
				pass.Apply();
				g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
			}//*/
			throw new NotImplementedException();
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
					effect.Parameters["View"].SetValue(View);
					effect.Parameters["Projection"].SetValue(Projection);
					effect.Parameters["ViewVector"].SetValue(ViewVector);
					effect.Parameters["ModelTexture"].SetValue(texture);

					Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
					effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
				}
				mesh.Draw();
			}
		}

		#endregion

		#region Other Methods

		/// <summary>
		/// Returns a shade of color, using a lower/upper bounds which are between 0 and 1
		/// </summary>
		/// <param name="color"></param>
		/// <param name="lowerBound"></param>
		/// <param name="upperBound"></param>
		/// <returns></returns>
		public static Color GetShade(Color color, float lowerBound, float upperBound)
		{
			float value = (float)rand.NextDouble() * (upperBound - lowerBound) + lowerBound;
			return color * value;
		}

		#endregion
	}
}
