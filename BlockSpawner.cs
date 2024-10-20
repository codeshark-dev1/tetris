using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    class BlockSpawner
    {
        public GameWorld gameWorld;
        public TetrisGrid grid;

        private TetrisBlock currentBlock, nextBlock;

        private string[] possibleShapes = { "O", "I", "S", "Z", "L", "J", "T" };

        public BlockSpawner()
        {
        }

        public void SpawnBlock()
        {
            var rand = new Random();
            string newShape = possibleShapes[rand.Next(possibleShapes.Length)];

            if (nextBlock == null)
            {
                nextBlock = new TetrisBlock(newShape);
                SpawnBlock();
            }
            
            currentBlock = nextBlock;
            nextBlock = new TetrisBlock(newShape);

            int spawnY = -currentBlock.currentShapeOffset;
            gameWorld.SetSpawnPosition(spawnY);
        }

        public TetrisBlock GetNewBlock()
        {
            return currentBlock;
        }

        public TetrisBlock GetNextBlock()
        {
            return nextBlock;
        }

        public void Reset()
        {
            currentBlock = null;
            nextBlock = null;
        }
    }
}