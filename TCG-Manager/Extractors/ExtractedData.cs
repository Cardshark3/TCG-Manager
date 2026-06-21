using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG_Manager.Extractors
{
    public class ExtractedData
    {

        Dictionary<string, string> GradePrices = new Dictionary<string, string>();

        public ExtractedData() 
        {
            
        }

        public void AddGradePrice(string type, string price) 
        { 
            GradePrices.Add(type, price);
        }

        public Dictionary<string, string> GetGradePrices() { return GradePrices; }
    }
}
