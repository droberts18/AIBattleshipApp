using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBattleshipApp
{
    public struct shipSquare {
        // coordinate location - row
        public int row;
        // coordinate location - column
        public int col;
        // if the square has been hit or not
        public bool isHit;
    }

    class Ship
    {
        // shipSquares - array that correlates to the squares the ship resides in and if the square has been hit yet
        public shipSquare[] shipLocation;
        // direction - the way in which the ship is facing: 0 = North, 1 = South, 2 = East, 3 = West
        private string direction;
        // isSunk - true if all of its residing squares have been hit
        private bool isSunk;

        public Ship()
        {

        }

        public Ship(int size, string direction, int startRow, int startColumn)
        {
            shipLocation = new shipSquare[size];
            this.direction = direction;
            // starting square of ship (left-right, top-bottom)
            shipLocation[0].row = startRow;
            shipLocation[0].col = startColumn;
            fillOutRemainingLocationSquares();
            isSunk = false;
        }

        public void checkIfShipHasSunk()
        {
            foreach(shipSquare square in shipLocation)
            {
                if (!square.isHit)
                    return;
            }
            isSunk = true;
        }

        private void fillOutRemainingLocationSquares()
        {
            // left to right
            if(direction == "horizontal")
            {
                for(int i = 1; i < getSize(); i++)
                {
                    shipLocation[i].row = shipLocation[0].row;
                    shipLocation[i].col = shipLocation[0].col + i;
                }
            }
            // else vertical, which is top to bottom
            else
            {
                for (int i = 1; i < getSize(); i++)
                {
                    shipLocation[i].row = shipLocation[0].row + i;
                    shipLocation[i].col = shipLocation[0].col;
                }
            }
        }

        // checks which ship has been hit, updates the shipSquare that was hit if so
        public bool checkIfShipIsAtLocation(int row, int col)
        {
            for(int i = 0; i < shipLocation.Length; i++)
            {
                if (shipLocation[i].row == row && shipLocation[i].col == col)
                {
                    shipLocation[i].isHit = true;
                    checkIfShipHasSunk();
                    return true;
                }
            }
            return false;
        }

        public string[] getOfficialCoordinatesOfShip()
        {
            string[] officialCoords = new string[5];
            const string possibleRowLetters = "ABCDEFGHIJ";

            for(int i = 0; i < shipLocation.Length; i++)
            {
                string coordinates = possibleRowLetters[shipLocation[i].row] + Convert.ToString(shipLocation[i].col+1);
                officialCoords[i] = coordinates;
            }
            return officialCoords;
        }

        public string getDirection()
        {
            return direction;
        }

        public int getSize()
        {
            return shipLocation.Length;
        }

        public bool getIsSunk()
        {
            return isSunk;
        }
    }
}
