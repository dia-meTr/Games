using System;
using System.Threading;
using System.Text;

namespace Lab10_1
{
    class Program
    {
        static int ballX;
        static int ballY;
        static int playerX;
        static int up;
        static int right;
        static int[,] arr = new int[5, 13];

        static void start()
        {
            Console.SetWindowSize(118, 30);
            Console.BufferWidth = 118;
            Console.BufferHeight = 30;
            arr = Level1();
            ballX = Console.WindowWidth / 2 - 1;
            ballY = Console.WindowHeight - 5;
            playerX = Console.WindowWidth / 2 - 4;
            up = 1;
            right = 0;
        }

        static void MovePlayer()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.RightArrow && playerX < Console.WindowWidth - 8) // Right
            {
                playerX += 2;
            }
            if (keyInfo.Key == ConsoleKey.LeftArrow && playerX > 1) //Left
            {
                playerX -= 2;
            }
        }

        static void MoveBall()
        {
            if (ballY == Console.WindowHeight - 5) //Hit the platform
            {
                switch (ballX - playerX)
                {
                    case -1: up = 1; right = -3; break;
                    case 0: up = 1; right = -2; break;
                    case 1: up = 1; right = -1; break;
                    case 2: up = 2; right = -1; break;
                    case 3: up = 1; right = 0; break;
                    case 4: up = 2; right = 1; break;
                    case 5: up = 1; right = 1; break;
                    case 6: up = 1; right = 2; break;
                    case 7: up = 1; right = 3; break;
                    default:
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 18, Console.WindowHeight / 2);
                        Console.Write("You lose. Press Enter to start again");
                        Console.ReadLine();
                        start();
                        return;
                }
            }
            else if (ballY <= 1)//Hit the top
            {
                up = -1 * up;
                right = -1 * right;
            }
            else if (ballY - 2 <= arr.GetLength(0) * 2 - 1 && ballY % 2 == 1)//Hit a block
            {
                if (ballX % 9 != 0)
                {
                    if (arr[(ballY - 3) / 2, ballX / 9] > 0)
                    {
                        arr[(ballY - 3) / 2, ballX / 9] = arr[(ballY - 3) / 2, ballX / 9] - 1;
                        up = -1 * up;
                    }
                }
                else if (arr[(ballY - 3) / 2, (ballX + 1) / 9] > 0)
                {
                    arr[(ballY - 3) / 2, (ballX + 1) / 9] = arr[(ballY - 3) / 2, (ballX + 1) / 9] - 1;
                    up = -1 * up;
                }

                bool win = true;
                for (int i = 0; i < arr.GetLength(0); i++) //Are you winner?
                {
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (arr[i, j] != 0)
                        {
                            win = false; //No, you are not
                            break;
                        }
                        if (win == false) break;
                    }
                }
                if (win == true) //Yes, you are
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 18, Console.WindowHeight / 2);
                    Console.Write("You won. Press Enter to start again");
                    Console.ReadLine();
                    start();
                }
            }

            if (ballX > Console.WindowWidth - 5 || ballX <= 2) //Hit walls
            {
                right = -1 * right;
            }

            //Move Right
            if (ballX + right >= Console.WindowWidth - 2)
            {
                ballX = Console.WindowWidth - 2;
            }
            else if (ballX + right <= 2)
            {
                ballX = 2;
            }
            else
            {
                ballX = ballX + right;
            }

            //Move Up
            ballY = ballY - up;
        }

        static void DrawPlayer()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(playerX, Console.WindowHeight - 4);
            char c = '▀';
            for (int i = 0; i < 8; i++)
            {
                Console.Write(c);
            }
        }

        static void DrawBall()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(ballX - 1, ballY);
            Console.Write("▀██▀");
            Console.SetCursorPosition(ballX - 1, ballY - 1);
            Console.Write("▄██▄");
            Console.SetCursorPosition(1, 1);
        }

        static void DrawField() //Draw blocks
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                Console.SetCursorPosition(1, 1 + i * 2);
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    switch (arr[i, j]) //Coulors of blocks
                    {
                        case 0: Console.ForegroundColor = ConsoleColor.Black; break;
                        case 1: Console.ForegroundColor = ConsoleColor.Blue; break;
                        case 2: Console.ForegroundColor = ConsoleColor.Yellow; break;
                        case 3: Console.ForegroundColor = ConsoleColor.Red; break;
                    }
                    Console.Write("████████ ");
                }
            }
            Console.SetCursorPosition(1, 1);

        }

        static int[,] Level1() //array of blocks
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (i == 0 || i == 4) { arr[i, j] = 1; }
                    else if (j < 2 || j > 10) { arr[i, j] = 1; }
                    else if (i == 2) { arr[i, j] = 3; }
                    else if (j < 4 || j > 8) { arr[i, j] = 1; }
                    else arr[i, j] = 2;
                    //arr[i, j] = 0;
                }
            }
            //arr[4, 6] = 1;
            return arr;
        }

        static void Main(string[] args)
        {
            Console.Title = "Arcanoid";
            start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    MovePlayer();
                }

                MoveBall();
                Console.Clear();
                DrawField();
                DrawPlayer();
                DrawBall();
                Thread.Sleep(100);
            }
        }
    }
}