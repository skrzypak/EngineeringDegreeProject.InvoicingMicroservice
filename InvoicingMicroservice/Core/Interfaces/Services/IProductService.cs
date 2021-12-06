using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface IProductService
    {
        public object Get(int espId);
    }
}
