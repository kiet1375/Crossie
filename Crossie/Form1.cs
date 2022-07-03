using Crossie.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crossie
{
    public partial class Form1 : Form
    {
        private FileHeader fileHeader;
        private Configuration configuration;
        private Vector<PointsConfiguration> pointsConfiguration;
        private Vector<WordsSolution> wordSolution;
        private Vector<SolutionSet> solutionSet;
        private Vector<Vector4Horizontal> letterPositionHorizontalList;
        private Vector<Vector4Vertical> letterPositionVerticalList;
        private Vector4Horizontal letterPositionHorizontal;
        private Vector4Vertical letterPositionVertical;
        private static string InputFile = "";
        private static string ConfigFile = "";
        private int totalscore = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void CheckStatus(string crozzieFile, string configFile)
        {
            InputFile = crozzieFile;
            ConfigFile = configFile;
            if (dataGridView1.Rows.Count > 0)
            {
                fileHeader = null;
                configuration = null;
                pointsConfiguration = null;
                wordSolution = null;
                solutionSet = null;
                letterPositionHorizontalList = null;
                letterPositionVerticalList = null;
                letterPositionHorizontal = null;
                letterPositionVertical = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                totalscore = 0;
            }


            if (!(InputFile.Equals("")))
            {
                if (!(ConfigFile.Equals("")))
                {
                    loadData(InputFile, ConfigFile);
                    InitializeBoard();
                    BeginSolution();
                    CalculateTotalScore();
                    DisplayView();
                }
            }
        }

        public void loadData(string crozzieFile, string configFile)
        {
            string inputFileName = "../../App_Data/" + crozzieFile;
            string configPath = "../../App_Data/" + configFile;

            DataSerializer<string>.LoadVectorFromTextFile(inputFileName, ref fileHeader, ref wordSolution, ref solutionSet);
            DataSerializer<string>.LoadVectorFromTextFile(configPath, ref pointsConfiguration, ref configuration);
            Validator validator = new Validator(ref fileHeader, ref wordSolution, ref solutionSet);
        }

        private void exitToolStripMenuItem_Click()
        {
            this.Close();
        }

        private void DisplayView()
        {
            Validator validator = new Validator();
            String html = null;
            int rowHeader = 1;
            int a = 0;
            //create html & table


            html = @"<!DOCTYPE html>
                                <html>
                                <head>
                                <style>
                                html, body{
                                    width: 100%;
                                    height: 100%;
                                }
                                
                                table {
                                    width:50%;
                                    height: 50%
                                }
                                table, td, th {
                                    border: 1px solid black;
                                    border-collapse: collapse;
                                }
                                th{
                                    background-color: #c7c7c7;
                                }
                                td {
                                    width:24px;
                                    text-align: center;
                                }
                                h1{
                                    font-size: 50px;
                                    color: #088da5;
                                }
                                h3{
                                    color: red;
                                }
                                </style>
                                <title>Crossie</title>
                                </head>";
            html += @"<body bgcolor= '#E6E6FA'> <center> <h1>Crossie</h1> <table>";


            //creating table header
            for (int i = 0; i <= dataGridView1.Columns.Count; i++)
            {
                html += @"<th align='center' valign='middle'>" +
                                i + "</th>";
            }


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                html += @"<tr>";
                html += @"<th align='center' valign='middle'>" + rowHeader++ + "</th>";

                foreach (DataGridViewCell x in dataGridView1.Rows[i].Cells)
                {
                    if (x.Value != null)
                    {
                        html += @"<style>td{background-color:#a6ff9b}</style>";
                        html += @"<td align='center' valign='middle'>" + x.Value + "</td>";
                    }

                    else
                    {
                        html += @"<style>td{background-color: #9ba6ff}</style>";
                        html += @"<td align='center' valign='middle'>" + "</td>";
                    }

                }
                html += @"</tr>";
            }
            a = validator.GetFileHeaderMistakes + validator.GetTotalConfigurationMistakes + validator.GetWordListMistakes + validator.GetSolutionSetMistakes;
            int b = configuration.PointsPerWord;
            int d = solutionSet.Count;
            d = b * d + totalscore;

            //table footer & end of html file
            html += @"</table></center>" + "<h3>Total errors found: " + a.ToString() +"<br />" + "View error in log file" + "</h3>" + "<h3 style= color:green;>" + "Total score: " + d + "</h3>" + "</body></html>";
            webBrowser1.DocumentText = html;
        }



        private void InitializeBoard()
        {
            dataGridView1.BackgroundColor = Color.Aqua;
            dataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < fileHeader.numberOfColumns; i++)
                dataGridView1.Columns.Add(i.ToString(), null);

            dataGridView1.RowCount = fileHeader.numberOfRows;

            foreach (DataGridViewColumn x in dataGridView1.Columns)
                x.Width = 30;

            foreach (DataGridViewRow x in dataGridView1.Rows)
                x.Height = 30;
        }

        private void BeginSolution()
        {
            try
            {
                Validator validator = new Validator();
                if (validator.GetFatalError)
                    throw new ExceptionHandeling();
                int c = 0;
                letterPositionHorizontalList = new Vector<Vector4Horizontal>();
                letterPositionVerticalList = new Vector<Vector4Vertical>();

                foreach (SolutionSet x in solutionSet)
                {
                    if (c == solutionSet.Count)
                        break;
                    var temp = x.Name.ToCharArray();

                    Add(temp, x.Orientation, x.RowPosition, x.ColumnPosition);
                    c++;
                }
            }
            catch(ExceptionHandeling e)
            {
                MessageBox.Show(e.Message);
            }


        }

        private void Add(char[] temp, string orientation, int row, int column)
        {
            row = row - 1;
            column = column - 1;

            if (orientation.Equals("HORIZONTAL"))
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    letterPositionHorizontal = new Vector4Horizontal();
                    dataGridView1.Rows[row].DefaultCellStyle.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    dataGridView1.Rows[row].Cells[column].Value = temp[i];
                    letterPositionHorizontal.Letter = temp[i];
                    letterPositionHorizontal.Row = row;
                    letterPositionHorizontal.Column = column++;
                    letterPositionHorizontal.Orientation = orientation;
                    letterPositionHorizontalList.Add(letterPositionHorizontal);
                }
            }


            if (orientation.Equals("VERTICAL"))
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    letterPositionVertical = new Vector4Vertical();
                    dataGridView1.Rows[row].DefaultCellStyle.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    dataGridView1.Rows[row].Cells[column].Value = temp[i];
                    letterPositionVertical.Letter = temp[i];
                    letterPositionVertical.Row = row++;
                    letterPositionVertical.Column = column;
                    letterPositionVertical.Orientation = orientation;
                    letterPositionVerticalList.Add(letterPositionVertical);
                }
            }
        }

        private void CalculateTotalScore()
        {
            if (fileHeader.difficultyLevel.Equals("EASY"))
                CalculateEasyScore();
            if (fileHeader.difficultyLevel.Equals("MEDIUM"))
                CalculateMediumScore();
            if (fileHeader.difficultyLevel.Equals("HARD"))
                CalculateHardScore();
        }

        private void CalculateEasyScore()
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                foreach (DataGridViewCell x in dataGridView1.Rows[i].Cells)
                    if (x.Value != null)
                        totalscore++;
        }

        private void CalculateMediumScore()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                foreach (DataGridViewCell x in dataGridView1.Rows[i].Cells)
                    if (x.Value != null)
                        GetLetterPoint(x.Value.ToString());
        }

        private void CalculateHardScore()
        {
            for (int i = 0; i < letterPositionVerticalList.Count; i++)
            {
                for (int j = 0; j < letterPositionHorizontalList.Count; j++)
                {
                    if (letterPositionVerticalList[i].Row.Equals(letterPositionHorizontalList[j].Row) &&
                        letterPositionVerticalList[i].Column.Equals(letterPositionHorizontalList[j].Column))
                    {
                        GetLetterPoint(letterPositionHorizontalList[j].Letter.ToString());
                    }

                }
            }
        }

        private void GetLetterPoint(string value)
        {
            int count = 0;
            char letter = Convert.ToChar(value);

            foreach (PointsConfiguration x in pointsConfiguration)
            {
                count++;
                if (count == pointsConfiguration.Count)
                    break;
                if (x.Letter.Equals(letter) && x.Orientation.Equals("INTERSECTING"))
                {
                    totalscore += x.Point;
                    break;
                }
                if (x.Letter.Equals(letter) && x.Orientation.Equals("NONINTERSECTING"))
                {
                    totalscore += x.Point;
                    break;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                InputFile = openFileDialog1.FileName;
            }
            if (!(InputFile.Equals("")))
            {
                string[] temp = InputFile.Split(new char[] { '/', '\\' });
                if(temp.Length == 1)
                    InputFile = temp[0];
                else
                    InputFile = temp[temp.Length-1];
            }
            CheckStatus(InputFile, ConfigFile);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ConfigFile = openFileDialog1.FileName;
            }
            if (!(ConfigFile.Equals("")))
            {
                string[] temp = ConfigFile.Split(new char[] { '/', '\\' });
                if (temp.Length == 1)
                    ConfigFile = temp[0];
                else
                    ConfigFile = temp[temp.Length-1];
            }
            CheckStatus(InputFile, ConfigFile);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string path = "../../Error log file/Error log file.txt";
            button1.Visible = true;
            richTextBox1.Visible = true;
            richTextBox1.LoadFile(path, RichTextBoxStreamType.PlainText);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Visible = false;
            button1.Visible = false;
        }
    }
}
