using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DBalls
{
	public class ObjectManager
	{
		#region Declarations
		private BoundingBox playingArea;
		public List<Sphere> spheres = new List<Sphere>();
		public List<TexturedRect> walls = new List<TexturedRect>();

		private Sphere spherePlaceHolder;
		private float sphere_initialSpeed = 10f;
		/// <summary>
		/// A list of spheres not be interacted with but drawn
		/// Will later be added
		/// </summary>
		private List<Sphere> nextSpheresToAdd = new List<Sphere>();

		private Random rand = new Random();
		#endregion

		#region Constructor

		public ObjectManager(BoundingBox playingArea, List<TexturedRect> walls, Sphere sphere, float sphereSpeed)
		{
			//TODO: Fill in collisionManager
			this.playingArea = playingArea;
			this.walls = walls;
			this.spherePlaceHolder = sphere.Clone();
		}

		#endregion

		#region Sphere Management

		/// <summary>
		/// Adds the next spheres to the spheres in play list
		/// </summary>
		public void AddNextSphere()
		{
			if (nextSpheresToAdd.Count > 0)
			{
				foreach (Sphere sphere in nextSpheresToAdd)
					spheres.Add(sphere);
				nextSpheresToAdd = new List<Sphere>();
			}
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
			foreach (Sphere sphere in nextSpheresToAdd)
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

			nextSpheresToAdd.Add(newSphere);
		}

		/// <summary>
		/// Adds count worth of spheres to the playingArea, each with a random position and velocity.
		/// If no position is found within 50 attempts, it returns
		/// </summary>
		/// <param name="count">Number of spheres to add</param>
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
						continue; //TODO: CHECK IF THIS IS POSSIBLE
					}
				}
				foreach (Sphere sphere in nextSpheresToAdd)
				{
					if (sphere.BoundingShape.Intersects(tempBounds))
						return;
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

				spheres.Add(newSphere);

				attempts = 0;
				count--;
			}
		}

		#endregion
		//TODO: Add ball-ball collision
		#region Collision Handling

		/// <summary>
		/// Checks for active balls hitting walls
		/// Doesn't reflect if the ball isn't travelling towards the wall (aka I fucked up in the previous frame)
		/// </summary>
		private void CheckSphereToWallCollisions()
		{
			foreach (Sphere sphere in spheres)
			{
				foreach (TexturedRect wall in walls)
				{
					if (sphere.BoundingShape.Intersects(wall.BoundingShape))
					{
						// V' = V -2N(N * V)
						// Basic reflection of vector over normal
						float reflection = Vector3.Dot(wall.Normal, sphere.Velocity);
						if (reflection >= 0)
							continue;
						sphere.Velocity = sphere.Velocity - 2 * wall.Normal * (reflection);
					}
				}
			}
		}

		#endregion

		#region Update and Draw

		public void Update(GameTime gameTime)
		{
			foreach (Sphere sphere in spheres)
				sphere.Update(gameTime);
		}

		/// <summary>
		/// Draws all active spheres, potential spheres
		/// and texturedRectangles
		/// </summary>
		public void Draw()
		{
			//TODO: Code for background/skybox

			foreach (Sphere sphere in spheres)
				sphere.Draw();
			foreach (Sphere sphere in nextSpheresToAdd)
				sphere.Draw();
			foreach (TexturedRect wall in walls)
				wall.Draw();
		}

		#endregion
	}
}
