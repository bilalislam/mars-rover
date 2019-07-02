using System;

namespace rovers.Exceptions
{
    public class InvalidCoordinateException : Exception
    {
        public InvalidCoordinateException(string ex) : base(ex)
        {
        }
    }
}