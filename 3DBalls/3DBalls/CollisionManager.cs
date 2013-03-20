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
		private BoundingBox playingArea;

		private Sphere spherePlaceHolder;

		public List<Sphere> spheres = new List<Sphere>();
		public List<TexturedQuad> walls = new List<TexturedQuad>();

		private Random rand = new Random();

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
				if (sphere.boundingShape.Intersects(temp))
					return;
			}

			Sphere newSphere = spherePlaceHolder.Clone();
			newSphere.boundingShape = temp;
			newSphere.Position = position;
			
		}
	}
}
