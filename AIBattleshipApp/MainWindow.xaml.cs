using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIBattleshipApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board aiBoard = new Board("AI");
        AI prologAi = new AI();
        // list used to keep track of all the buttons the user has clicked that are hits on ships
        List<Button> shipHitButtons = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
            setupUIBoardForUserToClick();
            setupUIBoardForAI();
        }

        private void userGuess(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            BattleshipWindow.IsHitTestVisible = false;
            clicked.IsHitTestVisible = false;

            string[] isHitResults = aiBoard.isHit(clicked.Name);

            if (isHitResults[0] != "Miss")
            {
                // adding button to list of buttons that correspond to hit ships
                shipHitButtons.Add(clicked);

                if (isHitResults.Length > 1)
                {
                    foreach(string coordinateOfSunkenShip in isHitResults)
                    {
                        foreach(Button buttonOfCoordinateOfSunkenShip in shipHitButtons)
                        {
                            if (buttonOfCoordinateOfSunkenShip.Name == coordinateOfSunkenShip)
                                // if the ship has been sunk, change all of its corresponding buttons to gray
                                buttonOfCoordinateOfSunkenShip.Background = Brushes.Gray;
                        }
                    }
                }
                else {
                    // if a ship has been hit, but not yet sunk
                    clicked.Background = Brushes.Green;
                }
            }
            else
            {
                // if the shot was miss
                clicked.Background = Brushes.Red;
            }
            checkWinStatus();
            endTurn();
        }

        private void aiGuess(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;

            string[] isHitResults = prologAi.getBoard().isHit(clicked.Name);

            if (isHitResults[0] != "Miss")
            {
                clicked.Background = Brushes.DarkGray;
            }
            else
            {
                // if the shot was miss
                clicked.Background = Brushes.DarkSlateBlue;
            }
            checkWinStatus();
            BattleshipWindow.IsHitTestVisible = true;
        }

        private void checkWinStatus()
        {
            if (aiBoard.hasAllShipsSunk())
            {
                MessageBox.Show("Congratulations! You won!");
                Application.Current.Shutdown();
            }
            else if (prologAi.getBoard().hasAllShipsSunk())
            {
                MessageBox.Show("You lost! Try again!");
                Application.Current.Shutdown();
            }
        }

        private void setupUIBoardForUserToClick()
        {
            char rowLetter = ' ';
            const string possibleRowLetters = "ABCDEFGHIJ";

            for(int rowCounter = 0; rowCounter < 10; rowCounter++)
            {
                rowLetter = possibleRowLetters[rowCounter];
                for(int columnCounter = 0; columnCounter < 10; columnCounter++)
                {
                    Button newBtn = new Button();
                    newBtn.Name = rowLetter + Convert.ToString(columnCounter+1);
                    newBtn.Click += new RoutedEventHandler(userGuess);
                    newBtn.Background = Brushes.CadetBlue;
                    Grid.SetRow(newBtn, rowCounter);
                    Grid.SetColumn(newBtn, columnCounter);
                    aiUiBoard.Children.Add(newBtn);
                }
            }
        }

        private void setupUIBoardForAI()
        {
            char rowLetter = ' ';
            const string possibleRowLetters = "ABCDEFGHIJ";

            for (int rowCounter = 0; rowCounter < 10; rowCounter++)
            {
                rowLetter = possibleRowLetters[rowCounter];
                for (int columnCounter = 0; columnCounter < 10; columnCounter++)
                {
                    Button newBtn = new Button();
                    newBtn.IsHitTestVisible = false;
                    newBtn.Click += new RoutedEventHandler(aiGuess);
                    newBtn.Name = rowLetter + Convert.ToString(columnCounter + 1);

                    if (prologAi.getBoard().gameBoard[rowCounter, columnCounter] == 1)
                        newBtn.Background = Brushes.Gray;
                    else
                        newBtn.Background = Brushes.CadetBlue;

                    Grid.SetRow(newBtn, rowCounter);
                    Grid.SetColumn(newBtn, columnCounter);
                    userUiBoard.Children.Add(newBtn);
                }
            }
        }

        private void endTurn()
        {
            int[] coordsOfGuess = prologAi.turn();
            Button btn = userUiBoard.Children.Cast<Button>().First(e => Grid.GetRow(e) == coordsOfGuess[0] && Grid.GetColumn(e) == coordsOfGuess[1]);
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
