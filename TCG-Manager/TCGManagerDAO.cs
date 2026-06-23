using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG_Manager.Models;

namespace TCG_Manager
{

    internal class TCGManagerDAO
    {
        public const string DatabaseFilename = "TCGCardManagerDataBase.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);


        SQLiteAsyncConnection database;

        public TCGManagerDAO()
        {
            //check for existing database
            if (File.Exists(DatabasePath))
            {
                database = new SQLiteAsyncConnection(DatabasePath, Flags);
            }
            //create database
            else
            {
                database = new SQLiteAsyncConnection(DatabasePath, Flags);

                database.CreateTableAsync<Card>();

                for (int i = 0; i < 15; i++)
                {
                    //Card card = new Card($"Test{i}", $"TestPack{i}", $"AA{1}", true, 2, $"test.test{i}");
                    //database.InsertAsync(card).Wait();
                }
            }
        }

        public void AddCard(Card card)
        {
            database.InsertAsync(card);
        }
        public void RemoveCard(int id) 
        {
            database.DeleteAsync<Card>(id).Wait();
        }
        public Card GetCardById(int id) 
        {
            return database.FindAsync<Card>(id).Result;
        }
        public List<Card> GetAllCards()
        {
            return database.Table<Card>().ToListAsync().Result;
        }
        public int GetCardCount()
        {
            return database.Table<Card>().CountAsync().Result;
        }

        public void SetCardAmount(int id, int newAmount)
        {
            database.ExecuteAsync("UPDATE Card SET Amount = ? WHERE ID = ?", newAmount, id).Wait();
        }

    }
}
