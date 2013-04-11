﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public class Sphere : ICloneable
	{
		#region Declarations
		private Model model;
		private Texture2D texture;
		private Effect effect;
		private Vector3 position;
		public Vector3 Position
		{
			get{ return this.position; }
			set{ this.position = value; BoundingShape.Center = value; }
		}
		public Vector3 Velocity = new Vector3(0, 0, 0);
		public static Vector3 Acceleration = new Vector3(0, 0, -40);
		public BoundingSphere BoundingShape;
		public float mass = 1f;
		public float high = 0;
		#endregion

		#region Constructors
		public Sphere(
			Model model, Texture2D texture, 
			Vector3 position, float radius, Effect effect)
		{
			this.model = model;
			this.texture = texture;

			this.Position = position;
			this.BoundingShape = new BoundingSphere(position, radius);
			this.effect = effect;
		}

		#endregion

		#region Update & Draw Methods
		public void Update(GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			// d = v*t + 1/2*a*t^2
			Position += Velocity * elapsed + Acceleration * elapsed * elapsed / 2;
			Velocity = Velocity + Acceleration * elapsed;
			
			/*
			Position += Velocity * elapsed;
			Velocity += Acceleration * elapsed;
			 * Euler integration, shitty for parabolas and everything else too
			*/		
		}

		public void Draw()
		{
			DrawHelper.DrawModelWithEffect(
				model, 
				texture, 
				Matrix.CreateTranslation(Position), 
				effect);
		}

		#endregion

		#region Cloning Methods
		/// <summary>
		/// Used to clone otherSphere, in case Clone doesn't work
		/// </summary>
		/// <param name="otherSphere"></param>
		public Sphere(Sphere otherSphere)
		{
			this.model = otherSphere.model;
			this.texture = otherSphere.texture;
			this.effect = otherSphere.effect.Clone();
			this.Position = otherSphere.Position;
			this.Velocity = otherSphere.Velocity;
			this.BoundingShape = otherSphere.BoundingShape;
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}
		public Sphere Clone()
		{
			return (Sphere)this.MemberwiseClone();
		}
		#endregion
	}
}
