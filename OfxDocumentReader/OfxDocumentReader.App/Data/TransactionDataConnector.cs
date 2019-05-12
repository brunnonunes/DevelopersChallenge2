using Dapper;
using Microsoft.Extensions.Options;
using OfxDocumentReader.App.ConfigurationSettings;
using OfxDocumentReader.App.DataModel;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace OfxDocumentReader.App.Data
{
    public class TransactionDataConnector : ITransactionDataConnector
    {
        private readonly IOptions<TransactionDatabaseSettings> _transactionDatabaseSettings;

        public TransactionDataConnector(IOptions<TransactionDatabaseSettings> transactionDatabaseSettings)
        {
            this._transactionDatabaseSettings = transactionDatabaseSettings;
        }

        public void SaveTransaction(TransactionModel transactionModel)
        {
            using (IDbConnection connection = new SQLiteConnection(_transactionDatabaseSettings.Value.DefaultConnection))
            {
                try
                {
                    connection.Execute("insert into [Transaction] (Type, DatePosted, Amount, Description) values (@Type, @DatePosted, @Amount, @Description)", transactionModel);
                }
                catch (System.Exception ex)
                {
                    throw;
                }
            }
        }

        public List<TransactionModel> LoadTransactions()
        {
            using (IDbConnection connection = new SQLiteConnection(_transactionDatabaseSettings.Value.DefaultConnection))
            {
                IEnumerable<TransactionModel> output = connection.Query<TransactionModel>("select * from [Transaction]", new DynamicParameters());

                return output.ToList();
            }
        }

        public void SaveTransactions(List<TransactionModel> transactionModelList)
        {
            foreach (TransactionModel transaction in transactionModelList)
            {
                this.SaveTransaction(transaction);
            }
        }
    }
}
