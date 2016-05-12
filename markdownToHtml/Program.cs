using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMark;
using System.Diagnostics.Contracts;

namespace markdownToHtml {
    class Program {
        static void Main(string[] args) {
            var md = new markdownToHtml();
            md.Run();
        }
    }

    class markdownToHtml {
        public void Run() {
            var filePath = getFilePath();
            Contract.Assume(filePath != null);
            var convertedHtml = convertToHtml(filePath);
            var outputDirectory = Path.GetDirectoryName(filePath);
            var outputFileName = Path.GetFileNameWithoutExtension(filePath) + "_converted.html";
            var outputFilePath = Path.Combine(outputDirectory, outputFileName);
            File.WriteAllText(outputFilePath, convertedHtml);
        }

        private string getFilePath() {
            string[] files = Environment.GetCommandLineArgs();
            if (files.Length > 1) {
                if (Path.GetExtension(files[1]) == ".md") {
                    return files[1];
                }
            }
            throw new ArgumentException("ファイルが指定されてないか、無効なファイルです");
        }

        private string convertToHtml(string filePath) {
            var input = File.ReadAllLines(filePath);
            var md = string.Empty;
            foreach (var line in input) {
                md += string.Format("{0}\r\n", line);
            }
            var bldr = new StringBuilder();           
            bldr.Append(@"<!DOCTYPE html>
<html lang='ja'>
<head>
  <meta charset='utf-8'>
  <title></title>
</head>
<body>
");
            bldr.Append(CommonMarkConverter.Convert(md));
            bldr.Append(@"</body>
</html>");
            return bldr.ToString();
        }
    }
}
