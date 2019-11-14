using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain1
{
   /// <summary>
   /// In this class we will have a list of blocks, which represents the blockchain (Chain property).
   /// </summary>
   public class BlockChain
   {
      /// <summary>
      /// Define the difficulty of mining a new block
      /// </summary>
      private readonly int _proofOfWorkDifficulty;

      /// <summary>
      /// Will get the miner who creates the new block
      /// </summary>
      private readonly double _miningReward;

      /// <summary>
      /// List of pending transactions
      /// </summary>
      // Pending transactions will be added to the new blocks by the miners,
      // this way transitioning from pending transactions to processed transactions.
      private List<Transaction> _pendingTransactions;
      public List<Block> Chain { get; set; }

      /// <summary>
      /// We initialize all the properties and we create the Genesis Block using <see cref="CreateGenesisBlock"/> method
      /// </summary>
      /// <param name="proofOfWorkDifficulty"></param>
      /// <param name="miningReward"></param>
      public BlockChain(int proofOfWorkDifficulty, int miningReward)
      {
         _proofOfWorkDifficulty = proofOfWorkDifficulty;
         _miningReward = miningReward;
         _pendingTransactions = new List<Transaction>();
         Chain = new List<Block> { CreateGenesisBlock() };
      }

      public void CreateTransaction(Transaction transaction)
      {
         _pendingTransactions.Add(transaction);
      }

      /// <summary>
      /// Can be used by the miners to mine new blocks.
      /// We firstly add a new transaction, the reward for the miner and after that we call the mine method of the block,
      /// to calculate a valid hash for that block.
      /// When we already have a valid block, we set its previous hash to the last block’s hash, this way creating a valid chain.
      /// Lastly we clear the pending transactions, because those transactions were already added to a block,
      /// so those transactions are finished successfully.
      /// </summary>
      /// <param name="minerAddress"></param>
      public void MineBlock(string minerAddress)
      {
         Transaction minerRewardTransaction = new Transaction(null, minerAddress, _miningReward);
         _pendingTransactions.Add(minerRewardTransaction);
         Block block = new Block(DateTime.Now, _pendingTransactions);
         block.MineBlock(_proofOfWorkDifficulty);
         block.PreviousHash = Chain.Last().Hash;
         Chain.Add(block);
         _pendingTransactions = new List<Transaction>();
      }

      /// <summary>
      /// Is used to check the validity of the chain, to be sure that the chain was not hacked, was not tampered.
      /// </summary>
      /// <returns></returns>
      public bool IsValidChain()
      {
         for (int i = 1; i < Chain.Count; i++)
         {
            Block previousBlock = Chain[i - 1];
            Block currentBlock = Chain[i];
            if (currentBlock.Hash != currentBlock.CreateHash())
               return false;
            if (currentBlock.PreviousHash != previousBlock.Hash)
               return false;
         }
         return true;
      }


      /// <summary>
      /// Is used to calculate the balance of a user of the blockchain.
      /// </summary>
      /// <param name="address">User identifier</param>
      /// <returns></returns>
      public double GetBalance(string address)
      {
         double balance = 0;
         foreach (Block block in Chain)
         {
            foreach (Transaction transaction in block.Transactions)
            {
               if (transaction.From == address)
               {
                  balance -= transaction.Amount;
               }
               if (transaction.To == address)
               {
                  balance += transaction.Amount;
               }
            }
         }
         return balance;
      }

      private Block CreateGenesisBlock()
      {
         List<Transaction> transactions = new List<Transaction> { new Transaction("", "", 0) };
         return new Block(DateTime.Now, transactions, "0");
      }
   }
}
