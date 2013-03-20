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

		TexturedQuad texQuad1, texQuad2;
		Quad quad1, quad2;
		Effect effect;
		Model beachBall;
		Texture2D abyssTexture, ballTexture;

		Matrix world;
		Matrix view;
		Matrix projection;
		Vector3 viewVector;

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
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			world = Matrix.CreateTranslation(0, 0, 0);
			view = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
			projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 16f / 9f, 0.01f, 100f);
			Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
			viewVector = -new Vector3(10, 10, 10);
			viewVector = Vector3.Normalize(viewVector);

			DrawHelper.Initialize(graphics.GraphicsDevice, spriteBatch, view, projection, viewVector);

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			ballTexture = Content.Load<Texture2D>(@"Models/BeachBallTexture");
			abyssTexture = Content.Load<Texture2D>(@"Textures/Abyss");
			effect = Content.Load<Effect>(@"Effects/TextureShader");			

			Effect otherEffect = Content.Load<Effect>(@"Effects/TextureShader");
			otherEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(Vector3.Zero));
			otherEffect.Parameters["View"].SetValue(view);
			otherEffect.Parameters["Projection"].SetValue(projection);
			otherEffect.Parameters["ViewVector"].SetValue(viewVector);
			otherEffect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Invert(Matrix.Transpose(Matrix.CreateTranslation(Vector3.Zero))));

			BasicEffect basicEffect = new BasicEffect(graphics.GraphicsDevice);
			basicEffect.EnableDefaultLighting();
			basicEffect.World = world;
			basicEffect.View = view;
			basicEffect.Projection = projection;
			basicEffect.VertexColorEnabled = false;
			basicEffect.LightingEnabled = true;
			basicEffect.Texture = abyssTexture;
			basicEffect.TextureEnabled = true;//*/

			Quad.SetQuad(graphics.GraphicsDevice, basicEffect);
			TexturedQuad.SetQuad(graphics.GraphicsDevice);
			
			beachBall = Content.Load<Model>(@"Models/BeachBall");

			#region NotNeeded
			quad1 = new Quad(Color.Yellow,
				new Vector3(5, 0, 5), new Vector3(-5, 0, 5),
				new Vector3(-5, 0, 0), new Vector3(5, 0, 0));
			/*quad2 = new Quad(Color.Silver,
				new Vector3(0, -5, 5), new Vector3(0, 5, 5),
				new Vector3(0, 5, 0), new Vector3(0, -5, 0));*/
			#endregion

			
			

			texQuad1 = new TexturedQuad(
				ballTexture, otherEffect,
				new Vector3(5, 0, 5), new Vector3(-5, 0, 5),
				new Vector3(-5, 0, 0), new Vector3(5, 0, 0));
			texQuad2 = new TexturedQuad(
				abyssTexture, basicEffect,
				new Vector3(0, -5, 5), new Vector3(0, 5, 5),
				new Vector3(0, 5, 0), new Vector3(0, -5, 0));
			
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

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

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			
			quad1.Draw();
			quad2.Draw();//*/

			texQuad1.Draw();
			texQuad2.Draw();
			DrawModelWithEffect(beachBall, world, view, projection);

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
		private void DrawModelWithEffect(Model model, Matrix world, Matrix view, Matrix projection)
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
	}
}
