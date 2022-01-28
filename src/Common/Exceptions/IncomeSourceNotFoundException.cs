using System;

namespace CashTrack.Helpers.Exceptions
{
    public class IncomeSourceNotFoundException : Exception
    {
        public IncomeSourceNotFoundException()
        {

        }
        public IncomeSourceNotFoundException(string id) : base(String.Format($"Unable to find income source with id {id}"))
        {

        }
        public IncomeSourceNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
