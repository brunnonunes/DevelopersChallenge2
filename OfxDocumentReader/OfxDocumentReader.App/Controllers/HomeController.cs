using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfxDocumentReader.App.Data;
using OfxDocumentReader.App.DataModel;
using OfxDocumentReader.App.Models;

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
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            // full path to file in temp location
            string filePath = Path.GetTempFileName();

            List<string> fileContent = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    using (StreamReader reader = new StreamReader(formFile.OpenReadStream()))
                    {
                        fileContent.AddRange(reader.ReadToEnd().Split("\r\n").ToList());
                    }
                }
            }

            List<TransactionModel> transactionList = new List<TransactionModel>();

            List<string> transactionHashList = new List<string>();

            TransactionModel transaction = new TransactionModel();

            foreach (string item in fileContent)
            {
                if (item.Contains("<STMTTRN>"))
                {
                    transaction = new TransactionModel();
                    continue;
                }

                if (item.Contains("<TRNTYPE>"))
                {
                    transaction.Type = item.Replace("<TRNTYPE>", "");
                    continue;
                }

                if (item.Contains("<DTPOSTED>"))
                {
                    transaction.DatePosted = item.Replace("<DTPOSTED>", "");
                    continue;
                }

                if (item.Contains("<TRNAMT>"))
                {
                    transaction.Amount = item.Replace("<TRNAMT>", "");
                    continue;
                }

                if (item.Contains("<MEMO>"))
                {
                    transaction.Description = item.Replace("<MEMO>", "");

                    string transactionHash = transaction.GetHash();

                    if (transactionHashList.Contains(transactionHash))
                    {
                        continue;
                    }

                    transactionHashList.Add(transactionHash);
                    transactionList.Add(transaction);
                    continue;
                }
            }

            this._transactionDataBaseConnector.SaveTransactions(transactionList);

            return RedirectToAction("Index", new { number = 99 });
        }
    }
}
