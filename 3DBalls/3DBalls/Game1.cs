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
		BasicEffect basicEffect;

		Matrix world = Matrix.CreateTranslation(0, 0, 0);
		Matrix view = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
		Matrix projection = Matrix.CreatePerspectiveFieldOfView(
			MathHelper.ToRadians(45), 16f / 9f, 0.01f, 100f);

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
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			basicEffect = new BasicEffect(graphics.GraphicsDevice);
			basicEffect.EnableDefaultLighting();
			basicEffect.World = world;
			basicEffect.View = view;
			basicEffect.Projection = projection;
			basicEffect.VertexColorEnabled = false; //Was true
			basicEffect.LightingEnabled = false;
			basicEffect.TextureEnabled = true; // was false

			Quad.SetQuad(graphics.GraphicsDevice, basicEffect);
			TexturedQuad.SetQuad(graphics.GraphicsDevice, basicEffect);
			Texture2D texture = Content.Load<Texture2D>("BeachBallTexture");

			/*quad1 = new Quad(Color.Yellow,
				new Vector3(5, 0, 5), new Vector3(-5, 0, 5),
				new Vector3(-5, 0, 0), new Vector3(5, 0, 0));
			quad2 = new Quad(Color.Silver,
				new Vector3(0, -5, 5), new Vector3(0, 5, 5),
				new Vector3(0, 5, 0), new Vector3(0, -5, 0));	*/		

			texQuad1 = new TexturedQuad(texture,
				new Vector3(5, 0, 5), new Vector3(-5, 0, 5),
				new Vector3(-5, 0, 0), new Vector3(5, 0, 0));
			texQuad2 = new TexturedQuad(texture,
				new Vector3(0, -5, 5), new Vector3(0, 5, 5),
				new Vector3(0, 5, 0), new Vector3(0, -5, 0));
			

			// TODO: use this.Content to load your game content here
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
			/*quad1.Draw();
			quad2.Draw();//*/

			texQuad1.Draw();
			texQuad2.Draw();

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
