using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace AI_Project
{
    public partial class Form1 : Form
    {

        ProcessWord PW_obj = new ProcessWord();
        Grammar Gr_obj = new Grammar();
        

        public Form1()
        {
            InitializeComponent();
            
        }
           
        

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtBox.Copy();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtBox.Clear();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtBox.Undo();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
                rtBox.LoadFile(open.FileName, RichTextBoxStreamType.PlainText);
            this.Text = open.FileName;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text Document(*.txt)|*.txt|All Files(*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
               rtBox.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
            this.Text = save.FileName;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtBox.Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtBox.Paste();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtBox.Redo();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog f = new FontDialog();

            f.Font = rtBox.SelectionFont;
            if (f.ShowDialog() == DialogResult.OK)
            {
                rtBox.SelectionFont = f.Font;
                


            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            ColorDialog clr = new ColorDialog();
            if (clr.ShowDialog() == DialogResult.OK)
                rtBox.ForeColor = clr.Color;
        }
     
       
        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void rtBox_TextChanged(object sender, EventArgs e)
        {
            bool Correct = false;
            if(rtBox.Text.Length>=2)
            Predict();

            if (rtBox.Text[rtBox.Text.Length - 1] == '.')
            {
                string[] w = Gr_obj.GetCurrentSentenceAndWord(rtBox.Text, rtBox.Text.Length);
                Correct = Gr_obj.CheckSentence1(w);
                Correct = Gr_obj.CheckSentence2(w);
                if (Correct)
                {
                    WarningsLabel.Text += "\n Sentence is Correct";


                }
                else
                {
                    WarningsLabel.Text += "\n Sentence is Incorrect";
                }
            
            }
            CheckSpelling();
            
        }
       
        
        
        
        
        
        



        void Predict()
        {


            int lp = 0;
            string AllText = null;





            SuggestionBox.Items.Clear();
            AllText = rtBox.Text.ToString();




            char[] AllTextArr = AllText.ToCharArray();
            string CurrWord;
            int i = AllTextArr.Length;
            int len = i;
            i = i - 2;
            int j = i + 1;
            CurrWord = PW_obj.GetCurrentWord(AllText, len);
            try
            {
                char StartingChar = AllTextArr[j];
                ;

                ;
                if (rtBox.SelectionStart - lp > 2 || i == -1 || Char.IsWhiteSpace(AllTextArr[i]))
                {
                    ;

                    if (SuggestionBox != null)
                    {
                        SuggestionBox.Items.Clear();

                    }
                    string[] PredictedList = PW_obj.PredictFromWord(CurrWord);


                    ;


                    ;
                    SuggestionBox.Items.AddRange(PredictedList);

                    ;

                    SuggestionBox.Enabled = true;
                }
                else
                {
                    SuggestionBox.Items.Clear();

                    SuggestionBox.Enabled = false; ;


                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message);

                
            }

            lp = rtBox.SelectionStart;
        
        }




        void CheckSpelling()
        {


            string text;
            string status = null;
            string currentword=null;
            text = rtBox.Text;
            char[] currentText = text.ToCharArray();
            int i = (currentText.Length);
            i--;

            if (currentText.Length > 0 && (currentText[i] == ' ' || currentText[i] == '.'))
            {
                currentword = PW_obj.GetCurrentWord(text, currentText.Length);
                status = PW_obj.CheckSpell(currentword);
                WarningsLabel.Text = "Last Word '" + currentword + "' is spelled " + status+"ly.";
            }



           
        
        
        
        
        
        
        
        
        
        }

        private void SuggestionBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string ReplacingWord = SuggestionBox.SelectedItem.ToString();
                if (ReplacingWord != null)
                {
                    string alltext = rtBox.Text;


                    int len = alltext.Length;


                    string cur = PW_obj.GetCurrentWord(alltext, len);





                    char[] AllTextArray = new char[(len - cur.Length) + ReplacingWord.Length];

                    for (int j = 0; j < (len - cur.Length); j++)
                    {

                        AllTextArray[j] = alltext[j];



                    }



                    int index = PW_obj.PredictionIndex(alltext, len);
                    int i = 0;
                    for (i = 0; i < ReplacingWord.Length; i++)
                    {

                        AllTextArray[index] = ReplacingWord[i];


                        index++;

                    }

                    alltext = string.Join("", AllTextArray);
                    rtBox.Text = alltext;
                    rtBox.Select(rtBox.Text.Length, 0);
                }
            }
        }

        private void rtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Down)
            {
                SetToListBoxElement();
            SuggestionBox.Focus();
            }
        }

        void SetToListBoxElement()
        {

            SuggestionBox.Focus();
            SuggestionBox.SelectedIndex = 0;
        

        }

        private void rtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)40)
            {


                SuggestionBox.Focus();
                SuggestionBox.SelectedIndex = 0;
            
            }
        }
    }
}
