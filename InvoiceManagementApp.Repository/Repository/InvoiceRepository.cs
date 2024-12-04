using InvoiceManagementApp.Models.ApplicationContext;
using InvoiceManagementApp.Models.Models;
using InvoiceManagementApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementApp.Repository.Repository
{
    public class InvoiceRepository: IInvoiceRepository
    {
        private readonly ApplicationContextDbContext _context;

        public InvoiceRepository(ApplicationContextDbContext context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await _context.Invoices.Include(i => i.Items).ToListAsync();
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }
    }
}
