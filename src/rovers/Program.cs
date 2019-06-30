using System;

namespace rovers {
    class Program {
        static void Main (string[] args) {
            var p = new Plateau (5, 5);
            p.AddRover (new Rover (1, 2, Direction.N, "RML"));
            //p.AddRover(new Rover(3, 3, Direction.E, "MMRMMRMRRM"));
            p.Run ();

            Console.WriteLine (p.ToString ());
        }
    }
}