using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Tetris;

class TetrisGame : Game
{
    private SpriteBatch spriteBatch;
    private InputHelper inputHelper;
    private GameWorld gameWorld;
    private Score score;
    private GameState gameState;

    public static ContentManager ContentManager { get; private set; }
    
    public static Point ScreenSize { get; private set; }

    [STAThread]
    static void Main(string[] args)
    {
        TetrisGame game = new TetrisGame();
        game.Run();
    }

    public TetrisGame()
    {        
        GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);

        ContentManager = Content;
        
        Content.RootDirectory = "Content";

        ScreenSize = new Point(800, 800);
        graphics.PreferredBackBufferWidth = ScreenSize.X;
        graphics.PreferredBackBufferHeight = ScreenSize.Y;

        inputHelper = new InputHelper();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        gameWorld = new GameWorld();

    }

    protected override void Update(GameTime gameTime)
    {
        inputHelper.Update(gameTime);
        gameWorld.HandleInput(gameTime, inputHelper);
        gameWorld.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        gameWorld.Draw(gameTime, spriteBatch);
    }
}

