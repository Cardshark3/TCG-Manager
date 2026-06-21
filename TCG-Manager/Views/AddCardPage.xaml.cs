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
        int Amount = int.Parse(AmountInput.Text);

        Card card = new Card(name, pack, cardID, isPromo, Amount, "");

        string image = extractor.GetImage(card);

        card.ImageURI = image;

        DAO.AddCard(card);
    }
}