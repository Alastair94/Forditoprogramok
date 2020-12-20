using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forditoprogram
{
    public partial class Form1 : Form
    {
        string[ , ] matrix = new string[12,7];
        TextBox[] textBoxes = new TextBox[84];
        string input;
        Automata automata;
        List<string[]> eredmeny;
        SoundPlayer winSound = new SoundPlayer(@"win.wav");
        SoundPlayer nopeSound = new SoundPlayer(@"nope.wav");

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Validal_Click(object sender, EventArgs e)
        {
            clearEredmenyLbls();
            input = txtB_Kif.Text;
            if (input == "")
            {
                return;
            }
            tablaToMatrix();
            automata = new Automata(input, matrix);
            lbl_Egyszerusitett.Text = automata.simple(input);
            eredmeny = automata.validal();
            for (int i = 0; i <eredmeny.Count - 1 ; i++)
            {
                lbl_InPut.Text += eredmeny[i][0] + "\n";
                lbl_Szabaly.Text += eredmeny[i][1] + "\n";
                lbl_Sorszam.Text += eredmeny[i][2] + "\n";
            }

            lbl_Eredmeny.Text = eredmeny[eredmeny.Count - 1][0];
            if (checkBox_Sound.Checked)
            {
                if (eredmeny[eredmeny.Count - 1][0].Length < 30)
                {
                    winSound.Play();
                }
                else
                {
                    nopeSound.Play();
                }
            }
            
        }

        private void tablaToMatrix()
        {
            int u = 0;
            foreach(Control curControl in this.Controls)
            {
                if(curControl.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox)curControl;
                    if(textBox.Name != "txtB_Kif")
                    {
                        //textBox.Text = u.ToString();
                        textBoxes[u] = textBox;
                        u++;
                    }
                }
            }
            u = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    matrix[i, j] = textBoxes[u].Text;
                    u++;
                }
            }
        }

        private void clearEredmenyLbls()
        {
            lbl_InPut.Text = "";
            lbl_Szabaly.Text = "";
            lbl_Sorszam.Text = "";
            lbl_Eredmeny.Text = "";
            lbl_Egyszerusitett.Text = "";
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
