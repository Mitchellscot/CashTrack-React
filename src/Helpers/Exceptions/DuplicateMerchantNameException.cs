using System;

namespace CashTrack.Helpers.Exceptions
{
    public class DuplicateMerchantNameException : Exception
    {
        public DuplicateMerchantNameException()
        {

        }
        public DuplicateMerchantNameException(string name) : base(String.Format($"Merchant already exists with the name {name}"))
        {

        }
        public DuplicateMerchantNameException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
