using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Difficulty
{
    Easy, Medium, Impossible, TwoPlayer
}

public class TicTacToeBoard : MonoBehaviour
{
    // VALUES FOR BOARD //
    int[,] board = new int[3, 3];

    GameObject[,] spaces = new GameObject[3, 3];
    
    // VALUES USED FOR SPRITE POSITIONING //
    const float X_START_X = -0.2f;
    const float X_START_Y = 2.4f;
    const float X_SPACING_X = 2.36f;
    const float X_SPACING_Y = 2.39f;
    
    const float O_START_X = -0.23f;
    const float O_START_Y = 2.4f;
    const float O_SPACING_X = 2.4f;
    const float O_SPACING_Y = 2.4f;

    const float BLANK_START_X = -0.22f;
    const float BLANK_START_Y = 2.4f;
    const float BLANK_SPACING_X = 2.376f;
    const float BLANK_SPACING_Y = 2.37f;

    // TRACKERS //
    int turn = 0;
    int computer_turn = 1;
    bool game_time = false;
    Difficulty difficulty = Difficulty.Easy;
    TicTacToeAI ai;

    float move_delay;

    public TMP_Dropdown difficulty_dropdown;
    public TMP_Dropdown turn_dropdown;
    

    // VICTORY DISPLAY CONTROLLER //
    VictoryDisplay victory_controller;

    // MAIN FUNCTIONS //
    void Start()
    {
        move_delay = 0.5f;

        ai = GetComponent<TicTacToeAI>();

        victory_controller = GetComponent<VictoryDisplay>();
    }

    void Update()
    {
        // Check if a space has been clicked, update it if so
        if(game_time)
        {
            if(difficulty != Difficulty.TwoPlayer)
            {
                if(turn != computer_turn)
                {
                    StartCoroutine(UpdateClickedSpaces());
                }

                VictoryCheck();

                if(turn == computer_turn)
                {
                    UpdateComputerSpaces();
                }

                VictoryCheck();
            }
            else
            {
                StartCoroutine(UpdateClickedSpaces());

                VictoryCheck();
            }
        }    
    }


    // HELPER FUNCTIONS //

