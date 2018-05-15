using Prolog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// -Uses CSProlog NuGet package: https://github.com/jsakamoto/CSharpProlog
/// </summary>

namespace AIBattleshipApp
{

    class PrologBrain
    {
        public string prologCode;
        private PrologEngine pEngine;

        public PrologBrain()
        {
            setInitialProlog();
        }

        // setup Prolog logic
        private void setInitialProlog()
        {
            var prolog = new PrologEngine(persistentCommandHistory: false);

            // 0 represents an unsunked ship, 1 represents a sunken ship. H = horizontal, V = vertical. The 'five', 'four', etc. names refer to each ship's capacity
            prologCode = @"ships([ship(0, H, five),
                                  ship(0, V, four),
                                  ship(0, H, firstThree),
                                  ship(0, H, secondThree),
                                  ship(0, V, two)]).

                                  shipCoord(0, 4).
                                  shipCoord(0, 5).
                                  shipCoord(0, 6).
                                  shipCoord(0, 7).
                                  shipCoord(0, 8).
                                  shipCoord(6, 0).
                                  shipCoord(7, 0).
                                  shipCoord(8, 0).
                                  shipCoord(9, 0).
                                  shipCoord(8, 3).
                                  shipCoord(8, 4).
                                  shipCoord(8, 5).
                                  shipCoord(7, 6).
                                  shipCoord(7, 7).
                                  shipCoord(7, 8).
                                  shipCoord(4, 5).
                                  shipCoord(5, 5).

                                  hasShipBeenSunk(X) :-
                                        ships(ship(1, X)).

                                  hit(X, Y) :-
                                         shipCoord(X, Y).

                                  hasBeenShot(X, Y) :-
                                          shot(X, Y).

                                  guessNorth(RowId) :-
                                          RowId \= 0.

                                  guessSouth(RowId) :-
                                          RowId \= 10.

                                  guessEast(ColId) :-
                                          ColId \= 10.

                                  guessWest(ColId) :-
                                          ColId \= 0.
                                  ";
        }

        public bool guessAt(int row, int col)
        {
            // Prolog query checks to see if the location at row, col has been hit already
            //string prologQuery = prologCols[col] + "(" + prologRows[row] + "," + "0).";
            pEngine = new PrologEngine(persistentCommandHistory: false);
            pEngine.ConsultFromString(prologCode);
            string prologQuery = "hit(" + row + "," + col + ").";
            var solution = pEngine.GetFirstSolution(prologQuery);
            return solution.Solved;
        }


        public bool checkIfDirectionOnBoard(string direction, int row, int col)
        {
            string prologQuery = "";

            pEngine = new PrologEngine(persistentCommandHistory: false);
            pEngine.ConsultFromString(prologCode);
            if (direction == "North" || direction == "South")
                prologQuery = "guess" + direction + "(" + row + ").";
            if (direction == "East" || direction == "West")
                prologQuery = "guess" + direction + "(" + col + ").";
            var solution = pEngine.GetFirstSolution(prologQuery);
            return solution.Solved;
        }
    }
}


// INITIAL PROLOG CODE (not working)

//prologCode = @"board([row(0,0,0,0,0,0,0,0,0,0,rowOne),
//                            row(0,0,0,0,0,0,0,0,0,0,rowTwo), 
//                            row(0,0,0,0,0,0,0,0,0,0,rowThree), 
//                            row(0,0,0,0,0,0,0,0,0,0,rowFour),
//                            row(0,0,0,0,0,0,0,0,0,0,rowFive),
//                            row(0,0,0,0,0,0,0,0,0,0,rowSix),
//                            row(0,0,0,0,0,0,0,0,0,0,rowSeven),
//                            row(0,0,0,0,0,0,0,0,0,0,rowEight),
//                            row(0,0,0,0,0,0,0,0,0,0,rowNine), 
//                            row(0,0,0,0,0,0,0,0,0,0,rowTen)]).

//                            board(row(0,0,0,0,0,0,0,0,0,0,rowOne)).
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowTwo)). 
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowThree)). 
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowFour)).
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowFive)).
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowSix)).
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowSeven)).
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowEight)).
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowNine)).
//                            board(row(0,0,0,0,0,0,0,0,0,0,rowTen)).

//                      checkAllRowsForValue(Value) :-
//                            checkRowForValue(rowOne, Value);
//                            checkRowForValue(rowTwo, Value);
//                            checkRowForValue(rowThree, Value);
//                            checkRowForValue(rowFour, Value);
//                            checkRowForValue(rowFive, Value);
//                            checkRowForValue(rowSix, Value);
//                            checkRowForValue(rowSeven, Value);
//                            checkRowForValue(rowEight, Value);
//                            checkRowForValue(rowNine, Value);
//                            checkRowForValue(rowTen, Value).

//                      checkRowForValue(RowId, Value) :-
//                            checkFirstColumnForValue(RowId, Value);
//                            checkSecondColumnForValue(RowId, Value);
//                            checkThirdColumnForValue(RowId, Value);
//                            checkFourthColumnForValue(RowId, Value);
//                            checkFifthColumnForValue(RowId, Value);
//                            checkSixthColumnForValue(RowId, Value);
//                            checkSeventhColumnForValue(RowId, Value);
//                            checkEighthColumnForValue(RowId, Value);
//                            checkNinthColumnForValue(RowId, Value);
//                            checkTenthColumnForValue(RowId, Value).

//                      checkFirstColumnForValue(RowId, Value) :-
//                            board(row(Value,_,_,_,_,_,_,_,_,_,RowId)).

//                      checkSecondColumnForValue(RowId, Value) :-
//                            board(row(_,Value,_,_,_,_,_,_,_,_,RowId)).

//                      checkThirdColumnForValue(RowId, Value) :-
//                            board(row(_,_,Value,_,_,_,_,_,_,_,RowId)).

//                      checkFourthColumnForValue(RowId, Value) :-
//                            board(row(_,_,_,Value,_,_,_,_,_,_,RowId)).

//                      checkFifthColumnForValue(RowId, Value) :-
//                            board(row(_,_,_,_,Value,_,_,_,_,_,RowId)).

//                      checkSixthColumnForValue(RowId, Value) :-
//                            board(row(_,_,_,_,_,Value,_,_,_,_,RowId)).

//                      checkSeventhColumnForValue(RowId, Value) :-
//                            board(row(_,_,_,_,_,_,Value,_,_,_,RowId)).

//                      checkEighthColumnForValue(RowId, Value) :-
//                            board(row(_,_,_,_,_,_,_,Value,_,_,RowId)).

//                      checkNinthColumnForValue(RowId, Value) :-
//                            board(row(_,_,_,_,_,_,_,_,Value,_,RowId)).

//                      checkTenthColumnForValue(RowId, Value) :-
//                            board(row(_,_,_,_,_,_,_,_,_,Value,RowId)).

//                      guessNorth(RowId) :-
//                            RowId \= 0.

//                      guessSouth(RowId) :-
//                            RowId \= 10.

//                      guessWest(ColId) :-
//                            ColId \= 0.

//                      guessEast(ColId) :-
//                            ColId \= 10.
//                      ";
