using System;

namespace TetrisGame
{
    public class Queue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        };

        private readonly Random random = new Random();

        public Block Next { get; private set; }

        public Queue() 
        {
            Next = RandomBlock();
        }

        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetAndChange()
        {
            Block block = Next;

            do
            {
                Next = RandomBlock();
            }
            while (block.Id == Next.Id);

            return block;
        }
    }
}
