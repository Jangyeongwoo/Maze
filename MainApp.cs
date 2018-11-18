using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeProgram
{
    class MainApp
    {
        static void Main(string[] args)
        {

            Maze maze = new Maze();
            maze.load("a.txt");
            PrintMaze print = new PrintMaze();
            print.print();

            StartMaze start = new StartMaze();
            maze.searchExit();
            print.print();





        }
    }
}
