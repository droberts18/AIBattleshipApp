using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBattleshipApp
{
    class Board
    {
        private string owner;
        public int[,] gameBoard;
        private Ship[] ships;

        public Board()
        {
            owner = "user";
            gameBoard = new int[10, 10];
            ships = new Ship[5];
            setupUserShips();
        }
        public Board(string name)
        {
            owner = name;
            gameBoard = new int[10, 10];
            ships = new Ship[5];
            setupRandomShips();
        }

        private void setupUserShips()
        {
            ships[0] = new Ship(5, "horizontal", 0, 4);
            ships[1] = new Ship(4, "vertical", 6, 0);
            ships[2] = new Ship(3, "horizontal", 8, 3);
            ships[3] = new Ship(3, "horizontal", 7, 6);
            ships[4] = new Ship(2, "vertical", 4, 5);
        }

        // Selects a random setup of ships among 3 created
        // Randomization of ships is next, logic is region at bottom of this class file
        private void setupRandomShips()
        {
            Random rdm = new Random();
            int rdmSetup = rdm.Next(0, 3);

            switch (rdmSetup) {
                case 0:
                    ships[0] = new Ship(5, "horizontal", 0, 1);
                    ships[1] = new Ship(4, "vertical", 2, 1);
                    ships[2] = new Ship(3, "horizontal", 9, 2);
                    ships[3] = new Ship(3, "horizontal", 7, 4);
                    ships[4] = new Ship(2, "vertical", 3, 8);
                    break;
                case 1:
                    ships[0] = new Ship(5, "horizontal", 2, 1);
                    ships[1] = new Ship(4, "vertical", 5, 4);
                    ships[2] = new Ship(3, "horizontal", 0, 1);
                    ships[3] = new Ship(3, "vertical", 3, 8);
                    ships[4] = new Ship(2, "vertical", 4, 2);
                    break;
                default:
                    ships[0] = new Ship(5, "horizontal", 7, 3);
                    ships[1] = new Ship(4, "vertical", 3, 5);
                    ships[2] = new Ship(3, "horizontal", 9, 5);
                    ships[3] = new Ship(3, "vertical", 0, 1);
                    ships[4] = new Ship(2, "vertical", 0, 8);
                    break;
            }

            updateGameBoard();
        }

        private void updateGameBoard()
        {
            foreach(Ship s in ships)
            {
                foreach(shipSquare sq in s.shipLocation)
                {
                    gameBoard[sq.row, sq.col] = 1;
                }
            }
        }

        public string[] isHit(string coordinateName)
        {
            int row = (char.ToUpper(coordinateName[0]) - 64)-1;
            int col = (int)char.GetNumericValue(coordinateName[1]) - 1;

            if (gameBoard[row, col] == 1)
            {
                foreach(Ship s in ships)
                {
                    if(s.checkIfShipIsAtLocation(row, col))
                    {
                        if(s.getIsSunk())
                            return s.getOfficialCoordinatesOfShip();
                        return new string[] { "Not sunk" };
                    }

                }
            }
            return new string[] { "Miss" };
        }

        public bool hasAllShipsSunk()
        {
            foreach(Ship s in ships)
            {
                if (!s.getIsSunk())
                    return false;
            }
            return true;
        }

        public Ship[] getShips()
        {
            return ships;
        }

        #region Testing completely random ship placements
        //private void setupShips()
        //{
        //    int[] shipSizes = { 5, 4, 3, 3, 2 };

        //    for(int numSetupShips = 0; numSetupShips < ships.Length; numSetupShips++)
        //    {
        //        Ship s = new Ship(shipSizes[numSetupShips]);
        //        int[] shipCoordinates = new int[2];
        //        obtainRandomAvailableStartingCoordinate(s.getSize());

        //        // puts ship in a valid location if it is not already
        //        checkIfValidShipLocation(s, shipCoordinates);
        //        ships[numSetupShips] = s;
        //        testGameBoard();
        //    }
        //}
        //private void checkIfValidShipLocation(Ship s, int[] possibleCoordinates)
        //{
        //    // North
        //    if (s.getDirection() == 0 && s.getSize()-1 > possibleCoordinates[0])
        //    {
        //        resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //    }
        //    // South
        //    else if (s.getDirection() == 1 && s.getSize()-1 > 10 - possibleCoordinates[0])
        //    {
        //        resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //    }
        //    // East
        //    else if (s.getDirection() == 2 && s.getSize()-1 > 10 - possibleCoordinates[1])
        //    {
        //        resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //    }
        //    // West
        //    else if(s.getDirection() == 3 && s.getSize()-1 > possibleCoordinates[1])
        //    {
        //        resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //    }
        //    checkIfSquaresAreOccupied(s, possibleCoordinates);
        //}

        //private void checkIfSquaresAreOccupied(Ship s, int[] possibleCoordinates)
        //{
        //    // North
        //    if(s.getDirection() == 0)
        //    {
        //        for(int i = possibleCoordinates[0]; i >= possibleCoordinates[0] - s.getSize() + 1; i--)
        //        {
        //            Console.Write(possibleCoordinates[0]);
        //            if (gameBoard[i, possibleCoordinates[1]] == 1)
        //                resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //        }
        //    }
        //    // South
        //    else if (s.getDirection() == 1)
        //    {
        //        for (int i = possibleCoordinates[0]; i < possibleCoordinates[0] + s.getSize(); i++)
        //        {
        //            Console.Write(possibleCoordinates[0]);
        //            if (gameBoard[i, possibleCoordinates[1]] == 1)
        //                resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //        }
        //    }
        //    // East
        //    else if (s.getDirection() == 2)
        //    {
        //        for (int i = possibleCoordinates[1]; i < possibleCoordinates[1] + s.getSize(); i++)
        //        {
        //            Console.Write(possibleCoordinates[1]);
        //            if (gameBoard[i, possibleCoordinates[0]] == 1)
        //                resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //        }
        //    }
        //    // West
        //    else
        //    {
        //        for (int i = possibleCoordinates[1]; i >= possibleCoordinates[1] - s.getSize() + 1; i--)
        //        {
        //            Console.Write(possibleCoordinates[1]);
        //            if (gameBoard[i, possibleCoordinates[0]] == 1)
        //                resetShipDirectionAndStartingLocation(s, possibleCoordinates);
        //        }
        //    }

        //    // at this point, the possible coordinates are a valid spot for the ship
        //    s.setLocation(possibleCoordinates);
        //    updateGameBoard(s, possibleCoordinates);
        //}

        //private int[] obtainRandomAvailableStartingCoordinate(int shipSize)
        //{
        //    Random rdm = new Random();
        //    int row;
        //    int col;

        //    do
        //    {
        //        row = rdm.Next(0, 10);
        //        col = rdm.Next(0, 10);
        //    } while (gameBoard[row, col] == 1);

        //    int[] coordinates = { row, col };
        //    return coordinates;
        //}

        //private void resetShipDirectionAndStartingLocation(Ship s, int[] possibleCoordinates)
        //{
        //    // the ship's starting coordinate does not allow for them to face this direction while remaining inside the board
        //    s.faceRandomDirection();
        //    possibleCoordinates = obtainRandomAvailableStartingCoordinate(s.getSize());
        //    checkIfValidShipLocation(s, possibleCoordinates);
        //}

        //private void updateGameBoard(Ship s, int[] coordinates)
        //{
        //    // North
        //    if(s.getDirection() == 0)
        //    {
        //        for(int i = coordinates[0]; i > coordinates[0] - s.getSize() + 1; i--){
        //            gameBoard[i, coordinates[1]] = 1;
        //        }
        //    }
        //    // South
        //    else if (s.getDirection() == 1)
        //    {
        //        for (int i = coordinates[0]; i < coordinates[0] + s.getSize(); i++)
        //        {
        //            gameBoard[i, coordinates[1]] = 1;
        //        }
        //    }
        //    // East
        //    else if (s.getDirection() == 2)
        //    {
        //        for (int i = coordinates[1]; i < coordinates[1] + s.getSize(); i++)
        //        {
        //            gameBoard[coordinates[0], i] = 1;
        //        }
        //    }
        //    // West
        //    else
        //    {
        //        for (int i = coordinates[1]; i >= coordinates[1] - s.getSize() + 1; i--)
        //        {
        //            gameBoard[coordinates[0], i] = 1;
        //        }
        //    }
        //}
        #endregion
    }
}
