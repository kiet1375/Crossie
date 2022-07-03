using Crossie.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    [Serializable]
    public class FileHeader : ISerializable, IComparer
    {
        public string difficultyLevel { get; set; }
        public int numberOfWords { get; set; } 
        public int numberOfRows { get; set; }
        public int numberOfColumns { get; set; }
        public int numberOfHorizontals { get; set; }
        public int numberOfVerticals { get; set; }

        public FileHeader()
        {
        }
        public FileHeader(string difficultyLevel, int numberOfWords, int numberOfRows, int numberOfColumns,
                          int numberOfHorizontal, int numberOfVerticals)
        {
            this.difficultyLevel = difficultyLevel;
            this.numberOfWords = numberOfWords;
            this.numberOfRows = numberOfRows;
            this.numberOfColumns = numberOfColumns;
            this.numberOfHorizontals = numberOfHorizontals;
            this.numberOfVerticals = numberOfVerticals;
        }

        public override string ToString()
        {
            return "Difficulty level: " + difficultyLevel + "\t\t" + "Number of words: " + numberOfWords + "\n" +
                   "Number of rows: " + numberOfRows + "\t\t" + "Number of columns: " + numberOfColumns + "\n" +
                   "Number of horizontals: " + numberOfHorizontals + "\t\t" + "Number of verticals: " + numberOfVerticals;
        }

        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("difficultyLevel", difficultyLevel, typeof(string));
            info.AddValue("numberOfWords", numberOfWords, typeof(int));
            info.AddValue("numberOfRows", numberOfRows, typeof(int));
            info.AddValue("numberOfColumns", numberOfColumns, typeof(int));
            info.AddValue("numberOfHorizontals", numberOfHorizontals, typeof(int));
            info.AddValue("numberOfVerticals", numberOfVerticals, typeof(int));
        }

        // The special constructor is used to deserialize values.
        public FileHeader(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            difficultyLevel = (string)info.GetValue("difficultyLevel", typeof(string));
            numberOfWords = (int)info.GetValue("numberOfWords", typeof(int));
            numberOfRows = (int)info.GetValue("numberOfRows", typeof(int));
            numberOfColumns = (int)info.GetValue("numberOfColumns", typeof(int));
            numberOfHorizontals = (int)info.GetValue("numberOfHorizontals", typeof(int));
            numberOfVerticals = (int)info.GetValue("numberOfVerticals", typeof(int));
        }
    }
}
