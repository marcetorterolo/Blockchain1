using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain1
{
   /// <summary>
   /// BLOCK Class
   /// </summary>
   public class Block
   {
      /// <summary>
      /// Time of block creation (to have a history).
      /// </summary>
      private readonly DateTime _timeStamp;

      /// <summary>
      /// To mine a block, we have a rule (the proof-of-work) about hashes, the hash of a valid block must start with x number of zeros.
      /// </summary>
      private long _nonce;

      /// <summary>
      /// Contains the hash of the previous block in the chain.
      /// </summary>
      public string PreviousHash { get; set; }

      /// <summary>
      /// This is the data stored in the blocks; here I’ve used a list of transactions, just to have an example similar to cryptocurrencies
      /// </summary>
      // IMPORTANT: the list of transactions must be a private variable, I used it as a public variable,
      // just because I wanted to have access to it, to modify the transactions, this way demonstrating
      // change detection and chain validity/invalidity.
      public List<Transaction> Transactions { get; set; }

      /// <summary>
      /// Hash of the block calculated based on all the properties of the block (change detection).
      /// </summary>
      public string Hash { get; private set; }

      /// <summary>
      /// We initialize all the properties and using the <see cref="CreateHash"/> method we calculate the hash of the block based on all properties of the block.
      /// This way, if anything will change inside the block, it will have different hash, so a change can be easily detected.
      /// </summary>
      /// <param name="timeStamp"></param>
      /// <param name="transactions"></param>
      /// <param name="previousHash"></param>
      public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash = "")
      {
         _timeStamp = timeStamp;
         _nonce = 0;
         Transactions = transactions;
         PreviousHash = previousHash;
         Hash = CreateHash();
      }

      /// <summary>
      /// Will be used by the miners to create new valid blocks. 
      /// The input parameter defines the difficulty of calculating new hashes. 
      /// </summary>
      /// <param name="proofOfWorkDifficulty">Defines how many zeros must be at the beginning of the hash.</param>
      public void MineBlock(int proofOfWorkDifficulty)
      {
         string hashValidationTemplate = new String('0', proofOfWorkDifficulty);

         while (Hash.Substring(0, proofOfWorkDifficulty) != hashValidationTemplate)
         {
            _nonce++;
            Hash = CreateHash();
         }
         Console.WriteLine("Blocked with HASH={0} successfully mined!", Hash);
      }

      /// <summary>
      /// We use SHA256 to create a hash based on all the properties of the block.
      /// </summary>
      /// <returns></returns>
      public string CreateHash()
      {
         using (SHA256 sha256 = SHA256.Create())
         {
            string rawData = PreviousHash + _timeStamp + Transactions + _nonce;
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Encoding.Default.GetString(bytes);
         }
      }
   }
}
