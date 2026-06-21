using Microsoft.Maui.Controls.Shapes;
using TCG_Manager.Models;
using static SQLite.TableMapping;

namespace TCG_Manager
{
    public partial class MainPage : ContentPage
    {
        private TCGManagerDAO DAO = new TCGManagerDAO();

        bool initalised = false;

        int COLUMN_COUNT = 10;

        GridLength GridWidth = new GridLength(200);
        GridLength GridHeight = new GridLength(300);

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //initalise and populate the grid here to make sure all of the columns are on screen
            //and no spill of the window
            //NOTE: could be used to dynamicly resize the gris on size change
            if (initalised == false)
            {
                GridWidth = new GridLength(width / COLUMN_COUNT);
                GridHeight = new GridLength(GridWidth.Value * 1.5);
                InitalizeGrid();
                PopulateGrid();
                initalised = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateGrid();
        }

        public void InitalizeGrid()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            //calculate how many rows need to be added to the page
            int cardCount = DAO.GetCardCount();

            int rowCount = 0;

            // check for a remainder comparing the amount of cards to the column count
            if (cardCount % COLUMN_COUNT == 0)
            {
                rowCount = cardCount / COLUMN_COUNT;
            }
            // if there is a remainder add an extra row 
            else
            {
                rowCount = cardCount / COLUMN_COUNT + 1;
            }

            for (int i = 0; i < COLUMN_COUNT; i++) 
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition(GridWidth));
            }
            for (int i = 0; i < rowCount; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition(GridHeight));
            }


            foreach (var row in MainGrid.RowDefinitions)
            {
                int rowNum = MainGrid.RowDefinitions.IndexOf(row);
                foreach (var col in MainGrid.ColumnDefinitions)
                {
                    int colNum = MainGrid.ColumnDefinitions.IndexOf(col);

                    Rectangle r = new Rectangle();
                    if ((colNum + rowNum) % 2 == 0)
                    {
                        r.Background = Color.Parse("Grey");
                    }
                    else
                    {
                        r.Background = Color.Parse("Black");
                    }
                    MainGrid.Add(r, colNum, rowNum);
                }
            }



        }

        // create each Stacklayout for each card in the database and add it to the grid
        // NOTE: constant individual database querying, could be an issue for large amounts of cards
        public void PopulateGrid()
        {
            int cardCount = DAO.GetCardCount();

            int gridRowPos = 0;
            int gridColumnPos = 0;

            for (int i = 1; i < cardCount + 1; i++)
            {
                Card card = DAO.GetCardById(i);

                MainGrid.Add(CreateCardStack(card), gridColumnPos, gridRowPos);

                gridColumnPos++;
                if (gridColumnPos == COLUMN_COUNT)
                {
                    gridColumnPos = 0;
                    gridRowPos++;
                }
            }
        }

        public StackLayout CreateCardStack(Card card)
        {
            StackLayout layout = new StackLayout();
            layout.VerticalOptions = LayoutOptions.Center;


            Label nameLabel = new Label();
            nameLabel.Text = card.GetName();
            nameLabel.HorizontalTextAlignment = TextAlignment.Center;
            layout.Children.Add(nameLabel);

            ImageButton imageButton = new ImageButton();
            imageButton.Source = card.ImageURI;
            imageButton.MaximumHeightRequest = 150;
            //imageButton.Clicked
            layout.Children.Add(imageButton);

            Label packLabel = new Label();
            packLabel.Text = card.GetPack();
            packLabel.HorizontalTextAlignment = TextAlignment.Center;
            layout.Children.Add(packLabel);

            Label amountLable = new Label();
            amountLable.Text = $"Amount: {card.GetAmount().ToString()}";
            amountLable.HorizontalTextAlignment = TextAlignment.Center;
            layout.Children.Add(amountLable);

            return layout;
        }
    }
}
