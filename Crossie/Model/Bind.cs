using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    class Bind
    {
        public FileHeader fileHeader { get; set; }
        public Vector<SolutionSet> solutionSetList { get; set; }
        public Vector<WordsSolution> wordList { get; set; }
        public Bind()
        {
        }
        public Bind(FileHeader fileHeader,Vector <WordsSolution> wordList, 
            Vector<SolutionSet> solutionSetList)
        {
            this.fileHeader = fileHeader;
            this.solutionSetList = solutionSetList;
            this.wordList = wordList;
        }

    }

}
