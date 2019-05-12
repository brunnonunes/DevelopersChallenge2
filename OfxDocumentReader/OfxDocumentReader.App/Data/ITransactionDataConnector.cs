using OfxDocumentReader.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfxDocumentReader.App.Data
{
    public interface ITransactionDataConnector
    {
        void SaveTransaction(TransactionModel transactionModel);

        List<TransactionModel> LoadTransactions();

        void SaveTransactions(List<TransactionModel> transactionModelList);
    }
}
