using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Newtonsoft.Json;

namespace BoardConstruction
{
    public class BoardSolutionTable : TableEntity
    {
        public string BoardSolutionJson { get; set; }

        public int BoardSize { get; set; }

        public int NumWordsInSolution { get; set; }

        public BoardSolutionTable() { }

        public BoardSolutionTable(BoardSolution solution)
        {
            this.RowKey = solution.GameBoardKey;
            this.PartitionKey = (Math.Abs(solution.GameBoardKey.GetHashCode() % 1000)).ToString();
            this.BoardSize = solution.SolvedBoard.Size;
            this.NumWordsInSolution = solution.SolutionWords.Count();

            string json = JsonConvert.SerializeObject(solution, Formatting.None);
            this.BoardSolutionJson = json;
        }
    }

    public class BoardPublisher
    {
        /// <summary>
        /// Will push the boards to Azure Tables storage 
        /// </summary>
        /// <param name="sol"></param>
        internal static void Publish(BoardSolution sol)
        {
            // Parse the connection string and return a reference to the storage account.
            string storageConfiguration = CloudConfigurationManager.GetSetting("StorageConnectionString");
            if (storageConfiguration == null)
            {
                storageConfiguration = Properties.Settings.Default.AzureStorageLocalConfig;
                // To debug with Fiddler
                // storageConfiguration = Properties.Settings.Default.AzureStorageLocalFiddler;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConfiguration);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("boardsoltuion");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            // Create a new BoardSolutionTable.
            BoardSolutionTable solTable = new BoardSolutionTable(sol);

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(solTable);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }
    }
}
