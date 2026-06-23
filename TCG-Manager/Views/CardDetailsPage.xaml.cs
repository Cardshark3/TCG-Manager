using TCG_Manager.Models;
using TCG_Manager.Extractors;

namespace TCG_Manager.Views;

public partial class CardDetailsPage : ContentPage
{
    int MAX_COLUMN_COUNT = 5;
    int columnCount = 0;
    double columnWidth = 100;

	Card currentCard;

	CardExtractor extractor = new PriceChartingExtractor();
    TCGManagerDAO DAO = new TCGManagerDAO();

	public CardDetailsPage(Card card)
	{
		InitializeComponent();

		currentCard = card;

		CardImage.Source = currentCard.ImageURI;
		CardName.Text = currentCard.Name + $" #{card.CardID}";
		CardPack.Text = currentCard.Pack;
		CardAmount.Text = currentCard.Amount.ToString();

    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        // calculate how many column the screen can reasonably fit
        columnCount = (int)(width / columnWidth);
        if(columnCount > MAX_COLUMN_COUNT)
            columnCount = MAX_COLUMN_COUNT;
        // set the width request for the AveragePriceGrid here so the grid will be centerd on screen
        AveragePriceGrid.WidthRequest = columnCount * columnWidth;

        // get the data for the card to be displayed on the table
        // do this here as the doing it elsware will break formatting for the grid
        ExtractedData data = extractor.Extract(currentCard);
        Dictionary<string, string> GradePrices = data.GetGradePrices();

        int column = 0;
        int rowOffset = 0;
        foreach (var k in data.GetGradePrices().Keys)
        {
            AveragePriceGrid.AddColumnDefinition(new ColumnDefinition(columnWidth));
            Label key = new Label();
            Thickness margin = new Thickness(5, 0, 5, 0);
            key.Margin = margin;
            key.BackgroundColor = new Color(0.75f);
            key.TextColor = new Color(0, 0, 0);

            Label value = new Label();
            value.Margin = margin;
            value.BackgroundColor = new Color(0.50f);
            value.TextColor = new Color(0, 0, 0);

            key.Text = k;
            value.Text = GradePrices[k];
            AveragePriceGrid.Add(key, column, 0 + rowOffset);
            AveragePriceGrid.Add(value, column, 1 + rowOffset);

            column++;

            if (column == columnCount)
            {
                column = 0;
                AveragePriceGrid.AddRowDefinition(new RowDefinition());
                AveragePriceGrid.AddRowDefinition(new RowDefinition());
                rowOffset += 2;
            }
        }
    }

    private void UpdateCard(int id)
    {
        currentCard = DAO.GetCardById(id);
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        ConfirmDeleteStack.IsVisible = !ConfirmDeleteStack.IsVisible;
    }
    private void ConfirmDeleteButton_Clicked(object sender, EventArgs e)
    {
        DAO.RemoveCard(currentCard.Id);
    }

    private void decrementButton_Clicked(object sender, EventArgs e)
    {
        if (currentCard.Amount <= 1)
        {
            DeleteButton_Clicked(sender, e);
            return;
        }

        DAO.SetCardAmount(currentCard.Id, currentCard.Amount - 1);
        UpdateCard(currentCard.Id);
        CardAmount.Text = currentCard.Amount.ToString();
    }

    private void incrementButton_Clicked(object sender, EventArgs e)
    {
        DAO.SetCardAmount(currentCard.Id, currentCard.Amount + 1);
        UpdateCard(currentCard.Id);
        CardAmount.Text = currentCard.Amount.ToString();
    }
}