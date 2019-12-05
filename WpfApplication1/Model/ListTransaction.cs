 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    public class ListTransaction
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Transaction Transaction { get; set; }
        public Item Item { get; set; }

        public ListTransaction() { }

        public ListTransaction(int quantity, Transaction transaction, Item transactionItem)
        {
            this.Quantity = quantity;
            this.Transaction = transaction;
            this.Item = transactionItem;
        }
    }
}