    //--GAME DATA FUNCTIONS--//
    void InitializeBoard()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                board[row, column] = -1;
            }
        }
    }

    int CheckForThree()
    {
        // Check rows
        for(int row = 0; row < 3; row++)
        {
            if((board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2]) && board[row, 0] != -1)
            {
                return board[row, 0];
            }
        }

        // Check columns
        for(int column = 0; column < 3; column++)
        {
            if((board[0, column] == board[1, column] && board[1, column] == board[2, column]) && board[0, column] != -1)
            {
                return board[0, column];
            }
        }

        // Check diagonals
        if(((board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) && board[0, 0] != -1) || ((board[2, 0] == board[1, 1] && board[1, 1] == board[0, 2]) && board[2, 0] != -1))
        {
            return board[1, 1];
        }

        // Check for draw
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                if(board[row, column] == -1)
                {
                    return -1;
                }
            }
        }
        
        return 2;
    }

    void ChangeBoardValue(int row, int column, int value)
    {
        board[row, column] = value;
    }

    //--UI FUNCTIONS--//
    public void StartGameTime()
    {
        turn = 0;

        InitializeBoard();

        InitializeSpaces();

        ActivateBlankSpaces();

        game_time = true;
    }

    public void ResetGame()
    {
        game_time = false;
        turn = 0;

        ChangeDifficulty(difficulty_dropdown);
        XorO(turn_dropdown);

        InitializeSpaces();
        InitializeBoard();
        victory_controller.ResetText();
    }

    public void ChangeDifficulty(TMP_Dropdown dropdown)
    {
        if(!game_time)
        {
            difficulty = (Difficulty)dropdown.value;
        }
    }

    public void XorO(TMP_Dropdown dropdown)
    {
        if(!game_time)
        {
            computer_turn = Mathf.Abs(dropdown.value - 1);
        }
    }

    void EndGameTime()
    {
        game_time = false;
        DeactivateBlankSpaces();
    }

    //--AI FUNCTIONS--//
    int ComputerMove()
    {
        if(difficulty != Difficulty.TwoPlayer || turn == computer_turn)
        {
            board = ai.MakeMove(board, difficulty, turn);
            turn = Mathf.Abs(turn - 1);
            return 1;
        }
        return 0;
    }

    //--GRAPHICAL FUNCTIONS--//
    void LoadX(int row, int column)
    {
        Destroy(spaces[row, column]);
        spaces[row, column] =  GameObject.Instantiate(Resources.Load("x")) as GameObject;
        spaces[row, column].transform.position = new Vector3(X_START_X + (column * X_SPACING_X), X_START_Y - (row * X_SPACING_Y), 0);
    }

    void LoadO(int row, int column)
    {
        Destroy(spaces[row, column]);
        spaces[row, column] =  GameObject.Instantiate(Resources.Load("o")) as GameObject;
        spaces[row, column].transform.position = new Vector3(O_START_X + (column * O_SPACING_X), O_START_Y - (row * O_SPACING_Y), 0);
    }

    void InitializeSpaces()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                Destroy(spaces[row, column]);
                spaces[row, column] = GameObject.Instantiate(Resources.Load("BlankSpace")) as GameObject;
                spaces[row, column].transform.position = new Vector3(BLANK_START_X + (column * BLANK_SPACING_X), BLANK_START_Y - (row * BLANK_SPACING_Y), 0);
                spaces[row, column].GetComponent<BlankTile>().Deactivate();
            }
        }
    }

    IEnumerator UpdateClickedSpaces()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                if(spaces[row, column].GetComponent<BlankTile>() != null)
                {
                    if(spaces[row, column].GetComponent<BlankTile>().clicked)
                    {
                        if(turn == 0)
                        {
                            LoadX(row, column);
                            ChangeBoardValue(row, column, turn);
                            DeactivateBlankSpaces();
                            if(difficulty != Difficulty.TwoPlayer)
                            {
                                yield return new WaitForSeconds(move_delay);
                            }
                            ActivateBlankSpaces();
                            turn = 1;
                        }
                        else
                        {
                            LoadO(row, column);
                            ChangeBoardValue(row, column, turn);
                            DeactivateBlankSpaces();
                            if(difficulty != Difficulty.TwoPlayer)
                            {
                                yield return new WaitForSeconds(move_delay);
                            }
                            ActivateBlankSpaces();
                            turn = 0;
                        }
                    }
                }
            }
        }
    }

    void UpdateComputerSpaces()
    {
        int moved = ComputerMove();
        if(moved != 0)
        {
            for(int row = 0; row < 3; row++)
            {
                for(int column = 0; column < 3; column++)
                {
                    if(board[row, column] == 0)
                    {
                        LoadX(row, column);

                    }
                    else if(board[row, column] == 1)
                    {
                        LoadO(row, column);
                    }
                }
            }
        }
    }

    void VictoryCheck()
    {
        if(CheckForThree() != -1)
        {
            if(CheckForThree() == 0)
            {
                victory_controller.DisplayWinner(0);
            }
            else if(CheckForThree() == 1)
            {
                victory_controller.DisplayWinner(1);
            }
            else
            {
                victory_controller.DisplayWinner(2);
            }
            EndGameTime();
        }
    }

    void DeactivateBlankSpaces()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                if(spaces[row, column].GetComponent<BlankTile>() != null)
                {
                    spaces[row, column].GetComponent<BlankTile>().Deactivate();
                }
            }
        }
    }

    void ActivateBlankSpaces()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                if(spaces[row, column].GetComponent<BlankTile>() != null)
                {
                    spaces[row, column].GetComponent<BlankTile>().Activate();
                }
            }
        }
    }

    // TEST FUNCTIONS //
    void InitializeAllX()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                spaces[row, column] = GameObject.Instantiate(Resources.Load("x")) as GameObject;
                spaces[row, column].transform.position = new Vector3(X_START_X + (column * X_SPACING_X), X_START_Y - (row * X_SPACING_Y), 0);
            }
        }
    }

    void InitializeAllO()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                spaces[row, column] = GameObject.Instantiate(Resources.Load("o")) as GameObject;
                spaces[row, column].transform.position = new Vector3(O_START_X + (column * O_SPACING_X), O_START_Y - (row * O_SPACING_Y), 0);
            }
        }
    }

    void UpdateAllX()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                Destroy(spaces[row, column]);
                spaces[row, column] = GameObject.Instantiate(Resources.Load("x")) as GameObject;
                spaces[row, column].transform.position = new Vector3(X_START_X + (column * X_SPACING_X), X_START_Y - (row * X_SPACING_Y), 0);
            }
        }
    }

    void UpdateAllO()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                //Destroy(spaces[row, column]);
                spaces[row, column] = GameObject.Instantiate(Resources.Load("o")) as GameObject;
                spaces[row, column].transform.position = new Vector3(O_START_X + (column * O_SPACING_X), O_START_Y - (row * O_SPACING_Y), 0);
            }
        }
    }
}

