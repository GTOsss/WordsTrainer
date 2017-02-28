using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace wordbook
{
    static class TextFileRedactor
    {
        static public bool end = false;
        static public bool work = true;
        
        /// <summary>
        /// Удаление пробелов строки.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string DeleteSpace(string arg)
        {
            if (arg == null)
                return "null";

            string str = arg;
            int lengthArray = str.Length;

            for (int i = 0; i < lengthArray; i++)
            {
                if (str[i].ToString() == " " || str[i].ToString() == "\t")
                {
                    arg = arg.Remove(i - (str.Length - arg.Length), 1);
                }
            }
            return arg;
        }

        /// <summary>
        /// Удаление всего кроме значения.
        /// </summary>
        /// <param name="arg">Строка из файла со знаком "=".</param>
        /// <returns></returns>
        static string DeleteToSign(string arg, string sign1, string sign2)
        {
            //DeleteSpace(arg);
            string str = arg;
            int lengthArray = str.Length;

            for (int i = 0; i < lengthArray; i++)
            {
                if (str[i].ToString() != sign1)
                {
                    arg = arg.Remove(i - (str.Length - arg.Length), 1);
                }
                else
                {
                    arg = arg.Remove(i - (str.Length - arg.Length), 1);
                    break;
                }
            }
            return GetNameOfLine(arg, sign2).Replace('*', '-');
            //return GetNameOfLine(DeleteSpace(arg), sign2);
        }

        /// <summary>
        /// Получить значение из строки.
        /// </summary>
        /// <param name="StringLine">Строка из файла со знаком "-".</param>
        /// <returns></returns>
        public static string GetValueOfLine(string StringLine)
        {
            //StringLine = DeleteSpace(StringLine);
            StringLine = DeleteToSign(StringLine, "-", ";");
            return StringLine.Replace('*', '-');
            //return DeleteSpace(StringLine);
        }

        /// <summary>
        /// Получить значение из строки.
        /// </summary>
        /// <param name="StringLine">Строка из файла.</param>
        /// <param name="Sign">Символ для ориентира.</param>
        /// <returns></returns>
        public static string GetValueOfLine(string StringLine, string Sign1, string Sign2)
        {
            //StringLine = DeleteSpace(StringLine);
            StringLine = DeleteToSign(StringLine, Sign1, Sign2);
            return StringLine.Replace('*', '-');
            //return DeleteSpace(StringLine);
        }

        /// <summary>
        /// Изменение значения. НЕ указывать новое значение разделенное на пробелы!
        /// </summary>
        /// <param name="FileArg">Массив строк из файла (файл).</param>
        /// <param name="Lineid">Номер строки в которой будут внесены изменения.</param>
        /// <param name="sign1">Символ с которого наченется чтение значения.</param>
        /// <param name="sign2">Символ с которого прекратится чтение значения.</param>
        /// <param name="newValue">Новое значение. (БЕЗ пробелов)</param>
        /// <returns></returns>
        public static List<string> SetValueOfLine(List<string> FileArg, int Lineid, string sign1, string sign2, string newValue)
        {
            bool start = false;

            for (int i = 0; i < FileArg[Lineid].Length; i++)
            {
                if (FileArg[Lineid][i] == sign1[0])
                    start = true;
                else if (FileArg[Lineid][i] == sign2[0])
                    break;

                int lengthValue = (DeleteSpace(GetValueOfLine(FileArg[Lineid], sign1, sign2))).Length;
                if (start == true && FileArg[Lineid][i] == (DeleteSpace(GetValueOfLine(FileArg[Lineid], sign1, sign2))[0]))
                {
                    string str1 = FileArg[Lineid].Remove(i), str2 = FileArg[Lineid].Remove(0, i + lengthValue);
                    FileArg[Lineid] = str1 + newValue + str2 + " ";
                    break;
                }
            }

            return FileArg;
        }

        /// <summary>
        /// Получить значение по имени и файлу.
        /// </summary>
        /// <param name="FileArg">Файл в виде массива строк.</param>
        /// <param name="NameElement">Имя элемента.</param>
        /// <returns></returns>
        public static string GetValueNF(string[] FileArg, string NameElement)
        {
            for (int i = 0; i < FileArg.Length; i++)
            {
                if (GetNameOfLine(FileArg[i]) == NameElement)
                    return GetValueOfLine(FileArg[i]).Replace('*', '-');
            }

            return "0";
        }

        public static string GetValueNF(List<string> FileArg, string NameElement)
        {
            for (int i = 0; i < FileArg.Count; i++)
            {
                if (GetNameOfLine(FileArg[i]) == NameElement)
                {
                    return GetValueOfLine(FileArg[i]).Replace('*', '-');
                }
            }

            return "error 2";
        }

        public static string GetValueNF(List<string> FileArg, string NameElement, string sign1, string sign2)
        {
            foreach (string item in FileArg)
            {
                if (GetNameOfLine(item) == NameElement)
                    return (GetValueOfLine(item, sign1, sign2));
            }
            return "error 004";
        }

        /// <summary>
        /// Получить имя элемента из строки.
        /// </summary>
        /// <param name="arg">Строка со знаком "-".</param>
        /// <returns></returns>
        public static string GetNameOfLine(string arg, string sign)
        {
            if (arg == null)
                return "error 01";

            //DeleteSpace(arg);

            string str = arg;
            int lengthArray = str.Length;

            for (int i = lengthArray - 1; i > 0; i--)
            {
                if (str[i].ToString() != sign)
                {
                    arg = arg.Remove(i, 1);
                }
                else
                {
                    arg = arg.Remove(i, 1);
                    break;
                }
            }

            return arg.Replace('*', '-');
            //return DeleteSpace(arg);
        }

        /// <summary>
        /// Получить имя элемента из строки.
        /// </summary>
        /// <param name="arg">Строка со знаком "=".</param>
        /// <returns></returns>
        public static string GetNameOfLine(string arg)
        {
            if (arg == null)
                return "";

            //DeleteSpace(arg);

            string str = arg;
            int lengthArray = str.Length;

            for (int i = lengthArray - 1; i > 0; i--)
            {
                if (str[i].ToString() != "-")
                {
                    arg = arg.Remove(i, 1);
                }
                else
                {
                    arg = arg.Remove(i, 1);
                    break;
                }
            }
            return arg.Replace('*', '-');
            //return DeleteSpace(arg);
        }

        /// <summary>
        /// Возвращает true если строка пустая или состоит из пробелов.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsEmptyString(string text)
        {
            Regex rg = new Regex(@"[A-Za-z0-9А-Яа-я_]");
            Match mt = rg.Match(text);
            string summ = "";
            while (mt.Success)
            {
                summ += mt.Value;
                mt = mt.NextMatch();
            }

            if (summ.Length > 0)
                return false;
            else
                return true;

        }

        public static string LineConctructor(string line, int trueValue)
        {
            //DeleteSpace(Line);
            string str = line;
            string sign = ";";
            int lengthArray = str.Length;
            bool ItIsOk = false;

            for (int i = 0; i < lengthArray; i++)
            {
                if (str[i].ToString() == sign)
                {
                    switch (sign)
                    {
                        case ";":
                            i = 0;
                            sign = ",";
                            break;
                        case ",":
                            i = 0;
                            sign = ":";
                            break;
                        case ":":
                            i = 0;
                            sign = "/";
                            break;
                    }

                    if (sign == "/")
                        ItIsOk = true;
                }
            }

            if (!ItIsOk)
            {
                int countt = 10;
                string tub = "";
                for (int i = 0; i < countt; i++)
                {
                    if ((line.Length + 1) < i * 4)
                    {
                        tub += "\t";
                    }
                }

                line += ";" + tub + "0, 0:" + trueValue + " /";
            }

            return line;
        }

        static public int GetCountMaxLineLength(List<string> FileArg)
        {
            int MaxLegth = 0;
            foreach (string el in FileArg)
            {
                if (MaxLegth < el.Length)
                    MaxLegth = el.Length;
            }
            return MaxLegth;
        }

        public static List<string> Sorting(List<string> FileArg)
        {
            quickSort(FileArg, 0, FileArg.Count - 1);
            return FileArg;
        }

        public static List<string> Sorting(List<string> FileArg, string sign1, string sign2)
        {
            quickSort(FileArg, 0, FileArg.Count - 1, sign1, sign2);
            return FileArg;
        }

        static List<string> quickSort(List<string> a, int l, int r)
        {
            string temp;
            int x = Convert.ToInt32(DeleteSpace(GetValueOfLine((a[l + (r - l) / 2]), ";", ",")));
            
            int i = l;
            int j = r;
            while (i <= j)
            {
                while (Convert.ToInt32(DeleteSpace(GetValueOfLine((a[i]), ";", ","))) > x) i++;
                
                while (Convert.ToInt32(DeleteSpace(GetValueOfLine((a[j]), ";", ","))) < x) j--;
                
                if (i <= j)
                {
                    temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;
                    i++;
                    j--;
                }
            }
            if (i < r)
                quickSort(a, i, r);

            if (l < j)
                quickSort(a, l, j);

            return a;
        } // спасибо Чарльзу Хоару 

        static List<string> quickSort(List<string> a, int l, int r, string sign1, string sign2)
        {
            string temp;
            int x = Convert.ToInt32(DeleteSpace(GetValueOfLine((a[l + (r - l) / 2]), sign1, sign2)));
            
            int i = l;
            int j = r;
            while (i <= j)
            {
                while (Convert.ToInt32(DeleteSpace(GetValueOfLine((a[i]), sign1, sign2))) > x) i++;
                
                while (Convert.ToInt32(DeleteSpace(GetValueOfLine((a[j]), sign1, sign2))) < x) j--;
                
                if (i <= j)
                {
                    temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;
                    i++;
                    j--;
                }
            }
            if (i < r)
                quickSort(a, i, r, sign1, sign2);

            if (l < j)
                quickSort(a, l, j, sign1, sign2);

            return a;
        } // спасибо Чарльзу Хоару 

        public static int GetCountLinesOfValue(List<string> FileArg, int value)
        {
            int count = 0;
            for (int i = 0; i < FileArg.Count; i++)
            {
                if (Convert.ToInt32(DeleteSpace(GetValueOfLine(FileArg[i], ";", ","))) == value)
                    count++;
            }

            return count;
        }

        public static List<string> GenerateShowWords(List<string> FileArg, int ShowCount)
        {
            Random rand = new Random();
            FileArg = Sorting(FileArg);

            for (int i = 0; GetCountLinesOfValue(FileArg, 1) < ShowCount && GetCountLinesOfValue(FileArg, 0) > 0; i++)
            {
                int minRand = GetCountLinesOfValue(FileArg, 1) + GetCountLinesOfValue(FileArg, 2);
                int r = rand.Next(minRand, FileArg.Count);
                FileArg = SetValueOfLine(FileArg, r, ";", ",", "1");
                FileArg = Sorting(FileArg);
            }

            return Sorting(FileArg);
        }

        public static int GetPercentOfNF(List<string> FileArg, string NameElement)
        {
            float fPercent = Convert.ToSingle(DeleteSpace(GetValueNF(FileArg, NameElement, ":", "/"))) / Convert.ToSingle(DeleteSpace(GetValueNF(FileArg, NameElement, ",", ":")));
            int ip = (int)Math.Round(100 / fPercent);

            if (ip > 100)
                ip = 100;

            return ip;
        }

        public static int GetPercentOfLine(string Line)
        {
            float fPercent = Convert.ToSingle(DeleteSpace(GetValueOfLine(Line, ",", ":")))/Convert.ToSingle(DeleteSpace(GetValueOfLine(Line, ":", "/")));
            int ip = (int)Math.Round((fPercent * 100));

            if (ip > 100)
                ip = 100;

            return ip;
        }

        public static List<string> GenerateFileImport(List<string> FileTextAng)
        {
            Regex reg = new Regex(@"(([a-zA-Z']+-)*[a-zA-Z']+)");

            List<string> words = new List<string>();

            bool reiteration = false;

            for (int i = 0; i < FileTextAng.Count; i++)
            {
                Match m = reg.Match(FileTextAng[i]);

                while (m.Success)
                {
                    for (int j = 0; j < words.Count; j++)
                    {
                        if (words[j] == m.Value.ToLower())
                        {
                            reiteration = true;
                            break;
                        }

                    }

                    if (!reiteration)
                    {
                        words.Add(m.Value.ToLower());
                    }

                    reiteration = false;
                    m = m.NextMatch();
                }
            }
            return words;
        }

        public static List<string> GenerateFileStatistic(List<string> FileTextAng)
        {
            Regex reg = new Regex(@"(([a-zA-Z']+-)*[a-zA-Z']+)");

            List<string> words = new List<string>();

            bool reiteration = false;

            for (int i = 0; i < FileTextAng.Count; i++)
            {
                Match m = reg.Match(FileTextAng[i]);

                while (m.Success)
                {
                    for (int j = 0; j < words.Count; j++)
                    {
                        if (DeleteSpace(GetNameOfLine(words[j])) == DeleteSpace(m.Value.ToLower()))
                        {
                            words = SetValueOfLine(words, j, "-", ";", (Convert.ToInt32(DeleteSpace(GetValueOfLine(words[j]))) + 1).ToString());
                            reiteration = true;
                            break;
                        }

                    }

                    if (!reiteration)
                    {
                        words.Add(m.Value.ToLower().Replace('-', '*') + " - 1;");
                    }

                    reiteration = false;
                    m = m.NextMatch();
                }
            }
            return words;
        }

        public static List<string> GenerateFileStatistic(List<string> FileTextAng, WindowSettings WndSett)
        {
            work = true;

            Regex reg = new Regex(@"(([a-zA-Z']+-)*[a-zA-Z']+)");

            List<string> words = new List<string>();

            bool reiteration = false;

            for (int i = 0; i < FileTextAng.Count && work; i++)
            {
                Action action = () => { WndSett.SetValueProgress((int)Math.Round((((float)i) / ((float)FileTextAng.Count)) * 100.0f), false); };
                WndSett.Dispatcher.Invoke(action);

                Match m = reg.Match(FileTextAng[i]);

                while (m.Success)
                {
                    for (int j = 0; j < words.Count; j++)
                    {
                        if (DeleteSpace(GetNameOfLine(words[j])) == DeleteSpace(m.Value.ToLower()))
                        {
                            words = SetValueOfLine(words, j, "-", ";", (Convert.ToInt32(DeleteSpace(GetValueOfLine(words[j]))) + 1).ToString());
                            reiteration = true;
                            break;
                        }

                    }

                    if (!reiteration)
                    {
                        words.Add(m.Value.ToLower().Replace('-', '*') + " - 1;");
                    }

                    reiteration = false;
                    m = m.NextMatch();
                }
            }
            return words;
        }

    }
}
