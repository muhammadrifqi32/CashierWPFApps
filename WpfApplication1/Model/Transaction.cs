using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int PriceTotal { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public Transaction()
        {
            this.OrderDate = DateTimeOffset.Now.DateTime;
        }
        public Transaction(int priceTotal)
        {
            this.PriceTotal = priceTotal;
        }
    }
}
