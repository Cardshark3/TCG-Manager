using TCG_Manager.Models;
using TCG_Manager.Extractors;

namespace TCG_Manager.Views;

public partial class CardDetailsPage : ContentPage
{
	Card currentCard;

	public CardDetailsPage(Card card)
	{
		InitializeComponent();

		currentCard = card;

		CardImage.Source = currentCard.ImageURI;
		CardName.Text = currentCard.Name;
		CardPack.Text = currentCard.Pack;
		CardAmount.Text = currentCard.Amount.ToString();
	}
}