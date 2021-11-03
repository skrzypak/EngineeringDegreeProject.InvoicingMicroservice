using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Exceptions
{
    public class TransferException : Exception
    {
        public TransferException(string msg) : base(msg)
        {
        }
    }
}
