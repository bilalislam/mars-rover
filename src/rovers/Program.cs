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
            //in case of RIP result for 1. rover and ignore flagged point by 2. rover
            p.AddRover(new Rover(1, 2, Direction.N, "LMMM"));
            p.AddRover(new Rover(1, 2, Direction.N, "LMMMRM"));

            //in case of expected result
            p.AddRover(new Rover(1, 2, Direction.N, "LMLMLMLMM"));
            p.AddRover(new Rover(3, 3, Direction.E, "MMRMMRMRRM"));

            IManager manager = new RoverManager(p);
            manager.Run();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}