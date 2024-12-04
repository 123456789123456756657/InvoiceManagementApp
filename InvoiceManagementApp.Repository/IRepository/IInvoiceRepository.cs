using InvoiceManagementApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementApp.Repository.IRepository
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> GetInvoiceByIdAsync(int id);
        Task AddInvoiceAsync(Invoice invoice);
    }
}
