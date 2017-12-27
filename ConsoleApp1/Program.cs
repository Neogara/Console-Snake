using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace ConsoleApp1
{
    public class SnakeHead : SnakeElement
    {
        enum MoveSide { left, right, down, up };

        MoveSide nowMoveSite = MoveSide.left;

        List<SnakeElement> Elements = new List<SnakeElement>();

        public void AddNewEllement(int offsetX = 0) 
        {
        //offSetX need for spawn body when game started
            Elements.Insert(0, new SnakeElement(GetPosX() + offsetX, GetPosY()));
        }

        public SnakeHead(int startPosX, int startPosY, int countElements = 10) : base(startPosX, startPosY)
        {
            for (int i = countElements; i >= 1; i--)
            {
                AddNewEllement(i);
            }
        }

        public void Turn(ConsoleKey moveKey)
        {
            if (moveKey == ConsoleKey.W)
            {
                if (nowMoveSite != MoveSide.down) { nowMoveSite = MoveSide.up; }
            }
            if (moveKey == ConsoleKey.S)
            {
                if (nowMoveSite != MoveSide.up) { nowMoveSite = MoveSide.down; }
            }
            if (moveKey == ConsoleKey.A)
            {
                if (nowMoveSite != MoveSide.right) { nowMoveSite = MoveSide.left; }
            }
            if (moveKey == ConsoleKey.D)
            {
                if (nowMoveSite != MoveSide.left) { nowMoveSite = MoveSide.right; }
            }
        }

        public void Move()
        {
            this.lastPosX = GetPosX();
            this.lastPosY = GetPosY();
            if (nowMoveSite == MoveSide.left) { SetPosX(GetPosX() - 1); return; }
            else if (nowMoveSite == MoveSide.right) { SetPosX(GetPosX() + 1); return; }
            else if (nowMoveSite == MoveSide.up) { SetPosY(GetPosY() - 1); return; }
            else if (nowMoveSite == MoveSide.down) { SetPosY(GetPosY() + 1); return; }

        }

        public void MoveElements()
        {
            int _lastPosX = this.lastPosX;
            int _lastPosY = this.lastPosY;

            int tmpx;
            int tmpy;

            int i = 0;
            foreach (var item in Elements)
            {
                tmpx = Elements[i].GetPosX();
                tmpy = Elements[i].GetPosY();
                Elements[i].SetPosX(_lastPosX);
                Elements[i].SetPosY(_lastPosY);
                item.lastPosX = _lastPosX = tmpx;
                item.lastPosY = _lastPosY = tmpy;

                i++;
            }
        }

        public void DrawAllElements()
        {
            if (Elements == null) { return; }

            foreach (var item in Elements)
            {
                item.Draw();
            }
            Console.SetCursorPosition(
            Elements.Last<SnakeElement>().lastPosX,
            Elements.Last<SnakeElement>().lastPosY);
            Console.Write(" ");
        }

        public void ChangeSnakeColor(ConsoleColor newColor)
        {
            Console.ForegroundColor = newColor;
            Draw();
            DrawAllElements();
            Console.ResetColor();
        } // warning ! this func no used for change snake color in game time.

        public bool IsLose()
        {
            foreach (var item in Elements)
            {
                if ((GetPosX() == item.GetPosX()) && (GetPosY() == item.GetPosY()))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class SnakeElement
    {
        int posX;
        int posY;

        public int lastPosY;
        public int lastPosX;

        public SnakeElement(int startPosX, int startPosY)
        {
            SetPosX(startPosX);
            SetPosY(startPosY);
        }

        public void SetPosX(int posX)
        {
            if (posX >= Console.WindowWidth)
            {
                this.posX = 0;
            }
            else if (posX < 0)
            {
                this.posX = Console.WindowWidth - 1;
            }
            else
            {
                this.posX = posX;
            }
        }

        public void SetPosY(int posY)
        {
            if (posY >= Console.WindowHeight)
            {
                this.posY = 0;
            }
            else if (posY < 0)
            {
                this.posY = Console.WindowHeight - 1;
            }
            else
            {
                this.posY = posY;
            }
        }

        public int GetPosX() { return this.posX; }

        public int GetPosY() { return this.posY; }

        public virtual void Draw()
        {
            Console.SetCursorPosition(GetPosX(), GetPosY());
            Console.Write("█");
        }

    }

    public class Eat : SnakeElement
    {
        public Eat(int startPosX, int startPosY) : base(startPosX, startPosY)
        {   }

        public override void Draw()
        {
            Console.SetCursorPosition(GetPosX(), GetPosY());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");

            Console.ResetColor();
        }

    }

    class Program
    {
        static void MiddleWriteTextX(string text, int posY)
        {

            Console.SetCursorPosition((Console.WindowWidth / 2) - (text.Length / 2), posY);
            Console.Write(text);

        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Console Snake";
            Console.WindowHeight = 30;
            Console.WindowWidth = 50;

            bool IsEndGame = false;
            while (true)
            {
                Console.Clear();
                Random rnd = new Random(DateTime.Now.Second);

                SnakeHead snake = new SnakeHead(20, 5);
                Eat eat = new Eat(15, 5);

                while (IsEndGame == false)
                {
                    while (!Console.KeyAvailable)
                    {
                        snake.Move();
                        if (snake.IsLose() == true)
                        {
                            IsEndGame = true;
                            snake.ChangeSnakeColor(ConsoleColor.Red);

                            MiddleWriteTextX("You are lose !", 10);
                            MiddleWriteTextX("Press any key to restart", 11);
                            break;
                        }
                        if ((snake.GetPosX() == eat.GetPosX()) && (snake.GetPosY() == eat.GetPosY()))
                        {
                            snake.AddNewEllement();
                            eat = new Eat(rnd.Next(0, 80), rnd.Next(0, 25));
                        }
                        snake.MoveElements();
                        snake.Draw();
                        snake.DrawAllElements();
                        eat.Draw();

                        Thread.Sleep(50);
                    };
                    snake.Turn(Console.ReadKey(true).Key);
                    Console.CursorVisible = false;
                }
                IsEndGame = false;
            }

        }
    }
}

