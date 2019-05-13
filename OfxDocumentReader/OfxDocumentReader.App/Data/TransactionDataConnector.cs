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
    /// <summary>
    /// Class that enables the connection to the database.
    /// </summary>
    public class TransactionDataConnector : ITransactionDataConnector
    {
        private readonly IOptions<TransactionDatabaseSettings> _transactionDatabaseSettings;

        public TransactionDataConnector(IOptions<TransactionDatabaseSettings> transactionDatabaseSettings)
        {
            this._transactionDatabaseSettings = transactionDatabaseSettings;
        }

        public void SaveTransaction(TransactionModel transactionModel)
        {
            const string SAVE_TRANSACTION_QUERY = @"insert into [Transaction] (Type, DatePosted, Amount, Description, QueryKey) values (@Type, @DatePosted, @Amount, @Description, @QueryKey)";

            using (IDbConnection connection = new SQLiteConnection(_transactionDatabaseSettings.Value.DefaultConnection))
            {
                connection.Execute(SAVE_TRANSACTION_QUERY, transactionModel);
            }
        }

        public void SaveTransactions(List<TransactionModel> transactionModelList)
        {
            foreach (TransactionModel transaction in transactionModelList)
            {
                this.SaveTransaction(transaction);
            }
        }

        public List<TransactionModel> LoadTransactionsByQueryKey(string queryKey)
        {
            const string LOAD_TRANSACTION_BY_QUERY_KEY = @"select Type, DatePosted, Amount, Description, QueryKey from [Transaction] where QueryKey = @QueryKey";

            using (IDbConnection connection = new SQLiteConnection(_transactionDatabaseSettings.Value.DefaultConnection))
            {
                IEnumerable<TransactionModel> output = connection.Query<TransactionModel>(LOAD_TRANSACTION_BY_QUERY_KEY, new { queryKey });

                return output.ToList();
            }
        }
    }
}
