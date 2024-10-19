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
    private TetrisBlock currentBlock, nextBlock;
    private Vector2 nextBlockPosition;

    private int currentXOffset = 0, currentYOffset = 0;
    private float currentMoveDelayY, moveDelayX = 0.075f, moveDelayMultiplier = 0.8f, currentMoveDelayX;
    public static float moveDelayY = 0.9f;

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");


        grid = new TetrisGrid();
        blockSpawner = new BlockSpawner();

        nextBlockPosition = new Vector2(grid.screenWidth * 0.65f, grid.screenHeight * 0.1f);
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        currentBlock = blockSpawner.GetNewBlock();
        nextBlock = blockSpawner.GetNextBlock();

        if (currentMoveDelayY <= 0 || (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) && currentMoveDelayY * moveDelayMultiplier <= 0) && currentBlock != null)
        {
            if (!CheckBlockCollision(currentXOffset, currentYOffset + 1))
            {
                currentYOffset++;
                currentMoveDelayY = moveDelayY;
            }
            else
            {
                Score.score += Score.blockScore;
                LockBlock();
                blockSpawner.SpawnBlock();
                currentYOffset = 0;
            }
        }

        if (keyboardState.IsKeyDown(Keys.R) && currentBlock != null)
        {
            bool[,] newShape = currentBlock.GetNextRotation();
            if (!CheckBlockCollision(currentXOffset, currentYOffset, newShape))
            {
                currentBlock.Rotate();
            }
        }

        if (currentBlock != null)
        {
            currentBlock.Update(gameTime);

            currentMoveDelayY -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentMoveDelayX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            currentMoveDelayY = moveDelayY;
            currentMoveDelayX = moveDelayX;
            currentXOffset = 0;
        }

        if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && currentMoveDelayX <= 0 && currentBlock != null)
        {
            if (!CheckBlockCollision(currentXOffset - 1, currentYOffset))
            {
                currentMoveDelayX = moveDelayX;
                currentXOffset--;
            }
        }

        if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && currentMoveDelayX <= 0 && currentBlock != null)
        {
            if (!CheckBlockCollision(currentXOffset + 1, currentYOffset))
            {
                currentMoveDelayX = moveDelayX;
                currentXOffset++;
            }
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

        if (nextBlock != null)
        {
            DrawNextBlock(spriteBatch, nextBlock);
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
                    int blockY = i * grid.cellHeight + (currentYOffset * grid.cellHeight);

                    spriteBatch.Draw(grid.emptyCell, new Rectangle(blockX, blockY, grid.cellWidth, grid.cellHeight), block.color);
                }
            }
        }
    }

    private bool CheckBlockCollision(int xOffset, int yOffset, bool[,] blockShape = null)
    {
        blockShape = blockShape ?? currentBlock.shape;

        for (int i = 0; i < blockShape.GetLength(0); i++)
        {
            for (int j = 0; j < blockShape.GetLength(1); j++)
            {
                if (blockShape[i, j])
                {
                    int gridX = xOffset + j;
                    int gridY = yOffset + i;

                    if (gridX < 0 || gridX >= grid.Width || gridY >= grid.Height)
                    {
                        return true; //collides with grid boundaries
                    }

                    if (gridY >= 0 && grid.cells[gridX, gridY] != Color.White)
                    {
                        return true; //overlap with another block
                    }
                }
            }
        }

        return false;
    }

    private void LockBlock()
    {
        for (int i = 0; i < currentBlock.shape.GetLength(0); i++)
        {
            for (int j = 0; j < currentBlock.shape.GetLength(1); j++)
            {
                if (currentBlock.shape[i, j])
                {
                    int gridX = currentXOffset + j;
                    int gridY = currentYOffset + i;

                    if (gridX >= 0 && gridX < grid.Width && gridY < grid.Height)
                    {
                        grid.cells[gridX, gridY] = currentBlock.color;
                    }
                }
            }
        }

        currentBlock = null;
    }

    private void DrawNextBlock(SpriteBatch spriteBatch, TetrisBlock block)
    {
        for (int i = 0; i < block.shape.GetLength(0); i++)
        {
            for (int j = 0; j < block.shape.GetLength(1); j++)
            {
                if (block.shape[i, j])
                {
                    int blockX = (int)nextBlockPosition.X + j * grid.cellWidth;
                    int blockY = (int)nextBlockPosition.Y + i * grid.cellHeight;

                    spriteBatch.Draw(grid.emptyCell, new Rectangle(blockX, blockY, grid.cellWidth, grid.cellHeight), block.color);
                }
            }
        }
    }

    public void Reset()
    {

    }
}