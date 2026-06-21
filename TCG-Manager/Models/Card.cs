using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG_Manager.Models
{
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Pack { get; set; }
        public string CardID { get; set; } 
        public bool IsPromo { get; set; }
        public int Amount { get; set; }
        public string ImageURI { get; set; }
        public Card() 
        {
            Name = string.Empty;
            Pack = string.Empty;
            CardID = string.Empty;
            IsPromo = false;
            Amount = 0;
            ImageURI = string.Empty;
        }

        public Card(string name, string pack, string cardID, bool isPromo, int amount, string imageURI)
        {
            Name = name;
            Pack = pack;
            CardID = cardID;
            IsPromo = isPromo;
            Amount = amount;
            ImageURI = imageURI;
        }

        public string GetName() { return Name; }
        public string GetPack() { return Pack; }
        public string GetCardID() { return CardID; }
        public bool IsCardPromo() { return IsPromo; }
        public int GetAmount() { return Amount; } 
        public string GetImageURI() { return ImageURI; }

        public void SetName(string name) { Name = name; }
        public void SetPack(string pack) { Pack = pack; }
        public void SetCardID(string cardID) { CardID =  cardID; }
        public void SetIsPromo(bool isPromo) { IsPromo = isPromo; }
        public void SetAmount(int amount) { Amount = amount; }
        public void SetImageURI(string imageURI) { ImageURI = imageURI; }
    }
}
