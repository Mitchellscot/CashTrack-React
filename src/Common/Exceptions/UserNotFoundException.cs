using System;

namespace CashTrack.Helpers.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {

        }
        public UserNotFoundException(string id) : base(String.Format($"No user found with an id of {id}"))
        {

        }
        public UserNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
