using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfxDocumentReader.App.Data;
using OfxDocumentReader.App.DataModel;
using OfxDocumentReader.App.Models;
using OfxDocumentReader.App.Utilities;

namespace OfxDocumentReader.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITransactionDataConnector _transactionDataBaseConnector;

        public HomeController(ITransactionDataConnector transactionDataBaseConnector)
        {
            this._transactionDataBaseConnector = transactionDataBaseConnector;
        }

        public IActionResult Index(int transactionQueryId)
        {
            List<TransactionModel> tmodellist = this._transactionDataBaseConnector.LoadTransactions();

            if (!tmodellist.Any())
            {
                return View(new List<TransactionViewModel>());
            }

            List<TransactionViewModel> vmlist = new List<TransactionViewModel>();

            foreach (TransactionModel tm in tmodellist)
            {
                vmlist.Add(new TransactionViewModel
                {
                    Amount = tm.Amount,
                    DatePosted = tm.DatePosted,
                    Description = tm.Description,
                    Type = tm.Type
                });
            }

            return View(vmlist);
        }

        [HttpPost("UploadFiles")]
        public IActionResult UploadFiles(List<IFormFile> files)
        {
            List<TransactionModel> transactionList = TransactionUtility.GetUniqueTransactions(files);

            this._transactionDataBaseConnector.SaveTransactions(transactionList);

            return RedirectToAction("Index", new { transactionQueryId = 99 });
        }
    }
}
