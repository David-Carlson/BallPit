using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public static class CameraController
	{
		#region Declarations
		private static float angle = 0;
		public static float Angle
		{
			get
			{
				return angle;
			}
			set
			{
				angle = value % (MathHelper.TwoPi);
			}
		}

		private static float angularAcceleration;

		private static float maxVelocity = 0.4f;
		private static float angularVelocity = 0f;
		private static float AngularVelocity
		{
			get
			{
				return angularVelocity;
			}
			set
			{
				angularVelocity = MathHelper.Clamp(value, -maxVelocity, maxVelocity);
			}
		}

		#endregion

		#region Update Methods
		public static void Update(GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			GamePadState padState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

			// Get the X component, this will relatively rotate the camera around
			angularAcceleration = padState.ThumbSticks.Right.X;
			if (angularAcceleration != 0)
				Console.WriteLine(angularAcceleration);//*/

			Angle += angularAcceleration * elapsed;
			Console.WriteLine("Angle = " + Angle);

			UpdateMatricies();
		}
		private static void UpdateMatricies()
		{
			/*
			DrawHelper.CameraLoc = new Vector3(
				(float)Math.Cos(Angle) * 30,
				(float)Math.Sin(Angle) * 30,
				30);*/
		}

		#endregion
	}
}
