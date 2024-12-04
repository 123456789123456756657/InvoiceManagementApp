using InvoiceManagementApp.Models.Models;
using InvoiceManagementApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementApp.Service.Invoices
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await _invoiceRepository.GetAllInvoicesAsync();
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int id)
        {
            return await _invoiceRepository.GetInvoiceByIdAsync(id);
        }
        public async Task AddInvoiceAsync(Invoice invoice)
        {
            if (invoice.Items == null || !invoice.Items.Any())
            {
                throw new ArgumentException("Invoice must have at least one item.");
            }
            invoice.TotalAmount = invoice.Items.Sum(item => item.Price * item.Quantity);

            await _invoiceRepository.AddInvoiceAsync(invoice);
        }

    }
}
