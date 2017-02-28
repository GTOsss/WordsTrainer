using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace wordbook
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Label labelWord = new Label(), labelMain = new Label(), labelAnswer = new Label();
        Button[] ButtonsAns = { new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button() };
        Button ResetProgress = new Button(), BtnSettings = new Button();
        ProgressBar ProgBarOfWord = new ProgressBar(), ProgBarOfWords = new ProgressBar();
        List<String> FileWords = new List<string>();
        List<String> FileSettings = new List<string>();

        int ValueShowWrods = 4;
        int ValueMustTrueAnswer = 2;
        string strFI = Environment.CurrentDirectory + @"\Import\import.txt";
        string strFIsafe = "import";

        public int ValueShowWrodsP { get { return ValueShowWrods; } set { ValueShowWrods = value; } }
        public int ValueMustTrueAnswerP { get { return ValueMustTrueAnswer; } set { ValueMustTrueAnswer = value; } }
        public string StrFI { get { return strFI; } set { strFI = value; } }
        public string StrFIsafe { get { return strFIsafe; } set { strFIsafe = value; } }

        int widthBtns = 200;

        bool ActivBtn = true;
        sbyte CountAnswer = 0;

        public MainWindow()
        {
            //test();
            InitializeComponent();
            IniOther();
            LoadFile();
            Sorting();
            GenerateShowWords();
            GenerateQuestion();
        }

        private void test()
        {
            List<string> worbook = new List<string>(File.ReadAllLines("WordBookAll.wordbok"));
            List<string> words = new List<string>(File.ReadAllLines("Statistic/16 тысяч строк.txt"));
            List<string> worbook2 = new List<string>();

            string str = TextFileRedactor.GetValueNF(worbook, TextFileRedactor.GetNameOfLine(words[3] + " "));

            WindowProgress wndp = new WindowProgress(new WindowSettings(this));
            wndp.Show();

            for (int i = 0; i < words.Count; i++)
            {
                wndp.ProgBar.Value = Math.Round((double)i / (double)words.Count * (double)100);
                for (int j = 0; j < worbook.Count; j++)
                {
                    if (TextFileRedactor.DeleteSpace(TextFileRedactor.GetNameOfLine(words[i])) == TextFileRedactor.DeleteSpace(TextFileRedactor.GetNameOfLine(worbook[j])))
                    {
                        Console.WriteLine(TextFileRedactor.DeleteSpace(TextFileRedactor.GetNameOfLine(words[i])) + "\n" + TextFileRedactor.DeleteSpace(TextFileRedactor.GetNameOfLine(worbook[j])));
                        break;
                    }
                }
            }
        }

        public void IniOther()
        {
            this.Width = 700;
            this.ResizeMode = ResizeMode.NoResize;
            this.KeyDown += new KeyEventHandler(KeyHandler);

            labelWord.Margin = new Thickness(0, 4, 0, 0);
            labelWord.Width = this.Width;
            labelWord.Height = 36;
            labelWord.Foreground = new SolidColorBrush(Colors.Green);
            labelWord.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            labelWord.FontSize = 16;
            labelWord.VerticalAlignment = VerticalAlignment.Top;
            labelWord.HorizontalAlignment = HorizontalAlignment.Center;
            labelWord.HorizontalContentAlignment = HorizontalAlignment.Center;

            ButtonsAns[2].HorizontalAlignment = HorizontalAlignment.Left;
            ButtonsAns[2].VerticalAlignment = VerticalAlignment.Top;
            ButtonsAns[2].Width = widthBtns;
            ButtonsAns[2].Height = 22;
            ButtonsAns[2].Margin = new Thickness((this.Width / 2) - (ButtonsAns[2].Width / 2) - 8, labelWord.Height + 10, 0, 0);
            ButtonsAns[2].Focusable = false;

            ButtonsAns[1].HorizontalAlignment = HorizontalAlignment.Left;
            ButtonsAns[1].VerticalAlignment = VerticalAlignment.Top;
            ButtonsAns[1].Margin = new Thickness((ButtonsAns[2].Margin.Left - ButtonsAns[2].Width) - 8, ButtonsAns[2].Margin.Top, 0, 0);
            ButtonsAns[1].Width = widthBtns;
            ButtonsAns[1].Height = 22;
            ButtonsAns[1].Focusable = false;

            ButtonsAns[3].HorizontalAlignment = HorizontalAlignment.Left;
            ButtonsAns[3].VerticalAlignment = VerticalAlignment.Top;
            ButtonsAns[3].Margin = new Thickness((ButtonsAns[2].Margin.Left + ButtonsAns[2].Width) + 8, ButtonsAns[2].Margin.Top, 0, 0);
            ButtonsAns[3].Width = widthBtns;
            ButtonsAns[3].Height = 22;
            ButtonsAns[3].Focusable = false;

            ButtonsAns[5].HorizontalAlignment = HorizontalAlignment.Left;
            ButtonsAns[5].VerticalAlignment = VerticalAlignment.Top;
            ButtonsAns[5].Margin = new Thickness(ButtonsAns[2].Margin.Left, ButtonsAns[2].Margin.Top + ButtonsAns[2].Height + 8, 0, 0);
            ButtonsAns[5].Width = widthBtns;
            ButtonsAns[5].Height = 22;
            ButtonsAns[5].Focusable = false;

            ButtonsAns[4].HorizontalAlignment = HorizontalAlignment.Left;
            ButtonsAns[4].VerticalAlignment = VerticalAlignment.Top;
            ButtonsAns[4].Margin = new Thickness((ButtonsAns[2].Margin.Left - ButtonsAns[2].Width) - 8, ButtonsAns[5].Margin.Top, 0, 0);
            ButtonsAns[4].Width = widthBtns;
            ButtonsAns[4].Height = 22;
            ButtonsAns[4].Focusable = false;

            ButtonsAns[6].HorizontalAlignment = HorizontalAlignment.Left;
            ButtonsAns[6].VerticalAlignment = VerticalAlignment.Top;
            ButtonsAns[6].Margin = new Thickness((ButtonsAns[2].Margin.Left + ButtonsAns[2].Width) + 8, ButtonsAns[5].Margin.Top, 0, 0);
            ButtonsAns[6].Width = widthBtns;
            ButtonsAns[6].Height = 22;
            ButtonsAns[6].Focusable = false;

            BtnSettings.HorizontalAlignment = HorizontalAlignment.Right;
            BtnSettings.VerticalAlignment = VerticalAlignment.Top;
            BtnSettings.Margin = new Thickness(0, 5, 5, 0);
            BtnSettings.Width = 30;
            BtnSettings.Height = 29;
            BtnSettings.Background = new LinearGradientBrush(Color.FromRgb(90, 90, 90), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
            BtnSettings.Focusable = false;
            BtnSettings.Click += new RoutedEventHandler(ClickSettings);

            labelMain.Margin = new Thickness(0, ButtonsAns[6].Margin.Top + 30, 0, 0);
            labelMain.Width = this.Width;
            labelMain.Height = 30;
            labelMain.FontSize = 16;
            labelMain.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            labelMain.Background = new SolidColorBrush(Color.FromRgb(55, 55, 55));
            labelMain.VerticalAlignment = VerticalAlignment.Top;
            labelMain.HorizontalAlignment = HorizontalAlignment.Center;
            labelMain.HorizontalContentAlignment = HorizontalAlignment.Center;

            labelAnswer.Margin = new Thickness(0, labelMain.Margin.Top + 32, 0, 0);
            labelAnswer.Width = this.Width;
            labelAnswer.Height = 30;
            labelAnswer.FontSize = 16;
            labelAnswer.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            labelAnswer.VerticalAlignment = VerticalAlignment.Top;
            labelAnswer.HorizontalAlignment = HorizontalAlignment.Center;
            labelAnswer.HorizontalContentAlignment = HorizontalAlignment.Center;

            ResetProgress.HorizontalAlignment = HorizontalAlignment.Left;
            ResetProgress.VerticalAlignment = VerticalAlignment.Top;
            ResetProgress.Content = "Сбросить результаты";
            ResetProgress.Width = 140;
            ResetProgress.Height = 30;
            ResetProgress.Margin = new Thickness(this.Width/2-ResetProgress.Width/2-6, this.Height/2-94,0,0);
            ResetProgress.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            ResetProgress.Background = new LinearGradientBrush(Color.FromRgb(90, 90, 90), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
            ResetProgress.Click += new RoutedEventHandler(ClikResetProgrees);

            ProgBarOfWord.HorizontalAlignment = HorizontalAlignment.Left;
            ProgBarOfWord.VerticalAlignment = VerticalAlignment.Top;
            ProgBarOfWord.Width = this.Width / 100 * 47;
            ProgBarOfWord.Height = 20;
            ProgBarOfWord.Margin = new Thickness((this.Width/100)*1,this.Height+(ProgBarOfWord.Height-85),0,0);
            ProgBarOfWord.Foreground = new SolidColorBrush(Color.FromRgb(0, 150, 0));
            ProgBarOfWord.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));

            ProgBarOfWords.HorizontalAlignment = HorizontalAlignment.Right;
            ProgBarOfWords.VerticalAlignment = VerticalAlignment.Top;
            ProgBarOfWords.Width = this.Width / 100 * 47;
            ProgBarOfWords.Height = 20;
            ProgBarOfWords.Margin = new Thickness(0, this.Height + (ProgBarOfWord.Height - 85), (this.Width / 100) * 1, 0);
            ProgBarOfWords.Foreground = new SolidColorBrush(Color.FromRgb(0, 150, 0));
            ProgBarOfWords.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));

            MainWin.Children.Add(ProgBarOfWord);
            MainWin.Children.Add(ProgBarOfWords);
            MainWin.Children.Add(ResetProgress);
            MainWin.Children.Add(labelWord);
            MainWin.Children.Add(labelMain);
            MainWin.Children.Add(labelAnswer);
            MainWin.Children.Add(ButtonsAns[1]);
            MainWin.Children.Add(ButtonsAns[2]);
            MainWin.Children.Add(ButtonsAns[3]);
            MainWin.Children.Add(ButtonsAns[4]);
            MainWin.Children.Add(ButtonsAns[5]);
            MainWin.Children.Add(ButtonsAns[6]);
            MainWin.Children.Add(BtnSettings);
        }

        void ClickSettings(object s, RoutedEventArgs e)
        {
            WindowSettings WS = new WindowSettings(this);
            WS.Show();
        }

        public void ClikResetProgrees(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < FileWords.Count; i++)
            {
                FileWords = TextFileRedactor.SetValueOfLine(FileWords, i, ";", ",", "0");
                FileWords = TextFileRedactor.SetValueOfLine(FileWords, i, ",", ":", "0");
                FileWords = TextFileRedactor.SetValueOfLine(FileWords, i, ":", "/", "" + ValueMustTrueAnswer);
            }

            File.Delete(strFI);
            File.WriteAllLines(strFI, FileWords);

            GenerateShowWords();
            GenerateQuestion();
        }

        void KeyHandler(object s, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad4:
                    PostClick(1);
                    break;
                case Key.NumPad5:
                    PostClick(2);
                    break;
                case Key.NumPad6:
                    PostClick(3);
                    break;
                case Key.NumPad1:
                    PostClick(4);
                    break;
                case Key.NumPad2:
                    PostClick(5);
                    break;
                case Key.NumPad3:
                    PostClick(6);
                    break;
               
            }
        }

        void PostClick(int NumberBtn)
        {
            if (CountAnswer < 0)
                CountAnswer = 0;

            if (TextFileRedactor.GetValueNF(FileWords, labelWord.Content.ToString()) == ButtonsAns[NumberBtn].Content.ToString())
            {
                for (int i = 1; i < 7; i++)
                {
                    ButtonsAns[i].Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    ButtonsAns[i].Background = new LinearGradientBrush(Color.FromRgb(90, 90, 90), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
                }

                ButtonsAns[NumberBtn].Background = new LinearGradientBrush(Color.FromRgb(0, 210, 0), Color.FromRgb(0, 150, 0), new Point(0.5, 1), new Point(0.5, 0));
                ButtonsAns[NumberBtn].Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                CountAnswer += 1;
            }
            else if (TextFileRedactor.GetValueNF(FileWords, labelWord.Content.ToString()) != ButtonsAns[NumberBtn].Content.ToString())
            {
                for (int i = 1; i < 7; i++)
                {
                    ButtonsAns[i].Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    ButtonsAns[i].Background = new LinearGradientBrush(Color.FromRgb(90, 90, 90), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
                }

                ButtonsAns[NumberBtn].Background = new LinearGradientBrush(Color.FromRgb(80, 10, 10), Color.FromRgb(120, 40, 40), new Point(0.5, 1), new Point(0.5, 0));
                ButtonsAns[NumberBtn].Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250));

                CountAnswer -= 1;
            }

            if (ActivBtn)
                InspectionAnswer(ButtonsAns[NumberBtn].Content.ToString(), NumberBtn);

            if (CountAnswer == 2)
                GenerateQuestion();

        }

        void InspectionAnswer(string answer, int NumberBtn)
        {

            if (TextFileRedactor.GetValueNF(FileWords, labelWord.Content.ToString()) == answer)
            {
                ActivBtn = false;
                labelMain.Background = new SolidColorBrush(Color.FromRgb(22, 95, 22));
                labelMain.Content = "Верно!";
            }
            else
            {
                ActivBtn = false;
                labelMain.Background = new SolidColorBrush(Color.FromRgb(95, 22, 22));
                labelMain.Content = "Не верно!";
                labelAnswer.Content = labelWord.Content + " - " + TextFileRedactor.GetValueNF(FileWords, labelWord.Content.ToString());
            }

            ResoultCount();
        }

        public void LoadFile()
        {
            if (File.Exists("Settings.txt"))
            {
                FileSettings = new List<string>(File.ReadAllLines("Settings.txt"));

                string nameLine = "";

                nameLine = TextFileRedactor.GetNameOfLine(FileSettings[0]);
                ValueShowWrods = Convert.ToInt32(TextFileRedactor.DeleteSpace(TextFileRedactor.GetValueNF(FileSettings, nameLine)));
                nameLine = TextFileRedactor.GetNameOfLine(FileSettings[1]);
                ValueMustTrueAnswer = Convert.ToInt32(TextFileRedactor.DeleteSpace(TextFileRedactor.GetValueNF(FileSettings, nameLine)));
                strFI = FileSettings[2];
                strFIsafe = FileSettings[3];
            }
            else
            {
                FileSettings.Add("ValueShowWrods-" + ValueShowWrods + ";");
                FileSettings.Add("ValeMustTrueAnswer-" + ValueMustTrueAnswer + ";");
                FileSettings.Add(strFI);
                FileSettings.Add(strFIsafe);

                File.WriteAllLines("Settings.txt", FileSettings);
            }

            StrFI = FileSettings[2];
            FileWords = new List<string>(File.ReadAllLines(strFI));

            for (int i = 0; i < FileWords.Count; i++)
            {
                FileWords[i] = TextFileRedactor.LineConctructor(FileWords[i], ValueMustTrueAnswer);
            }

            File.Delete(strFI);
            File.WriteAllLines(strFI, FileWords);
        }

        void GenerateShowWords()
        {
            TextFileRedactor.GenerateShowWords(FileWords, ValueShowWrods);
            Sorting();

            File.Delete(strFI);
            File.WriteAllLines(strFI, FileWords);
        }

        void Sorting()
        {
            FileWords = TextFileRedactor.Sorting(FileWords);
            File.Delete(strFI);
            File.WriteAllLines(strFI, FileWords);
        }

        void GenerateQuestion()
        {
            CountAnswer = 0;

            if (TextFileRedactor.GetCountLinesOfValue(FileWords, 2) == FileWords.Count)
            {
                ActivBtn = false;
                ShowEndStady();
            }
            else
            {
                ResetProgress.Visibility = Visibility.Hidden;

                foreach (Button item in ButtonsAns)
                {
                    item.Visibility = Visibility.Visible;
                }

                ActivBtn = true;
                Random rand = new Random();

                for (int i = 1; i < 7; i++)
                {
                    int r = rand.Next(TextFileRedactor.GetCountLinesOfValue(FileWords, 2), FileWords.Count - 1 - TextFileRedactor.GetCountLinesOfValue(FileWords, 0));

                    if (TextFileRedactor.IsEmptyString(TextFileRedactor.GetValueOfLine(FileWords[r])) == true)
                        MessageBox.Show("В файле иморта допущена ошибка! Номер строки: " + r, "Ошибка");

                    ButtonsAns[i].Content = TextFileRedactor.GetValueOfLine(FileWords[r]);
                    ButtonsAns[i].Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    ButtonsAns[i].Background = new LinearGradientBrush(Color.FromRgb(90, 90, 90), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
                    labelMain.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    labelMain.Background = new SolidColorBrush(Color.FromRgb(55, 55, 55));
                    labelMain.Content = "";
                    labelAnswer.Content = "";
                }

                int minRand = TextFileRedactor.GetCountLinesOfValue(FileWords, 2);
                int maxRand = minRand + (TextFileRedactor.GetCountLinesOfValue(FileWords, 1));

                labelWord.Content = TextFileRedactor.GetNameOfLine(FileWords[rand.Next(minRand, maxRand)]);
                ButtonsAns[rand.Next(1, 6)].Content = TextFileRedactor.GetValueNF(FileWords, labelWord.Content.ToString());

                ShowStatistics();        
            }
        }

        void ShowStatistics()
        {
            int AllPercent = 0;

            foreach (string el in FileWords)
            {
                AllPercent += TextFileRedactor.GetPercentOfLine(el);
            }

            ProgBarOfWord.Value = TextFileRedactor.GetPercentOfNF(FileWords, labelWord.Content.ToString());
            ProgBarOfWords.Value = (int)Math.Round(((float)AllPercent) / FileWords.Count);

        }

        void ResoultCount()
        {
            string lineOfFile = "";
            int LineIdOfFile = 0;
            foreach (var item in FileWords)
            {
                if (labelWord.Content.ToString() == TextFileRedactor.GetNameOfLine(item))
                {
                    lineOfFile = item;
                    break;
                }
                LineIdOfFile++;
            }

            float tAns = (float)Convert.ToInt32(TextFileRedactor.GetValueOfLine(lineOfFile, ",", ":"));
            float fAns = (float)Convert.ToInt32(TextFileRedactor.GetValueOfLine(lineOfFile, ":", "/"));

            //Console.WriteLine(Math.Round(tAns/fAns * 100));

            if (labelAnswer.Content.ToString().Length == 0 && (Math.Round(tAns / fAns * 100)) >= 100)
            {
                //Console.WriteLine(LineIdOfFile);
                FileWords = TextFileRedactor.SetValueOfLine(FileWords, LineIdOfFile, ",", ":", ((int)tAns) + 1 + "");
                FileWords = TextFileRedactor.SetValueOfLine(FileWords, LineIdOfFile, ";", ",", 2 + "");
                GenerateShowWords();
            }
            else if (labelAnswer.Content.ToString().Length == 0 && (Math.Round(tAns / fAns * 100)) < 100)
            {
                FileWords = TextFileRedactor.SetValueOfLine(FileWords, LineIdOfFile, ",", ":", ((int)tAns) + 1 + "");
            }
            else if (labelAnswer.Content.ToString().Length > 0)
            {
                FileWords = TextFileRedactor.SetValueOfLine(FileWords, LineIdOfFile, ":", "/", ((int)fAns) + 1 + "");
            }

            Sorting();

            File.Delete(strFI);
            File.WriteAllLines(strFI, FileWords);

            ShowStatistics();
        }

        void ShowEndStady()
        {
            foreach (Button item in ButtonsAns)
            {
                item.Visibility = Visibility.Hidden;
            }

            ResetProgress.Visibility = Visibility.Visible;
            labelMain.Background = new SolidColorBrush(Color.FromRgb(22, 95, 22));
            labelWord.Content = "";
            labelMain.Content = "Все слова выучены!";
        }

        public void ChangeSettings()
        {
            TextFileRedactor.SetValueOfLine(FileSettings, 0, "-", ";", ValueShowWrods + "");
            TextFileRedactor.SetValueOfLine(FileSettings, 1, "-", ";", ValueMustTrueAnswer + "");
            FileSettings[2] = strFI;
            FileSettings[3] = StrFIsafe;

            File.Delete("Settings.txt");
            File.WriteAllLines("Settings.txt", FileSettings);
        }

    }
}
