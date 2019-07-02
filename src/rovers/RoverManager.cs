using System;
using System.Linq;
using rovers.Exceptions;

namespace rovers
{
    public class RoverManager : IManager
    {
        private readonly Plateau _plateau;

        public RoverManager(Plateau plateau){
            _plateau = plateau;
        }

        public void Run(){
            if (!_plateau.Rovers.Any()){
                throw new RoverCannotFoundException("Any rovers can not found !");
            }

            var maxTaskCount = _plateau.Rovers.Max(x => x.Command).Length;
            for (int i = 0; i < maxTaskCount; i++){
                foreach (var rover in _plateau.Rovers){
                    rover.RunCommand(i);
                }
            }

            ConsoleOutput();
        }

        private void ConsoleOutput(){
            for (int i = 1; i <= _plateau.Rovers.Count; i++){
                Console.WriteLine($"rover {i} : {_plateau.Rovers[i - 1]}");
            }

            Console.WriteLine($"plateau matris : \n {_plateau}");
        }
    }
}