using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public static class Drawer
	{
		#region Static Members
		public static GraphicsDevice g;
		public static SpriteBatch spriteBatch;
		public static Matrix view, projection;
		public static Vector3 viewVector;
		#endregion

		#region Static Draw Methods

		

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
