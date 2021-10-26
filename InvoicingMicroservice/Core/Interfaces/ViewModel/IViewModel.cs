using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Interfaces.ViewModel
{
    public interface IViewModel
    {
        public int Id { get; }
    }
}
