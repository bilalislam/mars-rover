using Xunit;
using rovers.Exceptions;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace rovers.tests
{
    public class RoverTest
    {
        [Theory]
        [InlineData(5, 5)]
        [InlineData(10, 10)]
        public void Should_Be_True_Coordinates_With_Plateau(int x, int y){
            var p = new Plateau(x, y);
            x += 1;
            y += 1;
            Assert.AreEqual(x * y, p._points.Length);
        }

        [Theory]
        [InlineData(-1, 6, Direction.E, "LMLMLMLMM")]
        public void Should_Throw_Invalid_Coordinates_Exception(int x, int y, Direction direction, string cmd){
            var p = new Plateau(5, 5);
            Assert.ThrowsException<InvalidCoordinateException>(() => p.AddRover(new Rover(x, y, direction, cmd)));
        }

        [Theory]
        [InlineData(1, 2, Direction.N, "LMLMLMLMM")]
        public void Should_Be_Valid_Rover_Coordinates_On_Plateau(int x, int y, Direction direction, string cmd){
            //Arrange
            var p = new Plateau(5, 5);

            //Act
            var rover = p.AddRover(new Rover(x, y, direction, cmd));

            //Assert
            Assert.AreEqual(1, rover.X);
            Assert.AreEqual(2, rover.Y);
            Assert.AreEqual(Direction.N, rover.Direction);
            Assert.IsNotNull(p._points[x, y].RoverOn);
        }

        [Theory]
        [InlineData(5, 5)]
        public void Should_Be_Throw_Rover_Cannot_Found_Exception(int x, int y){
            var p = new Plateau(x, y);
            IManager manager = new RoverManager(p);
            Assert.ThrowsException<RoverCannotFoundException>(() => manager.Run());
        }

        [Theory]
        [InlineData(5, 5, Direction.E, "ADASDAS123DAS")]
        public void Should_Be_Throw_Invalid_Argument_Exception(int x, int y, Direction direction, string cmd){
            var p = new Plateau(x, y);
            Assert.ThrowsException<InvalidArgumentException>(() => p.AddRover(new Rover(x, y, direction, cmd)));
        }

        [Theory]
        [InlineData(5, 5)]
        public void Should_Be_Throw_Crush_Exception(int x, int y){
            //Arrange
            var p = new Plateau(x, y);
            var rover1 = new Rover(3, 0, Direction.W, "MM");
            var rover2 = new Rover(1, 2, Direction.N, "LLMM");

            //Act
            p.AddRover(rover1);
            p.AddRover(rover2);

            IManager manager = new RoverManager(p);
            //Assert
            Assert.ThrowsException<CrushDetectedException>(() => manager.Run());
        }


        [Theory]
        [InlineData(5, 5)]
        public void Should_Be_Throw_Out_of_Plateau_Exception(int x, int y){
            //Arrange
            var p = new Plateau(x, y);
            var rover1 = new Rover(3, 0, Direction.W, "MMMM");
            var rover2 = new Rover(1, 2, Direction.N, "LLMMM");

            //Act
            p.AddRover(rover1);
            p.AddRover(rover2);

            IManager manager = new RoverManager(p);
            //Assert
            Assert.ThrowsException<OutOfPlateauException>(() => manager.Run());
        }

        [Theory]
        [InlineData(5, 5)]
        public void Should_Be_Valid_Coordinates_For_All_Rovers(int x, int y){
            //Arrange
            var p = new Plateau(x, y);
            var rover1 = new Rover(1, 2, Direction.N, "LMLMLMLMM");
            var rover2 = new Rover(3, 3, Direction.E, "MMRMMRMRRM");

            //Act
            p.AddRover(rover1);
            p.AddRover(rover2);

            IManager manager = new RoverManager(p);
            manager.Run();

            //Assert
            Assert.AreEqual("1 3 N", rover1.ToString());
            Assert.AreEqual("5 1 E", rover2.ToString());
        }
    }
}