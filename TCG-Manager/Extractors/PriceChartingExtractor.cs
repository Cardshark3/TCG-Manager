using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TCG_Manager.Models;

namespace TCG_Manager.Extractors
{
    internal class PriceChartingExtractor : CardExtractor
    {
        public string baseUrl = "https://www.pricecharting.com/game/pokemon";

        List<string> alternateListings = new List<string>();

        public PriceChartingExtractor()
        {
            alternateListings.Add("completed-auctions-used");
            alternateListings.Add("completed-auctions-cib");
            alternateListings.Add("completed-auctions-new");
            alternateListings.Add("completed-auctions-graded");
            alternateListings.Add("completed-auctions-manual-only");
        }


        public override string GetImage(Card card)
        {
            string URI = "";

            string builtUrl = $"{baseUrl}-{card.Pack.Replace(" ", "-").ToLower()}/{card.Name.Replace(" ", "-").ToLower()}-{card.CardID}";

            //create http client and load the built url
            HttpClient client = new HttpClient();
            string http = client.GetStringAsync(builtUrl).Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(http);

            //get the specific node containing the first image
            string divID = "product_details";
            HtmlNode Node = doc.DocumentNode.SelectSingleNode($"//div[@id='{divID}']");
            Node = Node.SelectSingleNode("//img[@class='js-show-dialog']");

            //extract the image from the nodes html
            int pFrom = Node.OuterHtml.IndexOf("'") + 1;
            int pTo = Node.OuterHtml.IndexOf("'", pFrom);
            if (pFrom >= 1 && pTo > pFrom)
            {
                URI = Node.OuterHtml.Substring(pFrom, pTo - pFrom);
            }

            return URI;
        }

        public override bool VerifyCard(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
