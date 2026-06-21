using TCG_Manager.Models;
using TCG_Manager.Extractors;

namespace TCG_Manager.Views;

public partial class CardDetailsPage : ContentPage
{
	Card currentCard;

	CardExtractor extractor = new PriceChartingExtractor();

	public CardDetailsPage(Card card)
	{
		InitializeComponent();

		currentCard = card;

		CardImage.Source = currentCard.ImageURI;
		CardName.Text = currentCard.Name + $" #{card.CardID}";
		CardPack.Text = currentCard.Pack;
		CardAmount.Text = currentCard.Amount.ToString();

		ExtractedData data = extractor.Extract(card);
        Dictionary<string, string> GradePrices = data.GetGradePrices();

        int counter = 0;
        GridLength width = 100;
        foreach (var k in data.GetGradePrices().Keys)
        {
            AveragePriceGrid.AddColumnDefinition(new ColumnDefinition(width));
            Label key = new Label();
            Label value = new Label();

            key.Text = k;
            value.Text = GradePrices[k];
            AveragePriceGrid.Add(key, counter, 0);
            AveragePriceGrid.Add(value, counter, 1);

            counter++;
        }
    }
}