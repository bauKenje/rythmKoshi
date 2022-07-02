namespace RythmKoshi
{
    /// <summary>
    /// Дифферециальная функция
    /// </summary>
    public class DiffFunc
    {
        /// <summary>
        /// Коэфициенты из условия 
        /// </summary>
        private int[] koefs { get; set; }

        /// <summary>
        /// Начальные значения
        /// </summary>
        private int[] startValues { get; set; }

        /// <summary>
        /// Ответы степенной функции
        /// </summary>
        //private List<double> powersOfE { get; set; } = new List<double>();

        /// <summary>
        /// Интервал приращения аргумента - dX
        /// </summary>
        private double intervalStep { get => (endInterval - beginInterval) / (double)100; }

        /// <summary>
        /// Начальное значение аргумента - Xmin
        /// </summary>
        private int beginInterval { get; set; }

        /// <summary>
        /// Конечное значение аргумента - Xmax
        /// </summary>
        private int endInterval { get; set; }

        private PowerFunc powerFunc { get; set; }

        /// <summary>
        /// Координаты графика
        /// </summary>
        public Dictionary<double, double> choords { get; set; } = new Dictionary<double, double>();

        public DiffFunc(int[] koefs, int[] startValues, int beginInterval, int endInterval)
        {
            this.koefs = koefs;
            this.startValues = startValues;
            this.beginInterval = beginInterval;
            this.endInterval = endInterval;
        }

        public void Solve()
        {
            SolvePowerFunc();
            FindChords();
        }

        /// <summary>
        /// Решить степенную функцию
        /// </summary>
        public void SolvePowerFunc()
        {
            powerFunc = new PowerFunc(koefs);
            powerFunc.Solve();
            bool isNeedPlus = false;
            for (var i = powerFunc.answers.Count - 1; i >= 0; i--)
            {
                var startVal = startValues[i];
                if (startVal != 0)
                {
                    Console.Write(startValues[i] > 0 && isNeedPlus ? "+" : "");
                    isNeedPlus = true;
                    Console.Write($"{startValues[i]}*x^{i}*e^{powerFunc.answers[i]}");
                }
            }
            Console.WriteLine("=0");
        }

        /// <summary>
        /// Заполнить координаты
        /// </summary>
        public void FindChords()
        {
            double[] chordValues = new double[koefs.Length - 1];
            double[] currentValues = startValues.Select(x => (double)x).ToArray();
            double x = beginInterval;

            while (x <= endInterval)
            {
                for (var i = 0; i < currentValues.Length - 1; i++)
                {
                    chordValues[i] = Math.Round(currentValues[i] + intervalStep * currentValues[i + 1], 6);
                    //Console.WriteLine($"y{i} = {currentValues[i]}+{intervalStep}*{currentValues[i + 1]}");
                }
                double multimember = 0;
                //Console.Write($"y{currentValues.Length - 1} = {currentValues[currentValues.Length - 1]} + {intervalStep} * (");
                for (var i = 0; i < currentValues.Length; i++)
                {
                    multimember += Math.Round(-currentValues[i] * koefs[i] / koefs[0], 6);
                    //Console.Write($"{-currentValues[i]} * {constKoefs[i]} / {constKoefs[0]}+");
                }
                //Console.WriteLine(")");
                chordValues[currentValues.Length - 1] = Math.Round(currentValues[currentValues.Length - 1] + intervalStep * multimember, 6);

                double y = 0;
                for (int i = 0; i < currentValues.Length; i++)
                {
                    y += Math.Round(startValues[i] * Math.Pow(x, i) * Math.Pow(Math.E, -x * powerFunc.answers[i]), 6);
                }
                y = Math.Round(y, 6);


                Console.WriteLine($"x = {x};");

                for (var i = 0; i < currentValues.Length; i++)
                {
                    var difSymbols = string.Concat(Enumerable.Repeat("'", i));
                    Console.WriteLine($"y{difSymbols}({x}) = {currentValues[i]}");
                    currentValues[i] = chordValues[i];
                }

                Console.WriteLine($"y = {y};");
                Console.WriteLine();
                choords.Add(x, y);

                x += intervalStep;
                x = Math.Round(x, 6);
            }
        }
    }
}
