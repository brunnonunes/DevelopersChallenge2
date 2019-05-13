using OfxDocumentReader.App.Utilities;

namespace OfxDocumentReader.App.DataModel
{
    public class TransactionModel
    {
        public TransactionModel() { }

        public string Type { get; set; }

        public string DatePosted { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public string QueryKey { get; set; }

        public string GetHash()
        {
            return $"{ this.Type + this.DatePosted + this.Amount + this.Description}".GetHashString();
        }
    }
}
