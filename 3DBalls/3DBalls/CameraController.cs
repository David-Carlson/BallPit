using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
				angle = value % ((float)Math.PI * 2);
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
				AngularVelocity = MathHelper.Clamp(value, -maxVelocity, maxVelocity);
			}
		}

		#endregion

		#region Update Methods
		public static void Update(GameTime gameTime)
		{			
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			GamePadState padState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			angularAcceleration = padState.ThumbSticks.Right.X * 0.5f;

			if (angularAcceleration == 0f)
				AngularVelocity += -AngularVelocity / Math.Abs(AngularVelocity) * 0.5f;
			else
				AngularVelocity += angularAcceleration;		

			angle += AngularVelocity * elapsed;
		}
		private static void UpdateMatricies()
		{
			DrawHelper.CameraLoc = new Vector3(
				(float)Math.Cos(angle) * 30,
				(float)Math.Sin(angle) * 30,
				30);
		}

		#endregion
	}
}
