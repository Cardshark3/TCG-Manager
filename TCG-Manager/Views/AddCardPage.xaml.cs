using TCG_Manager.Extractors;
using TCG_Manager.Models;

namespace TCG_Manager.Views;

public partial class AddCardPage : ContentPage
{
    private TCGManagerDAO DAO = new TCGManagerDAO();

    private CardExtractor extractor = new PriceChartingExtractor();

    public AddCardPage()
	{
		InitializeComponent();
	}

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        string name = NameInput.Text;
        string pack = PackInput.Text;
        string cardID = IDInput.Text;
        bool isPromo = IsPromoCheckbox.IsChecked;
        int amount = 0;

        try
        {
            amount = int.Parse(AmountInput.Text);
        }catch
        {
            ErrorLabel.Text = "Amount must be a number.";
            return;
        }

        if(amount <= 0)
        {
            ErrorLabel.Text = "The amount must be at least one.";
            return;
        }

        if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pack) || string.IsNullOrEmpty(cardID))
        {
            ErrorLabel.Text = "Please make sure all fields are filled.";
            return;
        }

        Card card = new Card(name, pack, cardID, isPromo, amount, "");

        string image = extractor.GetImage(card);

        card.ImageURI = image;

        DAO.AddCard(card);
    }
}