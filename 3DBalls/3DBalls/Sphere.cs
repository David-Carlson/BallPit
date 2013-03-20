using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	class Sphere
	{
		private Model model;
		private Texture2D texture;
		private static Effect effect;

		public Vector3 Position;
		private Vector3 velocity;
		private static Vector3 Acceleration;
		private float radius;

		public void Update(GameTime gameTime)
		{
			//TODO: Fill in update
			//TODO Make collision checker
		}


		public void Draw()
		{
			DrawHelper.DrawModelWithEffect(model, texture, Matrix.CreateTranslation(Position), effect);
		}

	}
}
