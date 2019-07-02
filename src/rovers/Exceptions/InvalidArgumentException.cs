using System;

namespace rovers.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string ex) : base(ex){
        }
    }
}