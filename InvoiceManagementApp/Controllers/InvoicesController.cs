using InvoiceManagementApp.Models.Models;
using InvoiceManagementApp.Service.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InvoiceManagementApp.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly InvoiceService _invoiceService;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(InvoiceService invoiceService, ILogger<InvoicesController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
             
                var invoices = await _invoiceService.GetAllInvoicesAsync();

                return View(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error"); 
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state while creating an invoice.");
                    return View(invoice);
                }

                await _invoiceService.AddInvoiceAsync(invoice);
                _logger.LogInformation("Invoice created successfully with ID: {InvoiceId}", invoice.Id);

                TempData["SuccessMessage"] = "Invoice created successfully!";
                return RedirectToAction("Index");
            }
           
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}
