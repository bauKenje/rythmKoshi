using RythmKoshi;

Console.WriteLine("Решение уравнения типа a*y'''''+b*y''''+c*y'''+d*y''+e*y'+f*y=0");
Console.WriteLine("Введите коэфициенты через запятую");
string strKoefs = Console.ReadLine();
if (string.IsNullOrEmpty(strKoefs))
    strKoefs = "1, 15, 90, 270, 405, 243";
IEnumerable<int> koefs = strKoefs.Split('\u002C').Select(x => int.Parse(x.Trim()));

Console.WriteLine("Введите начальные условия y(0), y'(0), y''(0), y'''(0), y''''(0)");
string strStartValues = Console.ReadLine();
if (string.IsNullOrEmpty(strStartValues))
    strStartValues = "0, 3, -9, -8, 0";
IEnumerable<int> startValues = strStartValues.Split('\u002C').Select(x => int.Parse(x.Trim()));
if (koefs.Count() - startValues.Count() != 1)
    throw new Exception("Количество начальных условий должно быть на 1 меньше количества коэфициентов");

Console.WriteLine("Введите через запятую интервал вычисления");
string strInterval = Console.ReadLine();
if (string.IsNullOrEmpty(strInterval))
    strInterval = "0, 5";
IEnumerable<int> interval = strInterval.Split('\u002C').Select(x => int.Parse(x.Trim()));
if (interval.Count() != 2)
    throw new Exception("Интервал должен состоять из 2 чисел - начало и конец интервала");
if (interval.First() >= interval.Last())
    throw new Exception("Начало интервала должно быть меньше конца интервала");

try
{
    DiffFunc diffFunc = new DiffFunc(koefs.ToArray(), startValues.ToArray(), interval.First(), interval.Last());
    diffFunc.Solve();
    Console.WriteLine("Укажите путь к gnuplot.exe");
    string gnuPath;

    do
    {
        gnuPath = Console.ReadLine();
        if (File.Exists(gnuPath))
            break;
        Console.WriteLine("Файл не найден, укажите корректный путь");
    } while (true);
    gnuPath = gnuPath.Replace("\"", "").Replace("\\", "//");
    GnuLauncher.Launch(gnuPath, diffFunc.choords);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
