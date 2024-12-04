using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementApp.Models.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation property for related invoice items
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

    }
}
