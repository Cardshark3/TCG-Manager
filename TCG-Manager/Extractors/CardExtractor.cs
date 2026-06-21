using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG_Manager.Models;

namespace TCG_Manager.Extractors
{
    public abstract class CardExtractor
    {
        public abstract bool VerifyCard(Card card);

        public abstract string GetImage(Card card);

        public abstract ExtractedData Extract(Card card);
    }
}
