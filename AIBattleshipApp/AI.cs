using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBattleshipApp
{
    class AI
    {
        private Board targetBoard;
        public PrologBrain brain;
        string[] directionArr;
        int[] previousHit;

        public AI()
        {
            targetBoard = new Board();
            brain = new PrologBrain();
            directionArr = new string[]{ "North", "South", "East", "West"};
            previousHit = new int[] { -1, -1 };
        }

        public int[] turn()
        {
            int[] guess = { };
            // if no prior data, random guess
            if (previousHit[0] == -1)
            {
                // Passes in random coordinates to Prolog to see if they are the location of a ship
                guess = getRandomGuess();

                checkPrologResponse(brain.guessAt(guess[0], guess[1]), guess[0], guess[1]);
                return guess;
            }
            // else if there is a ship that has been hit but not sunk, the AI can guess areas around the hit
            else
            {
                foreach(string direction in directionArr)
                {
                    if(brain.checkIfDirectionOnBoard(direction, previousHit[0], previousHit[1]))
                    {
                        switch (direction) {
                            case "North":
                                if (targetBoard.gameBoard[previousHit[0] - 1, previousHit[1]] == 0)
                                {
                                    guess = new int[] { previousHit[0] - 1, previousHit[1] };
                                    checkPrologResponse(brain.guessAt(guess[0], guess[1]), guess[0], guess[1]);
                                    return guess;
                                }
                                break;
                            case "South":
                                if (targetBoard.gameBoard[previousHit[0] + 1, previousHit[1]] == 0)
                                {
                                    guess = new int[] { previousHit[0] + 1, previousHit[1] };
                                    checkPrologResponse(brain.guessAt(guess[0], guess[1]), guess[0], guess[1]);
                                    return guess;
                                }
                                break;
                            case "East":
                                if (targetBoard.gameBoard[previousHit[0], previousHit[1] + 1] == 0)
                                {
                                    guess = new int[] { previousHit[0], previousHit[1] + 1 };
                                    checkPrologResponse(brain.guessAt(guess[0], guess[1]), guess[0], guess[1]);
                                    return guess;
                                }
                                break;
                            case "West":
                                if (targetBoard.gameBoard[previousHit[0], previousHit[1] - 1] == 0)
                                {
                                    guess = new int[] { previousHit[0], previousHit[1] - 1 };
                                    checkPrologResponse(brain.guessAt(guess[0], guess[1]), guess[0], guess[1]);
                                    return guess;
                                }
                                break;
                            default:
                                guess = getRandomGuess();
                                break;
                       }
                    }
                }
            }
            guess = getRandomGuess();
            checkPrologResponse(brain.guessAt(guess[0], guess[1]), guess[0], guess[1]);
            previousHit[0] = -1;
            previousHit[1] = -1;
            return guess;
        }

        private int[] getRandomGuess()
        {
            int row, col;
            do
            {
                row = getRandomRowOrColNumber();
                col = getRandomRowOrColNumber();
            } while (targetBoard.gameBoard[row, col] != 0);
            return new int[] { row,col};
        }

        private void checkPrologResponse(bool isShipLocation, int row, int col)
        {
            if (isShipLocation)
            {
                targetBoard.gameBoard[row, col] = 1;
                previousHit = new int[] { row, col };
                // checks to find out which ship the AI hit
                foreach(Ship s in targetBoard.getShips())
                {
                    if(s.checkIfShipIsAtLocation(row, col) && s.getIsSunk())
                    {
                        previousHit = new int[]{ -1, -1};
                    }
                }
            }
            else
            {
                targetBoard.gameBoard[row, col] = -1;
            }
        }

        private int getRandomRowOrColNumber()
        {
            CryptoRandom rng = new CryptoRandom();
            return rng.Next(10);
        }

        private void shuffleDirections()
        {
            CryptoRandom rng = new CryptoRandom();
            int n = directionArr.Length;
            while(n > 1)
            {
                int index = rng.Next(n--);
                string temp = directionArr[n];
                directionArr[n] = directionArr[index];
                directionArr[index] = temp;
            }
        }

        public Board getBoard()
        {
            return targetBoard;
        }
    }
}
