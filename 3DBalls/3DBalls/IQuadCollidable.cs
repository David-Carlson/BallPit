using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace _3DBalls
{
	public interface IQuadCollidable
	{
		void Draw();
		BoundingBox BoundingShape
		{
			get;
			set;
		}
		Vector3 Normal
		{
			get;
			set;
		}
	}
}
