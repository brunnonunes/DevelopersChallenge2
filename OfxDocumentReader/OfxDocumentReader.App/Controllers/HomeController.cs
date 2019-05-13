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

        public IActionResult Index(string queryKey)
        {
            if (string.IsNullOrWhiteSpace(queryKey))
            {
                return View();
            }

            List<TransactionModel> transactionModelList = this._transactionDataBaseConnector.LoadTransactionsByQueryKey(queryKey);

            List<TransactionViewModel> transactionViewModelList = new List<TransactionViewModel>();
                       
            foreach (TransactionModel transactionModel in transactionModelList)
            {
                transactionViewModelList.Add(new TransactionViewModel
                {
                    Amount = transactionModel.Amount,
                    DatePosted = transactionModel.DatePosted,
                    Description = transactionModel.Description,
                    Type = transactionModel.Type
                });
            }

            return View(transactionViewModelList);
        }

        [HttpPost("UploadFiles")]
        public IActionResult UploadFiles(List<IFormFile> files)
        {
            if (!OfxFileValidator.Validate(files))
            {
                return View("Index");
            }

            List<TransactionModel> transactionList = TransactionUtility.GetDistinctTransactions(files);
            
            if (!transactionList.Any())
            {
                return RedirectToAction("Index");
            }

            this._transactionDataBaseConnector.SaveTransactions(transactionList);

            string queryKey = transactionList.FirstOrDefault().QueryKey;

            return RedirectToAction("Index", new { queryKey });
        }
    }
}
