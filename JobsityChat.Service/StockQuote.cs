using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Service
{
    public class StockQuote
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
    }
}
