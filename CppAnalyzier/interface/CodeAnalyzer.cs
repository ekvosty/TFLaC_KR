using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace @interface
{
    internal class CodeAnalyzer
    {
        string pattern = @"(\+)|(\-)|(L)|(l)|(\d)";
        private AnalyzerState AnalyzerState = AnalyzerState.FIRST_SYMBOL;
        private int ColumnCount = 0;
        private int errorCounter = 0;
        private bool hasEndLiteral = false;
        RichTextBox richTextBox;
        private List<char> lexems = new List<char>();


        public CodeAnalyzer(string text, RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
            foreach (char match in text)
            {
                lexems.Add(match);
                ColumnCount++;

                if (AnalyzerState == AnalyzerState.FIRST_SYMBOL)
                {
                    if (match == '+' || match == '-' || char.IsDigit(match)|| match == ' ')
                    {

                    }
                    else if (char.ToLowerInvariant(match) == 'l')
                    {
                        AppendInfo("Число не может начинаться с литерала l");
                        hasEndLiteral = true;
                        errorCounter++;
                    }
                    else
                    {
                        AppendInfo("В числе не может быть что-либо кроме цифр, плюса, минуса или литерала l ");
                        errorCounter++;
                    }
                    AnalyzerState = AnalyzerState.DIGIT;
                }
                else if (AnalyzerState == AnalyzerState.DIGIT)
                {
                    if (char.IsDigit(match)|| match == ' ')
                    {

                    }
                    else if (match == '+' || match == '-')
                    {
                        AppendInfo("Знак числа может быть задан только в его начале");
                        errorCounter++;
                    }
                    else if (char.ToLowerInvariant(match) == 'l')
                    {
                        if (ColumnCount == 2)
                        {
                            AppendInfo("Число не может быть без цифр");
                            hasEndLiteral = true;
                            errorCounter++;
                        }
                        else
                        {
                            AnalyzerState = AnalyzerState.END_DIGIT;
                            hasEndLiteral = true;
                        }
                    }
                    else
                    {
                        AppendInfo("В числе не может быть что-либо кроме цифр, плюса, минуса или литерала l ");
                        errorCounter++;
                    }
                }
                else if (AnalyzerState == AnalyzerState.END_DIGIT)
                {
                    if (char.IsDigit(match) )
                    {
                        AppendInfo("После окончания числа не может быть цифр");
                        errorCounter++;
                    }
                    
                    else if (match == '+' || match == '-')
                    {
                        AppendInfo("Знак числа может быть задан только в его начале");
                        errorCounter++;
                    }
                    else if (char.ToLowerInvariant(match) == 'l')
                    {
                        AppendInfo("В числе не может быть больше одного литерала l");
                        errorCounter++;
                    }
                    else
                    {
                        AppendInfo("В числе не может быть что-либо кроме цифр, плюса, минуса или литерала l ");
                        errorCounter++;
                    }
                }
            }
            if (errorCounter == 0 && hasEndLiteral)
            {
                richTextBox.AppendText("Число записано без ошибок");
            }
            if (!hasEndLiteral)
            {
                AppendInfo("Конец числа должен быть литералом l или L");
            }
        }

        private void AppendInfo(string error = "")
        {
            richTextBox.AppendText($"{error}{", ошибка в позиции "}{ColumnCount}{Environment.NewLine}");
        }
    }
}
