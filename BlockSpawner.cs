using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisTemplate
{
    class BlockSpawner
    {
        private TetrisBlock currentBlock, nextBlock;

        private string[] possibleShapes = { "O", "I", "S", "Z", "L", "J", "T" };

        public BlockSpawner()
        {
            SpawnBlock();
        }

        public void SpawnBlock()
        {
            var rand = new Random();
            string newShape = possibleShapes[rand.Next(possibleShapes.Length)];

            if (nextBlock != null)
            {
                currentBlock = nextBlock;
                nextBlock = new TetrisBlock(newShape);
            }
            else
            {
                nextBlock = new TetrisBlock(newShape);
                SpawnBlock();
            }
        }

        public TetrisBlock GetNewBlock()
        {
            return currentBlock;
        }

        public TetrisBlock GetNextBlock()
        {
            return nextBlock;
        }
    }
}