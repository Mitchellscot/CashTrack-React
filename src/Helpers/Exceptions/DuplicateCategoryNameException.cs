using System;

namespace CashTrack.Helpers.Exceptions
{
    public class DuplicateCategoryNameException : Exception
    {
        public DuplicateCategoryNameException()
        {

        }
        public DuplicateCategoryNameException(string name) : base(String.Format($"There is already a sub category called {name}"))
        {

        }
        public DuplicateCategoryNameException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
