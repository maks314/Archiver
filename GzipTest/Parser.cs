using System.Text.RegularExpressions;

namespace GzipTest
{
    class Parser
    {
        private Regex regExp;
        private string operation = "";
        private string pattern = "";
        private string firstName = "";
        private string secondName = "";
        private string extension = "";

        public string Operation { get => operation; set => operation = value; }
        public string Pattern { get => pattern; set => pattern = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string SecondName { get => secondName; set => secondName = value; }
        public string Extension { get => extension; set => extension = value; }

        public void ParserParameters(string inputString)
        {
            Pattern = @"\b((c|dec)ompress)\b";
            regExp = new Regex(Pattern);
            Operation = regExp.Match(inputString).ToString();
            inputString = DeConcat(inputString, Operation);
            Pattern = @"^(\b\w+\.[a-z][^\s]*)";
            regExp = new Regex(Pattern);
            FirstName = regExp.Match(inputString).ToString();
            Pattern = @"(?<=\w+\.).+(?=$)";
            regExp = new Regex(Pattern);
            Extension = regExp.Match(FirstName).ToString();
            inputString = DeConcat(inputString, FirstName);
            SecondName = inputString.ToString();

        }

        public static string DeConcat(string strFirst, string strSecond)
        {
            return (string)strFirst.Substring(strFirst.IndexOf(strSecond) + strSecond.Length + 1);
        }
    }
}
