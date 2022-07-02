using System.Diagnostics;

namespace RythmKoshi
{
    public static class GnuLauncher
    {
        /// <summary>
        /// Запустить gnuplot
        /// </summary>
        /// <param name="exePath">путь к gnuplot.exe</param>
        /// <param name="choords">Коо</param>
        public static void Launch(string exePath, Dictionary<double, double> choords)
        {
            string filePath = string.Join("//", exePath.Split("//").SkipLast(1).Concat(new string[1] { "koshiChoord.txt" }));
            using (var fs = File.CreateText(filePath))
            {
                foreach (KeyValuePair<double, double> point in choords)
                    fs.WriteLine($"{point.Key}\t{point.Value}".Replace(",", "."));
            }

            using (Process plotProcess = new Process())
            {
                plotProcess.StartInfo.FileName = exePath;
                plotProcess.StartInfo.UseShellExecute = false;
                plotProcess.StartInfo.RedirectStandardInput = true;
                plotProcess.Start();
                using (StreamWriter sw = plotProcess.StandardInput)
                {
                    string strInputText = $"plot \"{filePath}\" with lines";
                    sw.WriteLine(strInputText);
                    sw.Flush();

                    Console.ReadLine();
                    sw.Close();
                    File.Delete(filePath);
                }
                plotProcess.Close();
            }
        }
    }
}
