using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperSpace
{
    class Program
    {
        static void Main(string[] args)
        {
            Hyperspace game1 = new Hyperspace();
            game1.PlayGame();
  
           
        }
    }
    class Unit
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; }
        public string Symbol { get; set; }
        public bool IsSpaceRift { get; set; }

        //list that holds the obstacles you need to avoid
        static List<string> ObstacleList = new List<string>() { "*", ".", ":", ";", "'", "!", "?" };
        static Random rng = new Random();

        public Unit(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Symbol = ObstacleList[rng.Next(ObstacleList.Count())];
            this.Color = ConsoleColor.Cyan;
        }
        public Unit(int x, int y, ConsoleColor color, string symbol, bool isSpaceRift)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.Symbol = symbol;
            this.IsSpaceRift = isSpaceRift;
        }
        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Color;
            Console.WriteLine(Symbol);
        }
        
    }
    class Hyperspace
    {
        public int Score { get; set; }
        public int Speed { get; set; }
        public List<Unit> ObstacleList { get; set; }
        public Unit SpaceShip { get; set; }
        public bool Smashed { get; set; }
        
        private Random rng = new Random();

        public Hyperspace()
        {
            Console.BufferHeight = 40;
            Console.WindowHeight = 40;
            //Console.BufferWidth = 50; 
            Console.WindowWidth = 50;
            this.Score = 0;
            this.Speed = 0;
            this.ObstacleList = new List<Unit>();
            this.Smashed = false;
            this.SpaceShip = new Unit((Console.WindowWidth / 2) - 1, Console.WindowHeight- 1, ConsoleColor.Red, "@", false);
        }

        public void PlayGame()
        {
            while (Smashed == false)
            {
                int riftChance = rng.Next(0, 11);
                if (riftChance > 9)
                {
                    ObstacleList.Add(new Unit(rng.Next(0, Console.WindowWidth - 2), 2, ConsoleColor.Green, "%", true));
                }
                else
                {
                    ObstacleList.Add(new Unit(rng.Next(Console.WindowWidth- 2), 2));
                }
                MoveShip();
                MoveObstacles();
                DrawGame();

                if (Speed < 170)
                {
                    Speed++;
                }
                else
                {
                    //if speed gets up to 170 stay at that speed
                    Speed = 170;
                }
                System.Threading.Thread.Sleep(170 - Speed);
            }
        }

        /// <summary>
        /// will determine if the ship moves to the left or the right
        /// </summary>
        public void MoveShip()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                // move left is left arrow is pressed
                if (keyPressed.Key == ConsoleKey.LeftArrow && SpaceShip.X > 0)
                {
                    SpaceShip.X--;
                }
                    //move to the right if the right arrow key is pressed
                else if (keyPressed.Key == ConsoleKey.RightArrow && SpaceShip.X < (Console.WindowWidth - 2))
                {
                    SpaceShip.X++;
                }
                else
                {
                    //will display if incorrect option entered
                    Console.WriteLine("Invalid option");
                }
            }
        }

        public void  MoveObstacles()
        {
            List<Unit> newObstacleList = new List<Unit>();

            foreach (Unit unit in ObstacleList)
            {
                unit.Y++;
                
                if (unit.IsSpaceRift == false  && unit.X == SpaceShip.X && unit.Y == SpaceShip.Y )
                {
                    Speed -= 50;
                }
                if (unit.IsSpaceRift == false && unit.X == SpaceShip.X && unit.Y == SpaceShip.Y)
                {
                    Smashed = true;
                }
                if (unit.Y < Console.WindowHeight)
                {
                    newObstacleList.Add(unit);
                }
                    //keep rising in speed until reaches 170
                else if (Speed > 170)
                {
                    Score++;
                }

            }
            ObstacleList = newObstacleList;

        }

        public void DrawGame()
        {
            Console.Clear();
            SpaceShip.Draw();
            
            foreach (Unit unit in ObstacleList)
            {
                unit.Draw();
            }

            PrintAtPosition(20, 2, "Score: " + this.Score, ConsoleColor.Green);
            PrintAtPosition(20, 3, "Speed: " + this.Speed, ConsoleColor.Green);
        }

        public void PrintAtPosition(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
        }
    }
}
