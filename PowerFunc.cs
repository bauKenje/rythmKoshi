namespace RythmKoshi
{
    /// <summary>
    /// Степенная функция
    /// </summary>
    public class PowerFunc
    {
        /// <summary>
        /// Коэфициенты при x^n
        /// </summary>
        private int[] koefs { get; set; }

        /// <summary>
        /// Коэфициенты при x^n (меняются при выносе ответа типа (x-x0)*(Ax^n+...))
        /// </summary>
        private int[] tempKoefs { get; set; }

        /// <summary>
        /// Ответы уравнения
        /// </summary>
        public List<double> answers { get; set; }

        public PowerFunc(int[] koefs)
        {
            this.koefs = koefs;
            this.tempKoefs = koefs.Clone() as int[];
            this.answers = new List<double>();
        }

        /// <summary>
        /// Решить уравнение
        /// </summary>
        public void Solve()
        {
            while (tempKoefs.Count() > 3)
            {
                var answer = SearchAnswer();
                DevideByAnswer(answer);
            }
            SolveSquare();
        }

        /// <summary>
        /// Решить квадратное уравнение, если найдены все ответы кроме 2
        /// </summary>
        private void SolveSquare()
        {
            var descriminant = tempKoefs[1] * tempKoefs[1] - 4 * tempKoefs[0] * tempKoefs[2];
            var answer = -(tempKoefs[1] + Math.Sqrt(descriminant)) / (2 * tempKoefs[0]);
            answers.Add(answer);
            answer = -(tempKoefs[1] - Math.Sqrt(descriminant)) / (2 * tempKoefs[0]);
            answers.Add(answer);
            foreach (var an in answers)
            {
                Console.Write(an < 0 ? $"(x+{-an})" : an > 0 ? $"(x-{an})" : "x");
            }
            Console.WriteLine("=0");
        }

        /// <summary>
        /// Поменять временные коэфициенты при выносе (x-xn)
        /// </summary>
        /// <param name="answer">ответ xn</param>
        /// <exception cref="Exception">Вызывается, если при выносе за скобками остался свободный одночлен</exception>
        private void DevideByAnswer(int answer)
        {
            int[] koefsAfterDevide = new int[tempKoefs.Length - 1];
            for (int i = 0; i < tempKoefs.Length - 1; i++)
            {
                koefsAfterDevide[i] = tempKoefs[i];
                tempKoefs[i + 1] += answer * tempKoefs[i];
            }
            if (tempKoefs.Last() == 0)
            {
                answers.Add(answer);
                tempKoefs = koefsAfterDevide;
                foreach (var an in answers)
                {
                    Console.Write(an < 0 ? $"(x+{-an})" : an > 0 ? $"(x-{an})" : "x");
                }
                Console.Write("(");
                for (int i = 0; i < tempKoefs.Length; i++)
                {
                    if (i > 0 && koefsAfterDevide[i] > 0)
                        Console.Write("+");
                    if (koefsAfterDevide[i] != 1)
                        Console.Write(koefsAfterDevide[i]);
                    if (koefsAfterDevide.Length - i > 2)
                        Console.Write($"x^{koefsAfterDevide.Length - i - 1}");
                    if (koefsAfterDevide.Length - i == 2)
                        Console.Write($"x");
                    if (koefsAfterDevide[i] == 1 && koefsAfterDevide.Length - i == 1)
                        Console.Write("1");
                }
                Console.WriteLine(")");
            }
            else
                throw new Exception($"Неверный ответ: {answer}");
        }

        /// <summary>
        ///Подобрать целый ответ от -100 до 100
        /// </summary>
        /// <returns>Ответ</returns>
        /// <exception cref="Exception">Если нет целых ответов</exception>
        private int SearchAnswer()
        {
            int currentAnswer = 0;
            while (currentAnswer < 100)
            {
                if (CheckAnswer(currentAnswer))
                    return currentAnswer;
                if (CheckAnswer(-currentAnswer))
                    return -currentAnswer;
                currentAnswer++;
            }
            throw new Exception("Целых ответов нет");
        }

        /// <summary>
        /// Проверка правильности ответа
        /// </summary>
        /// <param name="answer">Ответ</param>
        /// <returns>Верный ли ответ. При подстановке уравнение превращается в верное равенство</returns>
        private bool CheckAnswer(int answer)
        {
            var sum = 0;
            int i = 0;
            foreach (var koef in tempKoefs.Reverse())
            {
                sum += koef * (int)Math.Pow(answer, i++);
            }
            return sum == 0;
        }
    }
}
