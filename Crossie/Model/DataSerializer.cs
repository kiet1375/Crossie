using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crossie.Model
{
    public class DataSerializer <T> where T : IConvertible
    {
        public static void Serialise(string path, Vector<T> vector)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, vector);
            }
        }
        public void deserialise(string path, ref Vector<T> vector)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                vector = (Vector<T>)bin.Deserialize(stream);

            }
        }

        internal static void LoadVectorFromTextFile(string path, ref FileHeader fileHeader, ref Vector<WordsSolution> wordSolutionList, ref Vector<SolutionSet> solutionSetList)
        {
            char[] delimiter = { ',' };
            fileHeader = new FileHeader();
            WordsSolution wordSolution = null;
            wordSolutionList = new Vector<WordsSolution>();
            SolutionSet solutionSet = null;
            solutionSetList = new Vector<SolutionSet>();
            Validator validator = null;
            string[] temp = null;
            string line = " ";
            int next = 0;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    validator = new Validator();
                    while ((line = sr.ReadLine()) != null)
                    {
                        next++;

                        if (next == 1)
                        {
                            temp = validator.CheckFileHeader(line);

                            for (int iterator = 0; iterator < temp.Length; iterator++)
                            {
                                switch (iterator)
                                {
                                    case (int)FileHeaderEnum.DIFFICULTY_LEVEL:
                                        fileHeader.difficultyLevel = temp[(int)FileHeaderEnum.DIFFICULTY_LEVEL];
                                        break;
                                    case (int)FileHeaderEnum.NUMBER_OF_WORDS:
                                        fileHeader.numberOfWords = Convert.ToInt32(temp[(int)FileHeaderEnum.NUMBER_OF_WORDS]);
                                        break;
                                    case (int)FileHeaderEnum.NUMBER_OF_ROWS:
                                        fileHeader.numberOfRows = Convert.ToInt32(temp[(int)FileHeaderEnum.NUMBER_OF_ROWS]);
                                        break;
                                    case (int)FileHeaderEnum.NUMBER_OF_COLUMNS:
                                        fileHeader.numberOfColumns = Convert.ToInt32(temp[(int)FileHeaderEnum.NUMBER_OF_COLUMNS]);
                                        break;
                                    case (int)FileHeaderEnum.NUMBER_OF_HORIZONTALS:
                                        fileHeader.numberOfHorizontals = Convert.ToInt32(temp[(int)FileHeaderEnum.NUMBER_OF_HORIZONTALS]);
                                        break;
                                    case (int)FileHeaderEnum.NUMBER_OF_VERTICALS:
                                        fileHeader.numberOfVerticals = Convert.ToInt32(temp[(int)FileHeaderEnum.NUMBER_OF_VERTICALS]);
                                        break;
                                }
                            }
                        }
                        if (next == 2)
                        {
                            temp = null;
                            temp = validator.CheckWordSolution(line);

                            for (int iterator = 0; iterator < temp.Length; iterator++)
                            {
                                wordSolution = new WordsSolution(temp[iterator]);
                                wordSolutionList.Add(wordSolution);
                            }
                        }
                        if (next > 2)
                        {
                            temp = null;
                            temp = validator.CheckSolutionSet(line);
                            if (validator.GetFatalError)
                                throw new ExceptionHandeling();

                            for (int iterator = 0; iterator < temp.Length; iterator++)
                            {
                                switch (iterator)
                                {
                                    case (int)SolutionSetEnum.ORIENTATION:
                                        solutionSet = new SolutionSet();
                                        solutionSet.Orientation = temp[(int)SolutionSetEnum.ORIENTATION];
                                        break;
                                    case (int)SolutionSetEnum.ROW_POSITION:
                                        solutionSet.RowPosition = Convert.ToInt32(temp[(int)SolutionSetEnum.ROW_POSITION]);
                                        break;
                                    case (int)SolutionSetEnum.COLUMN_POSITION:
                                        solutionSet.ColumnPosition = Convert.ToInt32(temp[(int)SolutionSetEnum.COLUMN_POSITION]);
                                        break;
                                    case (int)SolutionSetEnum.NAME:
                                        solutionSet.Name = temp[(int)SolutionSetEnum.NAME];
                                        break;
                                }
                            }
                            solutionSetList.Add(solutionSet);
                        }
                    }
                    sr.Close();
                }
            }
            catch (ExceptionHandeling e)
            {
                e.ExceptionError();
            }
        }

        internal static void LoadVectorFromTextFile(string path, ref Vector<PointsConfiguration> pointsConfigurationList,
                                                    ref Configuration configuration)
        {
            pointsConfigurationList = new Vector<PointsConfiguration>();
            PointsConfiguration pointsConfiguration = null;
            configuration = null;
            Validator validator = null;
            string[] temp = null;
            string line = " ";
            int next = 0;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    validator = new Validator();
                    while ((line = sr.ReadLine()) != null)
                    {
                        next++;

                        if (next == 1)
                        {
                            temp = validator.CheckLimit(line);
                            
                            configuration = new Configuration();

                            for (int iterator = 0; iterator < temp.Length; iterator++)
                            {
                                if (iterator == 1)
                                    configuration.GroupLimit = Convert.ToInt32(temp[iterator]);
                            }
                        }

                        if (next == 2)
                        {
                            temp = validator.CheckWordPoint(line);
                            
                            for (int i = 0; i < temp.Length; i++)
                            {
                                if (i == 1)
                                    configuration.PointsPerWord = Convert.ToInt32(temp[i]);
                            }
                        }

                        if (next > 2)
                        {
                            temp = validator.CheckPointConfig(line);

                            pointsConfiguration = new PointsConfiguration();
                            for (int i = 0; i < temp.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        pointsConfiguration.Orientation = temp[i];
                                        break;
                                    case 1:
                                        pointsConfiguration.Letter = Convert.ToChar(temp[i]);
                                        break;
                                    case 2:
                                        pointsConfiguration.Point = Convert.ToInt32(temp[i]);
                                        break;
                                }
                            }
                                pointsConfigurationList.Add(pointsConfiguration);
                        }
                    }
                    sr.Close();
                }
            }
            catch (ExceptionHandeling e)
            {
                e.ExceptionError();
            }
        }

        public static void LoadVectorFromTextFile(string path, ref Vector<T> vector)
        {
            vector = new Vector<T>();
            string line = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    //This would work only for primitive types
                    vector.Add((T)Convert.ChangeType(line, typeof(T)));
                }
                sr.Close();
            }
        }

        public static void SaveVectorToTextFile(string path, Vector<T> vector)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                var count = vector.Count;
                for (int i = 0; i < count; i++)
                {
                    sw.WriteLine(vector[i]);
                }
                sw.Close();
            }
        }

        public static void SaveVectorToTextFile(string path, string error)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            { 
                sw.WriteLine(error);
                sw.Close();
            }
        }

    }
}
