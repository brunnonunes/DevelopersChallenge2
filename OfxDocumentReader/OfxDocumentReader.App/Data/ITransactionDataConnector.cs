using OfxDocumentReader.App.DataModel;
using System.Collections.Generic;

namespace OfxDocumentReader.App.Data
{
    public interface ITransactionDataConnector
    {
        void SaveTransaction(TransactionModel transactionModel);

        List<TransactionModel> LoadTransactions();

        List<TransactionModel> LoadTransactionsByQueryKey(string Key);

        void SaveTransactions(List<TransactionModel> transactionModelList);
    }
}
