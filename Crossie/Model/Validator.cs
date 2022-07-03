using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crossie.Model
{
    class Validator
    {
        private FileHeader defaultHeader { get; set; }
        public FileHeader fileHeader { get; set; }
        public Vector<WordsSolution> wordsSolution { get; set; }
        public Vector<SolutionSet> solutionSet { get; set; }

        private const string path = "../../Error log file/Error log file.txt";
        private static int fileHeaderMistakes = 0;
        private static int wordListMistakes = 0;
        private static int solutionSetMistakes = 0;
        private static int configurationMistakes = 0;
        private static bool fatalException = false;
        public Validator() { }
        public Validator(ref FileHeader fileHeader, ref Vector<WordsSolution> wordsSolution,
                         ref Vector<SolutionSet> solutionSet)
        {
            this.fileHeader = fileHeader;
            this.wordsSolution = wordsSolution;
            this.solutionSet = solutionSet;

            InitiateValidation();
        }


        private void InitiateValidation()
        {
            bool nameListCount = false;
            bool columnCheck = false;
            bool checkWordSize = false;

            nameListCount = CheckSolutionNameCount();
            columnCheck = CheckColumnAllocation();
            checkWordSize = CheckWordSize();

        }

        private FileHeader DefaultHeader()
        {
            defaultHeader = new FileHeader();
            defaultHeader.difficultyLevel = "EASY";
            defaultHeader.numberOfWords = 30;
            defaultHeader.numberOfRows = 20;
            defaultHeader.numberOfColumns = 20;
            defaultHeader.numberOfHorizontals = 7;
            defaultHeader.numberOfVerticals = 7;

            return defaultHeader;
        }
        public string[] CheckFileHeader(string line)
        {
            FileHeader defaultHeader = new FileHeader();

            string[] temp = line.Split(',');
            int number = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (!(temp[i].Contains("EASY") || temp[i].Contains("MEDIUM") || temp[i].Contains("HARD")))
                        {
                            DataSerializer<string>.SaveVectorToTextFile(path, "File header error, incorrect difficulty level format: " + "Supplied " + temp[i]);
                            defaultHeader = DefaultHeader();
                            temp[i] = defaultHeader.difficultyLevel;
                            fileHeaderMistakes++;
                        }
                        break;
                    case 1:
                        if (!(Int32.TryParse(temp[i], out number)))
                        {
                            DataSerializer<string>.SaveVectorToTextFile(path, "File header error, incorrect number of words format: " + "Supplied " + temp[i]);
                            temp[i] = Convert.ToString(defaultHeader.numberOfWords);
                            fileHeaderMistakes++;
                        }    
                        break;
                    case 2:
                        if (!(Int32.TryParse(temp[i], out number)))
                        {
                            DataSerializer<string>.SaveVectorToTextFile(path, "File header error, incorrect number of rows format: " + "Supplied " + temp[i]);
                            temp[i] = Convert.ToString(defaultHeader.numberOfRows);
                            fileHeaderMistakes++;
                        }
                        break;
                    case 3:
                        if (!(Int32.TryParse(temp[i], out number)))
                        {
                            DataSerializer<string>.SaveVectorToTextFile(path, "File header error, incorrect number of columns format: " + "Supplied " + temp[i]);
                            temp[i] = Convert.ToString(defaultHeader.numberOfColumns);
                            fileHeaderMistakes++;
                        }  
                        break;
                    case 4:
                        if (!(Int32.TryParse(temp[i], out number)))

                        {
                            DataSerializer<string>.SaveVectorToTextFile(path, "File header error, incorrect number of horizontal format: " + "Supplied " + temp[i]);
                            temp[i] = Convert.ToString(defaultHeader.numberOfHorizontals);
                            fileHeaderMistakes++;
                        }
                        break;
                    case 5:
                        if (!(Int32.TryParse(temp[i], out number)))
                        {
                            DataSerializer<string>.SaveVectorToTextFile(path, "File header error, incorrect number of vertical format: " + "Supplied " + temp[i]);
                            temp[i] = Convert.ToString(defaultHeader.numberOfVerticals);
                            fileHeaderMistakes++;
                        }
                        break;
                }
            }
            return temp;
        }

        public string[] CheckWordSolution(string line)
        {
            char[] delimiter = { ','};
            string[] temp = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string pattern = "^[a-zA-Z]+$";
            List<int> errorIndex = new List<int>();

            for (int i = 0; i < temp.Length; i++)
            {
                if (!(System.Text.RegularExpressions.Regex.IsMatch(temp[i], pattern)))
                {
                    DataSerializer<string>.SaveVectorToTextFile(path, "Word list error, incorrect word format: " + "Supplied " + temp[i]);
                    temp[i] = "-1";
                    wordListMistakes++;
                }
            }
            ;
            return temp;
        }

        public string[] CheckSolutionSet(string line)
        {
            string path = "../../Error log file/Error log file.txt";
            string[] temp = line.Split(',');
            string pattern = "^[A-Za-z]+$";
            int number = 0;

            try
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (!(temp[i].Equals("HORIZONTAL") || temp[i].Equals("VERTICAL")))
                            {
                                DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect ORIENTATION format: " + "Supplied " + temp[i]);
                                solutionSetMistakes++;
                            }
                            break;

                        case 1:
                            if (!(Int32.TryParse(temp[i], out number)))
                            {
                                DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect ROW POSITION format: " + "Supplied " + temp[i]);
                                fatalException = true;
                                solutionSetMistakes++;
                            }
                                
                            break;
                        case 2:
                            if (!(Int32.TryParse(temp[i], out number)))
                            {
                                DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect COLUMN POSITION format: " + "Supplied " + temp[i]);
                                fatalException = true;
                                solutionSetMistakes++;
                            }
                            break;
                        case 3:
                            if (!(System.Text.RegularExpressions.Regex.IsMatch(temp[i], pattern)))
                            {
                                DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect NAMEXx format: " + "Supplied " + temp[i]);
                                fatalException = true;
                                solutionSetMistakes++;
                            }
                            break;
                    }
                }
                //if (fatalException)
                    //throw new ExceptionHandeling();
            }catch(ExceptionHandeling e)
            {
                MessageBox.Show(e.Message);
            }

            return temp;
        }

        private bool CheckSolutionNameCount()
        { 
            if(fileHeader.numberOfWords > 0)
                if (wordsSolution.Count != fileHeader.numberOfWords)
                {
                    DataSerializer<string>.SaveVectorToTextFile(path, "Word solution error, number exceeds max word limit: " + "Supplied " + wordsSolution.Count);
                    wordListMistakes++;
                    return false;
                }
            return true; 
        }

        private bool CheckSolutionNameDuplicates()
        {
            List<string> copy = new List<string>();
            for (int i = 0; i < solutionSet.Count; i++)
            {
                for (int j = 0; j < solutionSet.Count; j++)
                {
                    if(solutionSet[i].Name.Equals(solutionSet[j].Name))
                    {
                        DataSerializer<string>.SaveVectorToTextFile(path, "Word list error, incorrect max number format: " + "Supplied " + solutionSet[i].Name);
                        wordListMistakes++;
                    }   
                }
            }
                if (wordListMistakes > 0)
                return false;

            return true;
        }

        public bool CheckColumnAllocation()
        {
            try
            {
                for (int i = 0; i < solutionSet.Count; i++)
                {
                    if (solutionSet[i].ColumnPosition > fileHeader.numberOfColumns)
                    {
                        DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect COLUMN POSITION format: " + "Supplied " + solutionSet[i]);
                        solutionSetMistakes++;
                        solutionSet.RemoveAt(i);
                    }
                }
            }
            catch (ExceptionHandeling e)
            {

            }
            return true;
        }

        public bool CheckRowAllocation()
        {
            for (int i = 0; i < solutionSet.Count; i++)
            {
                if (solutionSet[i].Name.Length > fileHeader.numberOfRows - solutionSet[i].RowPosition)
                {
                    DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect max number format: " + "Supplied " + solutionSet[i].RowPosition);
                    solutionSet.RemoveAt(i);
                    solutionSetMistakes++;
                }

            }
            return true;

        }

        public bool CheckWordSize()
        {
            for (int i = 0; i < solutionSet.Count; i++)
            {
                if (solutionSet[i].Orientation.Equals("HORIZONTAL"))
                {
                    if (solutionSet[i].Name.Length > fileHeader.numberOfColumns)
                    {
                        DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect max number format: " + "Supplied " + solutionSet[i].Name);
                        solutionSet.RemoveAt(i);
                        solutionSetMistakes++;
                    }

                }
                if (solutionSet[i].Orientation.Equals("VERTICAL"))
                {
                    if (solutionSet[i].Name.Length > fileHeader.numberOfRows)
                    {
                        DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect max number format: " + "Supplied " + solutionSet[i].Name);
                        solutionSet.RemoveAt(i);
                        solutionSetMistakes++;
                    }
                }
            }
            return false;
        }

        //Checks limit of group max in config
        public string[] CheckLimit(string line)
        {
            int number = 0;
            string[] temp = line.Split(':', '=');
            if (!(Int32.TryParse(temp[1], out number)))
            {
                    DataSerializer<string>.SaveVectorToTextFile(path, "Configuration error, incorrect max number format: " + "Supplied " + temp[1]);
                    temp[1] = "1000";
                    solutionSetMistakes++;
             }
            return temp;       
        }

        //checks config word point
        public string[] CheckWordPoint(string line)
        {
            int number = 0;
            string[] temp = line.Split(':', '=');
            if (!(Int32.TryParse(temp[1], out number)))
            {
                 DataSerializer<string>.SaveVectorToTextFile(path, "Configuration error, incorrect word point format: " + "Supplied " + temp[2]);
                 solutionSetMistakes++;
                 temp[1] = "1";
            }
            return temp;
        }

        public string[] CheckPointConfig(string line)
        {
            int number = 0;
            string pattern = "^[a-zA-Z]+$";

            string[] temp = line.Split(':', '=');
            try
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (!(temp[i].Equals("INTERSECTING") || temp[i].Equals("NONINTERSECTING")))
                            {
                                DataSerializer<string>.SaveVectorToTextFile(path, "Config error, incorrect COMMAND format: " + "Supplied " + temp[i]);
                                temp[i] = "INTERSECTING";
                                solutionSetMistakes++;
                            }
                            break;
                        case 1:
                            if (!(System.Text.RegularExpressions.Regex.IsMatch(temp[i], pattern)))
                            {
                                DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect NAME format: " + "Supplied " + temp[i]);
                                temp[i] = " ";
                                fatalException = true;
                                solutionSetMistakes++;
                            }
                            break;
                        case 2:
                            if (!(Int32.TryParse(temp[i], out number)))
                            {
                                DataSerializer<string>.SaveVectorToTextFile(path, "Solution set error, incorrect COLUMN POSITION format: " + "Supplied " + temp[i]);
                                temp[i] = "1";
                                fatalException = true;
                                solutionSetMistakes++;
                            }
                            break;
                    }
                }
            }
            catch (ExceptionHandeling e)
            {
                MessageBox.Show(e.Message);
            }
            return temp;
        }

        public int GetFileHeaderMistakes
        {
            get { return fileHeaderMistakes; }
        }

        public int GetWordListMistakes
        {
            get { return wordListMistakes; }
        }
        public int GetSolutionSetMistakes
        {
            get { return solutionSetMistakes; }
        }

        public bool GetFatalError
        {
            get { return fatalException; }
        }

        public int GetTotalConfigurationMistakes
        {
           get { return configurationMistakes; }
        }

    }
}
