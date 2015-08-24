using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AI_Project
{
    class Grammar
    {
            string word;
        static string[] WordList;
        Words[] w1;
        public Grammar()
        {

            InitializeWordsList();
            InitializeWordsWithDB();
        
        }

        void InitializeWordsWithDB()
        {





            try
            {

                SqlConnection SQLCon = new SqlConnection();
                SqlDataAdapter SQLAdp = null;
                DataSet SQLdset = null;
                string ConString = @"Server=.\SQLExpress;" + @"AttachDbFilename=C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\PartsOfSpeech.mdf;" + "Database=PartsOfSpeech;" + "Trusted_Connection=Yes;";
                SQLCon.ConnectionString = ConString;










                // from db to array

                if (SQLCon != null)
                {

                    SQLCon.Open();

                    string Comm = "SELECT * FROM PoS";

                    SQLAdp = new SqlDataAdapter(Comm, ConString);

                    SQLAdp.SelectCommand = new SqlCommand(Comm, SQLCon);

                    SQLdset = new DataSet();

                    SQLAdp.Fill(SQLdset, "PoS");




                    SQLCon.Close();

                    if (SQLdset != null)
                    {
                        ;
                        int j = SQLdset.Tables["PoS"].Rows.Count;
                       
                        ;
                        w1 = new Words[j];
                        int i = 0;


                        for (i = 0; i < j - 1; i++)
                        {
                            try
                            {
                              w1[i].word = SQLdset.Tables["PoS"].Rows[i]["word"].ToString();

                              w1[i].property1 = SQLdset.Tables["PoS"].Rows[i]["P1"].ToString();

                              w1[i].property2 = SQLdset.Tables["PoS"].Rows[i]["P2"].ToString();

                              w1[i].property3 = SQLdset.Tables["PoS"].Rows[i]["P3"].ToString();

                            }
                            catch (Exception exc)
                            { 
                            
                            
                            }
                        }
                    }
                }












            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }



        
        
        }


        public bool CheckSentence1(string[] WordsFromSentence)
        {
        bool Correct=false;
           bool c1=false;
            bool c2=false;
            bool c3 = false;
            bool c4 = false;
            int i=0;
            int j = 0;

            for (i = 0; i < w1.Length; i++)
            {

                if (WordsFromSentence[j] == w1[i].word)
                {


                    break;
                
                }
            
            
            
            }
            string p1 = null;
            string p2 = null;
            string p3 = null;
                
            try
            {
                 p1 = w1[i].property1;
                 p2 = w1[i].property2;
                 p3 = w1[i].property3;
                
            
            
            }
            catch(Exception ex)
            {}

            if(p1=="noun" || p2=="pronoun")
                c1=true;

            j++;
            if (WordsFromSentence[j] == "do" || WordsFromSentence[j] == "does")
                c2 = true;

            j++;
            if (WordsFromSentence[j] == "not" )
                c3 = true;

            j++;
            for (i = 0; i < w1.Length; i++)
            {

                if (WordsFromSentence[j] == w1[i].word)
                {


                    break;

                }



            }
           

            try
            {
                p1 = w1[i].property1;
                p2 = w1[i].property2;
                p3 = w1[i].property3;



            }
            catch (Exception ex)
            { }

            if (p1 == "verb (transitive)" || p2 == "verb (transitive)" || p1 == "verb (participle)" || p2 == "verb (participle)" || p1 == "verb (intransitive)" || p2 == "verb (intransitive)")
                c4 = true;

            if (c1 && c2 && c3 && c4)
                Correct = true;

                return Correct;
        }

        public bool CheckSentence2(string[] WordsFromSentence)
        {
            bool Correct = false;
            bool c1 = false;
            bool c2 = false;

            int i = 0;
            int j = 0;

            for (i = 0; i < w1.Length; i++)
            {

                if (WordsFromSentence[j] == w1[i].word)
                {


                    break;

                }



            }
            string p1 = null;
            string p2 = null;
            string p3 = null;

            try
            {
                p1 = w1[i].property1;
                p2 = w1[i].property2;
                p3 = w1[i].property3;



            }
            catch (Exception ex)
            { }

            if (p1 == "noun" || p2 == "pronoun")
                c1 = true;

            j++;
            for (i = 0; i < w1.Length; i++)
            {

                if (WordsFromSentence[j] == w1[i].word)
                {


                    break;

                }



            }


            try
            {
                p1 = w1[i].property1;
                p2 = w1[i].property2;
                p3 = w1[i].property3;



            }
            catch (Exception ex)
            { }

            if (p1 == "verb (transitive)" || p2 == "verb (transitive)" || p1 == "verb (participle)" || p2 == "verb (participle)" || p1 == "verb (intransitive)" || p2 == "verb (intransitive)")
                c2 = true;

            if (c1 && c2)
                Correct = true;

            return Correct;
        }




        public string[] GetCurrentSentenceAndWord(string alltext, int length)
        {
            List<string> initialWords = new List<string>(); ;
            
            string text = null;
            text = alltext;
            
            
            
            int i;

            char[] temp1 = text.ToCharArray();
            char[] current=new char[text.Length];

            bool firstloop = false;



            for (i = text.Length-1; i >=0; i-- )
            {
                if (firstloop && temp1[i] == '.')
                    break;



                if (temp1[i] == ' ' || i==0)
                {
                    current = new char[text.Length];
                    int j = i+1;
                    if (i == 0)
                        j--;
                    int k = 0;
                    while (true)
                    {

                        current[k] = temp1[j];
                        k++;
                        j++;

                        try
                        {
                            if (temp1[j] == ' ' || temp1[j] == '.' || j==text.Length)
                                break;
                        }
                        catch (IndexOutOfRangeException ex)
                        { 
                        
                        }

                    }

                    char[] finalword = new char[k];

                            
                             string Word = string.Join("",current);
                             Word = Word.Trim();
                             initialWords.Add(Word);
                             ; ;
                
                }



                firstloop = true;
            }


            string[] WordsFromCurrentSentence = initialWords.ToArray();
            Array.Reverse(WordsFromCurrentSentence);

            return WordsFromCurrentSentence;

        }





         public string CheckSpell(string word)
        {
          
             word = word.ToLower();
            char[] charsToTrim = { ',', '.', '!',' ' };
            word = word.Trim(charsToTrim);
            bool Correct = false;

          
         
            try
            {












                int j=WordList.Length;

                        for (int i = 0; i < j; i++)
                        {

                            if (word == WordList[i])
                            {

                                Correct = true;
                                break;
                            }
                        }
                   
                
                
                






            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            
            }
            string msg = null;

            if (Correct)
                msg = "correct";
            else
                msg = "incorrect";

            return msg;
        }





         public void InitializeWordsList()
         {
             
             
             
            
                       
             char[] charsToTrim = { ',', '.', '!',' ' };
           

             int i = 0;


             try
             {

                 SqlConnection SQLCon = new SqlConnection();
                 SqlDataAdapter SQLAdp = null;
                 DataSet SQLdset = null;
                 string ConString = @"Server=.\SQLExpress;" + @"AttachDbFilename=C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\Thesaurus.mdf;" + "Database=Thesaurus;" + "Trusted_Connection=Yes;";
                 SQLCon.ConnectionString = ConString;










                 // from db to array

                 if (SQLCon != null)
                 {

                     SQLCon.Open();

                     string Comm = "SELECT * FROM Thesaurus";

                     SQLAdp = new SqlDataAdapter(Comm, ConString);

                     SQLAdp.SelectCommand = new SqlCommand(Comm, SQLCon);

                     SQLdset = new DataSet();

                     SQLAdp.Fill(SQLdset, "Thesaurus");




                     SQLCon.Close();

                     if (SQLdset != null)
                     {
                         ;
                         int j = SQLdset.Tables["Thesaurus"].Rows.Count;
                         WordList = new string[j - 1];
                         ;

                         for (i = 0; i < j-1; i++)
                         {

                            WordList[i]= SQLdset.Tables["Thesaurus"].Rows[i]["words"].ToString();
                            WordList[i] = WordList[i].ToLower();
                            WordList[i] = WordList[i].Trim(charsToTrim);
                         }
                     }
                 }


               




                




             }
             catch (Exception ex)
             {
                 System.Diagnostics.Debug.WriteLine(ex.Message);

             }

                
         
         
         }


         public int PredictionIndex(string alltext, int length)
         {

             string text = null;
             text = alltext;



             char[] current = new char[length];
             char[] temp = text.ToCharArray();
             string s = string.Join("", temp);


             int k = (s.Length);   // q k space ko count nhe karna




             int j = 0;
             bool firstLoop = true;

             for (j = 0; j <= s.Length; j++)
             {
                 k--;


                 if (k < 0)
                     break;
                 current[j] = temp[k];

                 if (!(firstLoop) && temp[k] == ' ')
                     break;

                 firstLoop = false;

             }

             k = k + 1;
             return k;




         }

         public string[] PredictFromWord(string CurrentWord)
         {

             bool ExceptionCaught = false;

             List<string> SuggestionList = new List<string>();

             int len = CurrentWord.Length;
             char[] temp2 = CurrentWord.ToArray();


             char[] temp1;
             

             int i = 0;
             int j = WordList.Length;


             



                 for (i = 0; i < j; i++)
                 {

                     temp1 = WordList[i].ToArray();

                     if (temp1.Length > temp2.Length)
                     {
                         try
                         {
                             if (len >= 8)
                             {

                                 if (temp1[0] == temp2[0] && temp1[1] == temp2[1] && temp1[2] == temp2[2] && temp1[3] == temp2[3] && temp1[4] == temp2[4] && temp1[5] == temp2[5] && temp1[6] == temp2[6] && temp1[7]==temp2[7])
                                 {
                                     if (WordList[i].Length >= word.Length)
                                         SuggestionList.Add(WordList[i]);

                                 }
                                 
                             }

                             else if (len >= 7)
                             {

                                 if (temp1[0] == temp2[0] && temp1[1] == temp2[1] && temp1[2] == temp2[2] && temp1[3] == temp2[3] && temp1[4] == temp2[4] && temp1[5] == temp2[5] && temp1[6] == temp2[6])
                                 {
                                     if (WordList[i].Length >= word.Length)
                                         SuggestionList.Add(WordList[i]);

                                 }
                             }
                             else if (len >= 6)
                             {

                                 if (temp1[0] == temp2[0] && temp1[1] == temp2[1] && temp1[2] == temp2[2] && temp1[3] == temp2[3] && temp1[4] == temp2[4] && temp1[5] == temp2[5])
                                 {
                                     if (WordList[i].Length >= word.Length)
                                         SuggestionList.Add(WordList[i]);

                                 }
                             }

                             else if (len >= 5)
                             {

                                 if (temp1[0] == temp2[0] && temp1[1] == temp2[1] && temp1[2] == temp2[2] && temp1[3] == temp2[3] && temp1[4] == temp2[4])
                                 {
                                     if (WordList[i].Length >= word.Length)
                                         SuggestionList.Add(WordList[i]);

                                 }
                             }


                             else if (len >= 4)
                             {

                                 if (temp1[0] == temp2[0] && temp1[1] == temp2[1] && temp1[2] == temp2[2] && temp1[3] == temp2[3])
                                 {
                                     if (WordList[i].Length >= word.Length)
                                         SuggestionList.Add(WordList[i]);

                                 }

                             }

                             else if (len >= 3)
                             {

                                 if (temp1[0] == temp2[0] && temp1[1] == temp2[1] && temp1[2] == temp2[2])
                                 {
                                     if (WordList[i].Length >= word.Length)
                                         SuggestionList.Add(WordList[i]);

                                 }

                             }

                             else if (len >= 2)
                             {

                                 if (temp1[0] == temp2[0] && temp1[1] == temp2[1])
                                 {
                                     if (WordList[i].Length >= word.Length)
                                         SuggestionList.Add(WordList[i]);

                                 }


                             }

                             else if (len == 1)
                             {


                                 if (temp1[0] == temp2[0] )
                                 {
                                     if (WordList[i].Length <= 2)
                                         SuggestionList.Add(WordList[i]);

                                 }


                             }

                         }
                          catch (IndexOutOfRangeException exc)
                         {

                             System.Diagnostics.Debug.WriteLine(exc.Message);

                             ExceptionCaught = true;
                         }


                     }
                 
                 }












                 if (!ExceptionCaught)
                 {
                     string[] SuggestedList = new string[SuggestionList.Count];

                     SuggestedList = SuggestionList.ToArray();
                     return SuggestedList;
                 }
                 else
                 {
                     string[] ExceptionWasCaught = new string[1];
                     ExceptionWasCaught[0] = "No Suggestions";
                     return ExceptionWasCaught;
                 
                 }
             
         
         }
    
    
    
    }
}
