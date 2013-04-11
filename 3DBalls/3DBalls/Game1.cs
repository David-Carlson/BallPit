using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ShapeTest;

namespace _3DBalls
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		#region Declarations

		TexturedQuad texQuad1, texQuad2;
		TexturedCollidableQuad texRect1;
		ColoredQuad quad1, quad2;
		Effect effect;
		Model beachBall;
		Texture2D abyssTexture, ballTexture;
		ObjectManager ObjManager;

		Matrix world;
		Matrix view;
		Matrix projection;
		Vector3 viewVector;
		#endregion

		#region Unused/Unwanted :(
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		private void DrawModelWithEffect(
			Model model, Matrix world, Matrix view, Matrix projection)
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
					effect.Parameters["ModelTexture"].SetValue(ballTexture);

					Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
					effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
				}
				mesh.Draw();
			}
		}

		#endregion

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			#region Matrix Setup and Initializations
			world = Matrix.CreateTranslation(0, 0, 0);
			Vector3 cameraLoc = new Vector3(30, 30, 10);
			Vector3 cameraTarget = new Vector3(-10, -10, 10);
			view = Matrix.CreateLookAt(cameraLoc, cameraTarget, new Vector3(0, 0, 1));
			projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 16f / 9f, 0.01f, 100f);
			Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
			viewVector = -new Vector3(10, 10, 10);
			viewVector = Vector3.Normalize(viewVector);

			DrawHelper.Initialize(graphics.GraphicsDevice, spriteBatch, 
				view, projection, viewVector, new Random());			
			#endregion

			#region Content Loading
			spriteBatch = new SpriteBatch(GraphicsDevice);
			ballTexture = Content.Load<Texture2D>(@"Models/BeachBallTexture");
			abyssTexture = Content.Load<Texture2D>(@"Textures/Abyss");
			effect = Content.Load<Effect>(@"Effects/TextureShader");
			beachBall = Content.Load<Model>(@"Models/BeachBall");
			#endregion

			#region Effect setup
			Effect specularEffect = Content.Load<Effect>(@"Effects/Specular");
			specularEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(Vector3.Zero));
			specularEffect.Parameters["View"].SetValue(view);
			specularEffect.Parameters["Projection"].SetValue(projection);
			specularEffect.Parameters["ViewVector"].SetValue(viewVector);
			specularEffect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Invert(Matrix.Transpose(Matrix.CreateTranslation(Vector3.Zero))));

			BasicEffect solidColorEffect = new BasicEffect(graphics.GraphicsDevice);
			solidColorEffect.EnableDefaultLighting();
			solidColorEffect.World = world;
			solidColorEffect.View = view;
			solidColorEffect.Projection = projection;
			solidColorEffect.VertexColorEnabled = true;
			solidColorEffect.LightingEnabled = false;
			solidColorEffect.Texture = null;
			solidColorEffect.TextureEnabled = false;

			BasicEffect textureEffect = new BasicEffect(graphics.GraphicsDevice);
			textureEffect.EnableDefaultLighting();
			textureEffect.World = world;
			textureEffect.View = view;
			textureEffect.Projection = projection;
			textureEffect.VertexColorEnabled = false;
			textureEffect.LightingEnabled = true;
			textureEffect.Texture = abyssTexture;
			textureEffect.TextureEnabled = true;
			#endregion

			#region Object setup

			Sphere modelSphere = new Sphere(beachBall, ballTexture, Vector3.Zero, 1.9f, effect);
			#region example wall setup
			/*quad1 = new ColoredQuad(Color.Gray, solidColorEffect,
				new Vector3(0, -20, 20), new Vector3(-20, -20, 20),
				new Vector3(-20, -20, 0), new Vector3(0, -20, 0));
			texRect1 = new TexturedCollidableQuad(abyssTexture, textureEffect,
				new Vector3(-20, -20, 20), new Vector3(-20, 0, 20),
				new Vector3(-20, 0, 0), new Vector3(-20, -20, 0));
			/*ColoredQuad quad2 = new ColoredQuad(Color.Gray*.5f, solidColorEffect,
				new Vector3(0, 0, 20), new Vector3(0, -20, 20),
				new Vector3(0, -20, 0), new Vector3(0, 0, 0));*/

			ColoredQuad upFacing = new ColoredQuad(
				DrawHelper.GetShade(Color.Gray, .7f, .8f),
				solidColorEffect,
				new Vector3(0, 0, 0), new Vector3(0, -20, 0),
				new Vector3(-20, -20, 0), new Vector3(-20, 0, 0));

			List<IQuadCollidable> walls = new List<IQuadCollidable>();
			walls.Add(upFacing);

			#endregion
			//getShadedColoredWallList(Color.Red, solidColorEffect, Vector3.Zero, new Vector3(-20, -20, 20))
			ObjManager = new ObjectManager(
				new BoundingBox(new Vector3(-20, -20, 0), new Vector3(0, 0, 20)),
				getShadedColoredWallList(Color.Gray, solidColorEffect, Vector3.Zero, new Vector3(-20, -20, 20)),
				modelSphere,
				10f);

			ObjManager.AddSphere(1.9f, new Vector3(-10, -10, 10), new Vector3(0, 0, -9));
			ObjManager.AddSphere(1.9f, new Vector3(-10, -10, 15), new Vector3(0, -20, 0));
			ObjManager.AddNextSpheres();

			#endregion

			#region NotNeeded
			/*quad1 = new ColoredQuad(Color.Green, basicEffect,
				new Vector3(5, 0, 1.9f), new Vector3(-5, 0, 1.9f),
				new Vector3(-5, 0, 0), new Vector3(5, 0, 0));
			quad2 = new Quad(Color.Silver,
				new Vector3(0, -5, 5), new Vector3(0, 5, 5),
				new Vector3(0, 5, 0), new Vector3(0, -5, 0));
			 texQuad2 = new TexturedQuad(
				abyssTexture, basicEffect,
				new Vector3(0, -5, 5), new Vector3(0, 5, 5),
				new Vector3(0, 5, 0), new Vector3(0, -5, 0));//*/
			#endregion
		}

		#region Wall Maker

		/// <summary>
		/// Creates a box of randomly shaded inward facing walls
		/// </summary>
		/// <param name="color"></param>
		/// <param name="effect"></param>
		/// <param name="pos"></param>
		/// <param name="dim"></param>
		/// <returns></returns>
		private List<IQuadCollidable> getShadedColoredWallList(
			Color color, Effect effect, 
			Vector3 position, Vector3 oppositeCornerVec)
		{
			Vector3 pos = position; 
			Vector3 dim = oppositeCornerVec;

			float lowerShade = 0.25f;
			float upperShade = 0.75f;
			List<IQuadCollidable> walls = new List<IQuadCollidable>();			

			//Faces -X direction
			walls.Add(new ColoredQuad(
				DrawHelper.GetShade(color, lowerShade, upperShade), 
				effect, 
				new Vector3(0, 0, dim.Z) + pos, new Vector3(0, dim.Y, dim.Z) + pos, 
				new Vector3(0, dim.Y, 0) + pos, new Vector3(0, 0, 0) + pos));

			// Faces +Y direction
			walls.Add(new ColoredQuad(
				DrawHelper.GetShade(color, lowerShade, upperShade), 
				effect,
				new Vector3(0, dim.Y, dim.Z) + pos, new Vector3(dim.X, dim.Y, dim.Z) + pos, 
				new Vector3(dim.X, dim.Y, 0) + pos, new Vector3(0, dim.Y, 0) + pos));

			// Faces X Direction
			walls.Add(new ColoredQuad(
				DrawHelper.GetShade(color, lowerShade, upperShade), 
				effect,
				new Vector3(dim.X, dim.Y, dim.Z) + pos, new Vector3(dim.X, 0, dim.Z) + pos,
				new Vector3(dim.X, 0, 0) + pos, new Vector3(dim.X, dim.Y, 0) + pos));

			// Faces -Y Direction
			walls.Add(new ColoredQuad(
				DrawHelper.GetShade(color, lowerShade, upperShade), 
				effect,
				new Vector3(dim.X, 0, dim.Z) + pos, new Vector3(0, 0, dim.Z) + pos, 
				new Vector3(0, 0, 0) + pos, new Vector3(dim.X, 0, 0) + pos));

			// Faces +Z Direction (Floor)
			walls.Add(new ColoredQuad(
				DrawHelper.GetShade(color, lowerShade, upperShade), 
				effect, 
				new Vector3(0, 0, 0) + pos, new Vector3(0, dim.Y, 0) + pos, 
				new Vector3(dim.X, dim.Y, 0) + pos, new Vector3(dim.X, 0, 0) + pos));

			// Faces -Z Direction (Ceiling)
			walls.Add(new ColoredQuad(
				DrawHelper.GetShade(color, lowerShade, upperShade), 
				effect,
				new Vector3(0, dim.Y, dim.Z) + pos, new Vector3(0, 0, dim.Z) + pos, 
				new Vector3(dim.X, 0, dim.Z) + pos, new Vector3(dim.X, dim.Y, dim.Z) + pos));

			return walls;

		}

		/// <summary>
		/// Returns a list of textured walls with one corner at position and the other at the point 'dimensions'
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="effect"></param>
		/// <param name="dimensions"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		private List<IQuadCollidable> getTexturedWallList(
			Texture2D texture, Effect effect, 
			Vector3 pos, Vector3 dim)
		{
			List<IQuadCollidable> walls = new List<IQuadCollidable>();

			//Faces -X direction
			walls.Add(new TexturedCollidableQuad(texture, effect,
				new Vector3(0, 0, dim.Z) + pos, new Vector3(0, dim.Y, dim.Z) + pos,
				new Vector3(0, dim.Y, 0) + pos, new Vector3(0, 0, 0) + pos));

			// Faces +Y direction
			walls.Add(new TexturedCollidableQuad(texture, effect,
				new Vector3(0, dim.Y, dim.Z) + pos, new Vector3(dim.X, dim.Y, dim.Z) + pos,
				new Vector3(dim.X, dim.Y, 0) + pos, new Vector3(0, dim.Y, 0) + pos));

			// Faces X Direction
			walls.Add(new TexturedCollidableQuad(texture, effect,
				new Vector3(dim.X, dim.Y, dim.Z) + pos, new Vector3(dim.X, 0, dim.Z) + pos,
				new Vector3(dim.X, 0, 0) + pos, new Vector3(dim.X, dim.Y, 0) + pos));

			// Faces -Y Direction
			walls.Add(new TexturedCollidableQuad(texture, effect,
				new Vector3(dim.X, 0, dim.Z) + pos, new Vector3(0, 0, dim.Z) + pos,
				new Vector3(0, 0, 0) + pos, new Vector3(dim.X, 0, 0) + pos));

			// Faces +Z Direction (Floor)
			walls.Add(new TexturedCollidableQuad(texture, effect,
				new Vector3(0, 0, 0) + pos, new Vector3(0, dim.Y, 0) + pos,
				new Vector3(dim.X, dim.Y, 0) + pos, new Vector3(dim.X, 0, 0) + pos));

			// Faces -Z Direction (Ceiling)
			walls.Add(new TexturedCollidableQuad(texture, effect,
				new Vector3(0, dim.Y, dim.Z) + pos, new Vector3(0, 0, dim.Z) + pos,
				new Vector3(dim.X, 0, dim.Z) + pos, new Vector3(dim.X, dim.Y, dim.Z) + pos));

			return walls;

		}

		#endregion
				
		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			ObjManager.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
		
			if (ObjManager != null)
				ObjManager.Draw();
		
			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}

		

	}
}
