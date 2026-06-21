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
            //alternateListings.Add("completed-auctions-used");
            //alternateListings.Add("completed-auctions-cib");
            //alternateListings.Add("completed-auctions-new");
            //alternateListings.Add("completed-auctions-graded");
            //alternateListings.Add("completed-auctions-manual-only");
        }

        private string BuildURL(Card card)
        {
            return $"{baseUrl}-{card.Pack.Replace(" ", "-").ToLower()}/{card.Name.Replace(" ", "-").ToLower()}-{card.CardID}";
        }


        public override string GetImage(Card card)
        {
            string URI = "";

            string builtUrl = BuildURL(card);

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

        public override ExtractedData Extract(Card card)
        {
            ExtractedData data = new ExtractedData();

            string builtURL = BuildURL(card);

            HttpClient client = new HttpClient();

            string http = client.GetStringAsync(builtURL).Result;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(http);

            string tableDataID = "full-prices";

            HtmlNode tableNode = doc.DocumentNode.SelectSingleNode($"//div[@id='{tableDataID}']");

            foreach (HtmlNode row in tableNode.SelectNodes(".//tr"))
            {
                string key = "";
                string value = "";
                foreach (HtmlNode cell in row.SelectNodes(".//td"))
                {
                    if (cell.GetClasses().Count() != 0)
                    {
                        value = cell.InnerText;
                    }
                    else
                    {
                        key = cell.InnerText;
                    }
                }

                data.AddGradePrice(key, value);
            }

            return data;
        }
    }
}
