using Microsoft.AspNetCore.Http;
using OfxDocumentReader.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfxDocumentReader.App.Utilities
{
    public class TransactionUtility
    {
        public TransactionUtility(){}

        public static List<TransactionModel> GetDistinctTransactions(List<IFormFile> files) {

            List<string> fileContentList = new List<string>();

            foreach (var formFile in files)
            {
                fileContentList.AddRange(OfxFileToStringConverter.Convert(formFile).Split("\r\n").ToList());
            }

            List<TransactionModel> transactionList = new List<TransactionModel>();

            List<string> transactionHashList = new List<string>();

            TransactionModel transaction = new TransactionModel();

            Guid transactionQueryId = Guid.NewGuid();

            foreach (string item in fileContentList)
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
                    transaction.QueryKey = transactionQueryId.ToString();
                    transactionList.Add(transaction);
                    continue;
                }
            }

            return transactionList;
        }
    }
}
