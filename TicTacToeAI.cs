using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeAI : MonoBehaviour
{
    // MASTER MOVE FUNCTION //
    public int[,] MakeMove(int[,] board, Difficulty d, int turn)
    {
        if(d == Difficulty.Easy)
        {
            return EasyMove(board, turn);
        }
        else if(d == Difficulty.Medium)
        {
            return MediumMove(board, turn);
        }
        else if(d == Difficulty.Impossible)
        {
            return ImpossibleMove(board, turn);
        }

        return board;
    }

    // MOVE SUB-FUNCTIONS BASED ON DIFFICULTY //
    int[,] EasyMove(int[,] board, int turn)
    {
        while(true)
        {
            int r = Random.Range(0, 3);
            int c = Random.Range(0, 3);
            if(board[r, c] == -1)
            {
                board[r, c] = turn;
                return board;
            }
        }
    }

    int[,] MediumMove(int[,] board, int turn)
    {
        int enemy = Mathf.Abs(turn - 1);

        for(int row = 0; row < 3; row++)
        {
            int enemy_count = 0;
            for(int column = 0; column < 3; column++)
            {
                if(board[row, column] == enemy)
                {
                    enemy_count++;
                }
            } 
            if(enemy_count == 2)
            {
                for(int column = 0; column < 3; column++)
                {
                    if(board[row, column] == -1)
                    {
                        board[row, column] = turn;
                        return board;
                    }
                } 
            }
        }

        for(int column = 0; column < 3; column++)
        {
            int enemy_count = 0;
            for(int row = 0; row < 3; row++)
            {
                if(board[row, column] == enemy)
                {
                    enemy_count++;
                }
            } 
            if(enemy_count == 2)
            {
                for(int row = 0; row < 3; row++)
                {
                    if(board[row, column] == -1)
                    {
                        board[row, column] = turn;
                        return board;
                    }
                } 
            }
        }

        if(board[1, 1] == enemy)
        {
            if(board[0, 0] == enemy)
            {
                if(board[2, 2] == -1)
                {
                    board[2, 2] = turn;
                    return board;
                }
            }
            else if(board[2, 0] == enemy)
            {
                if(board[0, 2] == -1)
                {
                    board[0, 2] = turn;
                    return board;
                }
            }
            else if(board[2, 2] == enemy)
            {
                if(board[0, 0] == -1)
                {
                    board[0, 0] = turn;
                    return board;
                }
            }
            else if(board[0, 2] == enemy)
            {
                if(board[2, 0] == -1)
                {
                    board[2, 0] = turn;
                    return board;
                }
            }
        }

        return EasyMove(board, turn);
    }

    int[,] ImpossibleMove(int[,] board, int turn)
    {
        float best_score = -Mathf.Infinity;
        int best_row = -1;
        int best_column = -1;
        
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                if(board[row, column] == -1)
                {
                    board[row, column] = turn;
                    float score = Minimax(board, false, turn);
                    board[row, column] = -1;
                    if(score > best_score)
                    {
                        best_score = score;
                        best_row = row;
                        best_column = column;
                    }
                }
            } 
        }

        board[best_row, best_column] = turn;
        return board;
    }

    // HELPER FUNCTIONS //
    float Minimax(int[,] board, bool is_maximizer, int cpu_turn)
    {
        int player_turn = Mathf.Abs(cpu_turn - 1);

        if(IsWinner(cpu_turn, board))
        {
            return 1f;
        }
        else if(IsWinner(player_turn, board))
        {
            return -1f;
        }
        else if(IsFull(board))
        {
            return 0f;
        }

        if(is_maximizer)
        {
            float best_score = -Mathf.Infinity;
            for(int row = 0; row < 3; row++)
            {
                for(int column = 0; column < 3; column++)
                {
                    if(board[row, column] == -1)
                    {
                        board[row, column] = cpu_turn;
                        float score = Minimax(board, false, cpu_turn);
                        board[row, column] = -1;
                        best_score = Mathf.Max(score, best_score);
                    }
                } 
            }
            return best_score;
        }
        else
        {
            float best_score = Mathf.Infinity;
            for(int row = 0; row < 3; row++)
            {
                for(int column = 0; column < 3; column++)
                {
                    if(board[row, column] == -1)
                    {
                        board[row, column] = player_turn;
                        float score = Minimax(board, true, cpu_turn);
                        board[row, column] = -1;
                        best_score = Mathf.Min(score, best_score);
                    }
                } 
            }
            return best_score;
        }


        return 0;
    }

    bool IsWinner(int turn, int[,] board)
    {
        for(int row = 0; row < 3; row++)
        {
            if((board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2]) && board[row, 0] == turn)
            {
                return true;
            }
        }

        for(int column = 0; column < 3; column++)
        {
            if((board[0, column] == board[1, column] && board[1, column] == board[2, column]) && board[0, column] == turn)
            {
                return true;
            }
        }

        if(((board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) && board[0, 0] == turn) || ((board[2, 0] == board[1, 1] && board[1, 1] == board[0, 2]) && board[2, 0] == turn))
        {
            return true;
        }

        return false;
    }

    bool IsFull(int[,] board)
    {
        for(int row = 0; row < 3; row++)
        {
            for(int column = 0; column < 3; column++)
            {
                if(board[row, column] == -1)
                {
                    return false;
                }
            } 
        }
        return true;
    }
    
}
