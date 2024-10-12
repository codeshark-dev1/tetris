using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TetrisTemplate;

class GameWorld
{
    enum GameState
    {
        Playing,
        GameOver
    }
    public static Random Random { get { return random; } }
    static Random random;

    SpriteFont font;
    GameState gameState;
    public TetrisGrid grid;

    private BlockSpawner blockSpawner;
    private TetrisBlock currentBlock;
    private int currentXOffset = 0, currentYOffset = 0;
    private float moveDelay = 1, currentMoveDelay;

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
        blockSpawner = new BlockSpawner();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        blockSpawner.Update(gameTime);
        currentBlock = blockSpawner.GetNewBlock();

        if (currentBlock != null)
        {
            currentBlock.Update(gameTime);

            currentMoveDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            currentMoveDelay = moveDelay;
            currentXOffset = 0;
        }

        if (currentYOffset + 1 > 18/* || BlockCollides()*/)
        {
            LockBlock();
            blockSpawner.SpawnBlock();
            currentYOffset = 0;
        }

        if ((currentMoveDelay <= 0 || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) && currentBlock != null && currentYOffset + 1 <= 18)
        {
            currentMoveDelay = moveDelay;
            currentYOffset++;
        }

        if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && currentXOffset - 1 >= 0 && currentBlock != null)
        {
            currentXOffset--;
        }

        if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && currentXOffset + 1 <= 17 && currentBlock != null)
        {
            currentXOffset++;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        grid.Draw(gameTime, spriteBatch);

        if (currentBlock != null) 
        {
            DrawCurrentBlock(spriteBatch, currentBlock);
        }

        spriteBatch.End();
    }

    private void DrawCurrentBlock(SpriteBatch spriteBatch, TetrisBlock block) 
    {
        for (int i = 0; i < block.shape.GetLength(0); i++)
        { 
            for (int j = 0;  j < block.shape.GetLength(1); j++)
            {
                if (block.shape[i, j])
                {
                    int blockX = j * grid.cellWidth + (currentXOffset * grid.cellWidth);
                    int blockY = i * grid.cellHeight + (currentYOffset * grid.cellWidth);

                    spriteBatch.Draw(grid.emptyCell, new Rectangle(blockX, blockY, grid.cellWidth, grid.cellHeight), block.color);
                }
            }
        }
    }

    private void LockBlock()
    {
        for (int i = 0; i < currentBlock.shape.GetLength(0); i++)
        {
            for (int j = 0; j < currentBlock.shape.GetLength(1); j++)
            {
                if (currentBlock.shape[i, j])
                {
                    grid.cells[currentXOffset + i, currentYOffset + j] = currentBlock.color;
                }
            }
        }
        currentBlock = null;
    }
    public void Reset()
    {

    }
}
