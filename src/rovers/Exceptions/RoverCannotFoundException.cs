using System;

namespace rovers.Exceptions
{
    public class RoverCannotFoundException : Exception
    {
        public RoverCannotFoundException(string ex) : base(ex)
        {
        }
    }
}