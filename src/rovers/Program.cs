using System;
using System.Diagnostics;

namespace rovers
{
    class Program
    {
        static void Main(string[] args){
            var sw = new Stopwatch();
            sw.Start();

            var p = new Plateau(5, 5);
//            p.AddRover(new Rover(1, 2, Direction.N, "LMLMLMLMM"));
//            p.AddRover(new Rover(3, 3, Direction.E, "MMRMMRMRRM"));
            p.AddRover(new Rover(3, 0, Direction.W, "MM"));
            p.AddRover(new Rover(1, 2, Direction.N, "LLMM"));

            IManager manager = new RoverManager(p);
            manager.Run();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}