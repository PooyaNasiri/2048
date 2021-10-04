using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace _2048
{
    public class AI
    {
        private static State currnetState;
        private static Agenda agenda;
        public static bool think = false;
        private static int[][] Board = new int[4][];
        private static int s;
        private static long Ls = 0, Rs = 0, Us = 0, Ds = 0;
        private static int[][] TempBoard = new int[4][];

        internal static void start(Game oGame)
        {
            
            currnetState = new State('.', "S", oGame.iBoard, 0);
            agenda = new Agenda(currnetState);
            s = oGame.iScore;
            string way = "";

            think = true;

            for (int i = 0; i < 4; i++)
            {
                TempBoard[i] = new int[4];
                for (int j = 0; j < 4; j++)
                    TempBoard[i][j] = oGame.iBoard[i][j];
            }

            Us = Ds = Ls = Rs = 0;
            way = currnetState.getWay();

            while (way.Length < 10)
            {

                for (int i = 0; i < 4; i++)
                {
                    Board[i] = new int[4];
                    for (int j = 0; j < 4; j++)
                        Board[i][j] = currnetState.getBoard()[i][j];
                }

                copy(oGame);

                oGame.moveBoard(Game.Direction.eUP);
                if (!_Equals(Board, oGame.iBoard))
                {
                    // scoring(Score(oGame.iBoard, oGame.iScore));
                    agenda.Add('U', way, oGame.iBoard, Score(oGame.iBoard, oGame.iScore));
                }

                copy(oGame);

                oGame.moveBoard(Game.Direction.eLEFT);
                if (!_Equals(Board, oGame.iBoard))
                {
                    //    scoring(Score(oGame.iBoard,oGame.iScore));
                    agenda.Add('L', way, oGame.iBoard, Score(oGame.iBoard, oGame.iScore));
                }

                copy(oGame);

                oGame.moveBoard(Game.Direction.eRIGHT);
                if (!_Equals(Board, oGame.iBoard))
                {
                    //    scoring(Score(oGame.iBoard, oGame.iScore));
                    agenda.Add('R', way, oGame.iBoard, Score(oGame.iBoard, oGame.iScore));
                }


                copy(oGame);

                oGame.moveBoard(Game.Direction.eDOWN);
                if (!_Equals(Board, oGame.iBoard))
                {
                    //   scoring(Score(oGame.iBoard, oGame.iScore));
                    agenda.Add('D', way, oGame.iBoard, Score(oGame.iBoard, oGame.iScore));
                }


                if (!agenda.Remove(ref currnetState)) break;
                way = currnetState.getWay();
            }

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    oGame.iBoard[i][j] = TempBoard[i][j];


            think = false;

            Decide(oGame);

            //   oGame.Update();


            //   if (oGame.bRender) oGame.Draw(Graphics.FromImage(new Bitmap(396, 600)));

            //Stopwatch sw = new Stopwatch(); // sw cotructor
            //sw.Start();
            //while (sw.ElapsedMilliseconds < 1000);

            // Thread.Sleep(10);
            //  start(oGame);
        }

        private static void scoring(long v)
        {
            switch (which_Way(currnetState))
            {
                case Game.Direction.eUP: Us += v; break;
                case Game.Direction.eDOWN: Ds += v; break;
                case Game.Direction.eLEFT: Ls += v; break;
                case Game.Direction.eRIGHT: Rs += v; break;
            }
        }
        static int[] t = new int[16];
        private static long Score(int[][] iBoard, int iScore)
        {
            int zeroCount = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (iBoard[i][j] == 0) zeroCount++;

            
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    t[j + (i * 4)] = iBoard[i][j];


            for (int i = 0; i < 30; i++)
                for (int j = 0; j < 15; j++)
                    if (t[j] < t[j + 1])
                    {
                        int t1 = t[j];
                        t[j] = t[j + 1];
                        t[j + 1] = t1;
                    }

            

            //if (iBoard[0][0] == 512) ss += 5120;
            //if (iBoard[0][0] == 1024)
            //{
            //    ss += 102400;
            //    if (iBoard[0][1] == 512) ss += 5120;
            //    if (iBoard[0][1] == 256) ss += 2560;
            //}

            //if (iBoard[0][0] == 2048)
            //{
            //    ss += 20480;
            //    if (iBoard[0][1] == 1024)ss += 10240;
            //    if (iBoard[0][2] == 512) ss += 5120;
            //    if (iBoard[0][1] == 512) ss += 5120;
            //}

            

            long a = //iScore - s +
                        (((iBoard[0][0] - 17) * 150) +
                        ((iBoard[1][0] - 9) * 80) +
                        ((iBoard[2][0] - 5) * 40) +
                        ((iBoard[3][0] - 3) * 20) +
                        (iBoard[0][1] * 10) +
                        (iBoard[1][1] * 12) +
                        (iBoard[2][1] * 14) +
                        (iBoard[3][1] * 16) +
                        (iBoard[0][2] * 8) +
                        (iBoard[1][2] * 6) +
                        (iBoard[2][2] * 4) +
                        (iBoard[3][2] * 2));

            long b = a / 10, c = -a/500;

            long ss = 0;
            if (iBoard[0][0] == t[0]) ss += b;
            if (iBoard[1][0] == t[1]) ss += b;
            if (iBoard[2][0] == t[2]) ss += b;
            if (iBoard[3][0] == t[3]) ss += b;
            if (iBoard[0][0] == 2 * iBoard[1][0] && iBoard[0][0] > 500) ss += b;
            if (iBoard[1][0] == 2 * iBoard[2][0] && iBoard[1][0] > 200) ss += b;
            if (iBoard[2][0] == 2 * iBoard[3][0] && iBoard[2][0] > 100) ss += b;
            if (iBoard[3][0] == 2 * iBoard[3][1] && iBoard[3][0] > 100) ss += b;

            a += ss + (((iBoard[3][0] > iBoard[2][0]) ? c : +b) +
                        ((iBoard[2][0] > iBoard[1][0]) ? c : +b) +
                        ((iBoard[1][0] > iBoard[0][0]) ? c : +b) +

                        ((iBoard[0][1] > iBoard[0][0]) ? c : +b) +
                        ((iBoard[1][1] > iBoard[1][0]) ? c : +b) +
                        ((iBoard[2][1] > iBoard[2][0]) ? c : +b) +
                        ((iBoard[3][1] > iBoard[3][0]) ? c : +b) +
                        
                        ((iBoard[0][2] > iBoard[0][1]) ? c : +b) +
                        ((iBoard[1][2] > iBoard[1][1]) ? c : +b) +
                        ((iBoard[2][2] > iBoard[2][1]) ? c : +b) +
                        ((iBoard[3][2] > iBoard[3][1]) ? c : +b));


            return (a * zeroCount) / (currnetState.getWay().Length);

        }

        private static void Decide(Game oGame)
        {
            State best = Agenda.start;
            currnetState = Agenda.start;
            Game.label1.Text = "";
            while (currnetState != null)
            {
                if (currnetState.getScore() > best.getScore())
                {
                    //  Game.label1.Text += best.getWay()+ " / ";
                    best = currnetState;
                }
                currnetState = currnetState.next;
            }

            Game.Direction d = which_Way(best);
            Game.label1.Text += "best: " + best.getWay() + " " + best.getScore() + " + " + t[0] + " + " + t[1];
            //int z = 0;
            //do
            //{

            switch (d)
            {
                case Game.Direction.eUP: oGame.moveBoard(Game.Direction.eUP); break;
                case Game.Direction.eLEFT: oGame.moveBoard(Game.Direction.eLEFT); break;
                case Game.Direction.eRIGHT: oGame.moveBoard(Game.Direction.eRIGHT); break;
                case Game.Direction.eDOWN: oGame.moveBoard(Game.Direction.eDOWN); break;
            }

            //    if (d == Game.Direction.eUP) d = Game.Direction.eLEFT;
            //    else if (d == Game.Direction.eLEFT) d = Game.Direction.eRIGHT;
            //    else if (d == Game.Direction.eRIGHT) d = Game.Direction.eDOWN;
            //    else if (d == Game.Direction.eDOWN) d = Game.Direction.eUP;

            //    Game.label1.Text += "d";
            //    z++;

            //    if (z > 1)
            //    {
            //        Game.currentGameState = Game.GameState.eAbout;
            //        oGame.Draw(Graphics.FromImage(new Bitmap(396, 600)));
            //    }
            //} while (_Equals(TempBoard, oGame.iBoard));


            //  int[] a = new int[4];
            //  long[] l = new long[4];

            //l[0] = Us;
            //l[1] = Ds;
            //l[2] = Ls;
            //l[3] = Rs;

            //a[0] = 1;
            //a[1] = 2;
            //a[2] = 3;
            //a[3] = 4;

            //for (int i = 0; i < 4; i++)
            //    for (int j = 0; j < 3; j++)
            //        if (l[j] < l[j + 1])
            //        {
            //            long t1 = l[j];
            //            l[j] = l[j + 1];
            //            l[j + 1] = t1;

            //            int t2 = a[j];
            //            a[j] = a[j + 1];
            //            a[j + 1] = t2;

            //        }

            //  Game.label1.Text = best.getScore() + " - " + oGame.iBoard[0][0] + " - " + which_Way(best).ToString();
            //int z = 0;
            //do
            //{
            //    switch (a[z])
            //    {
            //        case 1: oGame.moveBoard(Game.Direction.eUP); break;
            //        case 2: oGame.moveBoard(Game.Direction.eDOWN); break;
            //        case 3: oGame.moveBoard(Game.Direction.eLEFT); break;
            //        case 4: oGame.moveBoard(Game.Direction.eRIGHT); break;
            //    }
            //    z++;
            //} while (_Equals(TempBoard, oGame.iBoard));

        }

        private static Game.Direction which_Way(State state)
        {
            string s = (state.getWay().TrimStart("S.".ToCharArray()));
            if (s.StartsWith("U")) return Game.Direction.eUP;
            if (s.StartsWith("D")) return Game.Direction.eDOWN;
            if (s.StartsWith("L")) return Game.Direction.eLEFT;
            if (s.StartsWith("R")) return Game.Direction.eRIGHT;
            return 0;
        }

        private static void copy(Game oGame)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    oGame.iBoard[i][j] = Board[i][j];

            oGame.iScore = s;
        }

        static bool _Equals(int[][] a, int[][] b)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (a[i][j] != b[i][j])
                        return false;
            return true;
        }

    }

    internal class State
    {
        internal State next;
        private char name;
        private const int length = 4;
        private string way;
        private long score;
        private int[][] Board;


        internal State(char name, string way, int[][] C, long score)
        {
            Board = new int[length + 1][];
            for (int i = 0; i < length; i++)
            {
                Board[i] = new int[4];
                for (int j = 0; j < length; j++)
                    Board[i][j] = C[i][j];

            }
            this.name = name;
            this.way = way + name;
            this.score = score;
        }


        internal string getWay()
        {
            return way;
        }
        internal int[][] getBoard()
        {
            return this.Board;
        }

        internal long getScore()
        {
            return this.score;
        }
    }
    
    internal class Agenda
    {
        private State front, rear;
        public static State start;
        internal Agenda(State currnetState)
        {

            front = currnetState;
            rear = currnetState;
            front.next = rear;
            start = currnetState;
            start.next = front;

        }
        internal bool Add(char name, string way, int[][] Board, long score)
        {
            State s = new State(name, way, Board, score);
            rear.next = s;
            rear = s;
            return true;
        }
        internal bool Remove(ref State item)
        {
            if (front == null) return false;
            item = front;
            if (front == rear) rear = front = null;
            else front = front.next;
            return true;
        }
    }
}
