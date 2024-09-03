using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionChecker.Models
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Merchant { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
    }
}
