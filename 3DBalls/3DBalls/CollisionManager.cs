using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	class CollisionManager
	{
		#region Declarations
		private BoundingBox playingArea;
		private Sphere spherePlaceHolder;
		private float sphere_initialSpeed = 10f;
		public List<Sphere> spheres = new List<Sphere>();
		public List<TexturedQuad> walls = new List<TexturedQuad>();

		private Random rand = new Random();

		#endregion

		public CollisionManager()
		{
			//TODO: Fill in collisionManager

		}

		/// <summary>
		/// Adds a modelSphere at a given position
		/// If that isn't possible, it returns
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		public void AddSphere(float radius, Vector3 position, Vector3 velocity)
		{
			BoundingSphere temp = new BoundingSphere(position, radius);
			
			foreach (Sphere sphere in spheres)
			{
				if (sphere.BoundingShape.Intersects(temp))
					return;
			}

			Sphere newSphere = spherePlaceHolder.Clone();
			newSphere.BoundingShape = temp;
			newSphere.Position = position;
			float xSpeed = rand.Next(-1, 1);
			float ySpeed = rand.Next(-1, 1);
			float zSpeed = rand.Next(-1, 1);
			Vector3 newVelocity = new Vector3(xSpeed, ySpeed, zSpeed);
			newVelocity = Vector3.Normalize(newVelocity) * sphere_initialSpeed;
			newSphere.Velocity = newVelocity;			
		}

		public void AddRandomSpheres(int count)
		{
			int attempts = 0;
			while (count > 0)
			{
				
				if (attempts >= 50) // Ensures it doesn't try forever to find a fit that doesn't exist
					return;

				//Picks a random location that is in bounds
				float xPos = (float)rand.NextDouble() * (playingArea.Max.X - playingArea.Min.X) + playingArea.Min.X;
				float yPos = (float)rand.NextDouble() * (playingArea.Max.Y - playingArea.Min.Y) + playingArea.Min.Y;
				float zPos = (float)rand.NextDouble() * (playingArea.Max.Z - playingArea.Min.Z) + playingArea.Min.Z;
				Vector3 newPosition = new Vector3(xPos, yPos, zPos);
				BoundingSphere tempBounds = new BoundingSphere(newPosition, (playingArea.Max.X - playingArea.Min.X) / 2f);

				//Tests if it collides with existing balls
				foreach (Sphere sphere in spheres)
				{
					if (sphere.BoundingShape.Intersects(tempBounds))
					{
						attempts++;
						continue;
					}
				}
				//It works! Now fill it up with a random velocity and such
				Sphere newSphere = spherePlaceHolder.Clone();
				newSphere.BoundingShape = tempBounds;

				float xSpeed = rand.Next(-1, 1);
				float ySpeed = rand.Next(-1, 1);
				float zSpeed = rand.Next(-1, 1);
				Vector3 newVelocity = new Vector3(xSpeed, ySpeed, zSpeed);
				newVelocity = Vector3.Normalize(newVelocity) * sphere_initialSpeed;
				newSphere.Velocity = newVelocity;

				attempts = 0;
				count--;
			}
		}
	}
}
