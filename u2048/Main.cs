
using System;
using System.Drawing;
using System.Windows.Forms;

namespace _2048
{
    public partial class Main : Form
    {
        private Game oGame;

        private Graphics gGraphics, gG;
        private Bitmap bBackground;
        private bool run = false;


        //private long lFPSTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //private int iNumOfFPS, nNumOfFPS;

        public Main()
        {
            InitializeComponent();

            bBackground = new Bitmap(396, 600);

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            gGraphics = this.CreateGraphics();
            gG = Graphics.FromImage(bBackground);

            oGame = new Game();
            CFG.getInstance();
        }
        

        public void Draw(Graphics g)
        {
            g.Clear(Color.FromArgb(251, 248, 239));
            oGame.Draw(g);
        }
        

        private void Timer1_Tick(object sender, EventArgs e)
        {
            oGame.Update();
            
            if (oGame.bRender) { 
                Draw(gG);
                gGraphics.DrawImage(bBackground, new Point(0, 0));
            }
            if (run) AI.start(oGame);
        }
        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            _move(e);
        }

        private void _move(KeyEventArgs e)
        {
            if (!oGame.kTOP && !oGame.kRIGHT && !oGame.kBOTTOM && (e.KeyCode == Keys.A || e.KeyCode == Keys.Left))
            {
                //  oGame.kLEFT = true;
                oGame.moveBoard(Game.Direction.eLEFT);
            }
            else if (!oGame.kLEFT && !oGame.kRIGHT && !oGame.kBOTTOM && (e.KeyCode == Keys.W || e.KeyCode == Keys.Up))
            {
                //  oGame.kTOP = true;
                oGame.moveBoard(Game.Direction.eUP);
            }
            else if (!oGame.kTOP && !oGame.kLEFT && !oGame.kBOTTOM && (e.KeyCode == Keys.D || e.KeyCode == Keys.Right))
            {
                //  oGame.kRIGHT = true;
                oGame.moveBoard(Game.Direction.eRIGHT);
            }
            else if (!oGame.kTOP && !oGame.kRIGHT && !oGame.kLEFT && (e.KeyCode == Keys.S || e.KeyCode == Keys.Down))
            {
                //  oGame.kBOTTOM = true;
                oGame.moveBoard(Game.Direction.eDOWN);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            run = checkBox1.Checked;
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (oGame.kLEFT && (e.KeyCode == Keys.A || e.KeyCode == Keys.Left))
                oGame.kLEFT = false;

            if (oGame.kTOP && (e.KeyCode == Keys.W || e.KeyCode == Keys.Up))
                oGame.kTOP = false;

            if (oGame.kRIGHT && (e.KeyCode == Keys.D || e.KeyCode == Keys.Right))
                oGame.kRIGHT = false;

            if (oGame.kBOTTOM && (e.KeyCode == Keys.S || e.KeyCode == Keys.Down))
                oGame.kBOTTOM = false;
        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            _move(e);
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            run = checkBox1.Checked;
        }

        private void Main_MouseClick(object sender, MouseEventArgs e){ oGame.checkButton(e.X, e.Y); }
    }
}
