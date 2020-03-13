using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Othello
{
    public enum eSystemReply
    {
        None,
        ValidMove,
        ErrorBadInput,
        ErrorBadLogicInput,
        PlayerHasNoMoves,
        PlayerCanMove,
        PlayerWantsAnotherGame,
        PlayerWantsToQuit,
        GameOver
    }


    public class GameManager
    {
        private Player m_BlackPlayer = null;
        private Player m_WhitePlayer = null;
        private Player m_CurrentPlayer = null;
        private int m_BlackPlayerScore;
        private int m_WhitePlayerScore;
        private Game m_CurrentGame = null;
        private CurrentGameForm m_GameUI = null;

   
        public void InitGame()
        {
            m_BlackPlayer = new Player(Color.Black);
            m_WhitePlayer = new Player(Color.White);
            m_CurrentPlayer = m_BlackPlayer;
            m_BlackPlayerScore = m_WhitePlayerScore = 0;

            GameSettingsForm startForm = new GameSettingsForm();
            startForm.ShowDialog();
            if (startForm.DialogResult == DialogResult.OK)
            {
                if (startForm.IsAgainstComputer)
                {
                    m_WhitePlayer.IsComputer = true;
                }

                m_CurrentGame = new Game(startForm.BoardSize);
                m_GameUI = new CurrentGameForm(startForm.BoardSize, this);
                m_CurrentGame.InitNewGame();

                startGame();
            }
        }

        private void anotherRound()
        {
            m_CurrentGame.RestartGame();
            m_CurrentPlayer = m_BlackPlayer;
            List<PossibleMove?> possibleMoves = m_CurrentGame.GetPossibleMoves(m_CurrentPlayer);
            m_CurrentGame.UpdateCellsForPossibleMoves(possibleMoves);
        }

        private void startGame()
        {
            List<PossibleMove?> possibleMoves = m_CurrentGame.GetPossibleMoves(m_CurrentPlayer);
            m_CurrentGame.UpdateCellsForPossibleMoves(possibleMoves);
            m_GameUI.FormBorderStyle= FormBorderStyle.Fixed3D;
            m_GameUI.Text = "Othello-Black's Turn";
            m_GameUI.ShowDialog();
        }

        public Cell GetCellFromBoard(int i_X, int i_Y)
        {
            return m_CurrentGame.Board[i_X, i_Y];
        }

        private void switchPlayer(Player i_Player)
        {
            if (i_Player == m_BlackPlayer)
            {
                m_CurrentPlayer = m_WhitePlayer;
                m_GameUI.Text = "Othello-White's Turn";
            }
            else
            {
                m_CurrentPlayer = m_BlackPlayer;
                m_GameUI.Text = "Othello-Black's Turn";
            }
        }

        public void DoIteration(int i_X, int i_Y)
        {
            bool playerCanMove = false;
            if (!m_CurrentPlayer.IsComputer)
            {
                m_CurrentGame.Move(i_X, i_Y, m_CurrentPlayer);

                switchPlayer(m_CurrentPlayer);
                playerCanMove = updatePossibleMoves();
            }

            if (m_CurrentPlayer.IsComputer && playerCanMove)
            {
                m_CurrentGame.DoComputerMove(m_CurrentPlayer);
                switchPlayer(m_CurrentPlayer);
                playerCanMove = updatePossibleMoves();
            }

            if (!playerCanMove)
            {
                switchPlayer(m_CurrentPlayer);
                playerCanMove = updatePossibleMoves();

                if (!playerCanMove)
                {
                    showEndOfGameMessage();
                }
            }
        }

        private bool updatePossibleMoves()
        {
            bool updated = false;

            List<PossibleMove?> possibleMoves = m_CurrentGame.GetPossibleMoves(m_CurrentPlayer);
            if (possibleMoves.Count != 0)
            {
                updated = true;
                m_CurrentGame.UpdateCellsForPossibleMoves(possibleMoves);
            }

            return updated;
        }

        private Player getWinner(out string o_Score)
        {
            Player winner = null; // if its a tie, we return null
            m_CurrentGame.UpdatePlayerScore(m_BlackPlayer);
            m_CurrentGame.UpdatePlayerScore(m_WhitePlayer);
            o_Score = String.Format("{0}/{1}", m_BlackPlayer.Score, m_WhitePlayer.Score);

            if (m_BlackPlayer.Score > m_WhitePlayer.Score)
            {
                winner = m_BlackPlayer;
                m_BlackPlayerScore++;
            }
            else if (m_WhitePlayer.Score > m_BlackPlayer.Score)
            {
                winner = m_WhitePlayer;
                o_Score = String.Format("{0}/{1}", m_WhitePlayer.Score, m_BlackPlayer.Score);
                m_WhitePlayerScore++;
            }

            return winner;
        }

        private void showEndOfGameMessage()
        {
            string score, endOfGameAnnounce;
            Player winner = getWinner(out score);

            if (winner != null)
            {
                endOfGameAnnounce = String.Format(
@"{0} Won!! ({1}) ({2}/{3})
Whould you like another round?",
                winner.ToString(), score, m_WhitePlayerScore, m_BlackPlayerScore);
            }
            else
            {
                endOfGameAnnounce = String.Format(
@"Its a tie ({0}) ({1}/{2})
Whould you like another round?",
                score, m_WhitePlayerScore, m_BlackPlayerScore);
            }

            DialogResult dialogResult = MessageBox.Show(endOfGameAnnounce, "Othello", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                anotherRound();
            }
            else
            {
                m_GameUI.Close();
            }
        }

    }
}