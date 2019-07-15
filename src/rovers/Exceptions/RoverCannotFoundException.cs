using System;

namespace rovers.Exceptions
{
    public class RoverCannotFoundException : Exception
    {
        public RoverCannotFoundException(string ex) : base(ex){
        }
    }

    public class CrushDetectedException : Exception
    {
        public CrushDetectedException(string ex) : base(ex){
        }
    }

    public class OutOfPlateauException : Exception
    {
        public OutOfPlateauException(string ex) : base(ex){
        }
    }
}