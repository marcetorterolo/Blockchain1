using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain1
{
   /// <summary>
   /// Transaction Class
   /// </summary>
   public class Transaction
   {
      /// <summary>
      /// Identifies the sender of the money.
      /// </summary>
      public string From { get; }

      /// <summary>
      /// Identifies the receiver of the money
      /// </summary>
      public string To { get; }

      /// <summary>
      /// Represents the amount of money sent.
      /// </summary>
      public double Amount { get; }

      public Transaction(string from, string to, double amount)
      {
         From = from;
         To = to;
         Amount = amount;
      }
   }
}
