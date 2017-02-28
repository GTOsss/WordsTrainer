using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wordbook
{
    public class WindowProgress : Window
    {
        Grid grid = new Grid();
        Label labelTitle = new Label();
        Button btnMain = new Button();
        ProgressBar progBar = new ProgressBar();
        WindowSettings wndSettings;

        public ProgressBar ProgBar { get { return progBar; } set { progBar = value; } }
        public Label LabelTitlle { get { return labelTitle; } set { labelTitle = value; } }
        public Button BtnMain { get { return btnMain; } set { btnMain = value; } }

        public WindowProgress(WindowSettings wndSettings)
        {
            this.wndSettings = wndSettings;
            Ini();
        }

        void Ini()
        {
            this.Width = 300;
            this.Height = 116;
            this.ResizeMode = ResizeMode.NoResize;
            this.Closed += new EventHandler(ClosedWindow);

            grid.Width = this.Width - 14;
            grid.Height = this.Height - 36;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0, 0, 0, 0);

            labelTitle.Width = grid.Width;
            labelTitle.Height = 30;
            labelTitle.Content = "Формирование файла";
            labelTitle.FontSize = 12;
            labelTitle.HorizontalAlignment = HorizontalAlignment.Left;
            labelTitle.VerticalAlignment = VerticalAlignment.Top;
            labelTitle.HorizontalContentAlignment = HorizontalAlignment.Center;
            labelTitle.VerticalContentAlignment = VerticalAlignment.Top;
            labelTitle.Margin = new Thickness(0,-4,0,0);

            progBar.Width = grid.Width;
            progBar.Height = 24;
            progBar.HorizontalAlignment = HorizontalAlignment.Left;
            progBar.VerticalAlignment = VerticalAlignment.Top;
            progBar.Margin = new Thickness(0,22,0,0);

            btnMain.Width = 70;
            btnMain.Height = 25;
            btnMain.HorizontalAlignment = HorizontalAlignment.Center;
            btnMain.VerticalAlignment = VerticalAlignment.Bottom;
            btnMain.Margin = new Thickness(0,0,0,4);
            btnMain.Content = "Отменить";
            btnMain.Click += new RoutedEventHandler(clickBtnMain);
 
            this.AddChild(grid);
            grid.Children.Add(progBar);
            grid.Children.Add(labelTitle);
            grid.Children.Add(btnMain);
        }

        void ClosedWindow(object s, EventArgs e)
        {
            wndSettings.StopTranslateImportFile();
            TextFileRedactor.work = false;
        }

        void clickBtnMain(object s, RoutedEventArgs e)
        {
            wndSettings.StopTranslateImportFile();
            TextFileRedactor.work = false;
            this.Close();
        }
    }

    public class WindowSettings : Window
    {
        Grid grid = new Grid();
        Button BtnSave = new Button(), btnResetProgress = new Button(), btnOverviewSaveFi = new Button();
        TextBox tbValueShowWrods = new TextBox(), tbValueMustTrueAnswer = new TextBox(); 
        Label labelValueShowWrods = new Label(), labelValueMustTrueAnswer = new Label(), labelFI = new Label();
        MainWindow MainWind;
        WindowProgress wndProgress;
        OpenFileDialog openFileDialog;



        GroupBox groupBoxCreateIF = new GroupBox();
        Grid gridCreateIF = new Grid(); 
        Button btnCreateImportFile = new Button(), btnOverviewCreateFI = new Button();
        Label labelEnterNameFI = new Label(), labelFileOrigin = new Label();
        TextBox textBoxEnterName = new TextBox();

        GroupBox groupBoxCreateStatisticF = new GroupBox();
        Grid gridCreateStatisticF = new Grid();
        Button btnCreateStatisticF = new Button(), btnOverviewCreateFStatistic = new Button();
        Label labelStatisticF = new Label(), labelFileOriginS = new Label();
        TextBox textBoxEnterNameStatisticF = new TextBox();


        private string strLabelFI = "Файл импорта: ";
        private string strFI;
        private string strAllFI;

        private volatile bool isWork = true;
        private volatile string WillNameIF;

        public WindowSettings(MainWindow mw)
        {
            MainWind = mw;
            Ini();
        }

        void Ini()
        {
            this.Width = 350;
            this.Height = 400;
            this.Background = new SolidColorBrush(Color.FromRgb(150, 150, 150));
            this.ResizeMode = ResizeMode.NoResize;

            grid.Width = this.Width - 14;
            grid.Height = this.Height - 36;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0, 0, 0, 0);

            // создание импорт файла {
            #region CreateFileImport

            gridCreateIF.Width = this.Width-24;
            gridCreateIF.Height = 80;

            groupBoxCreateIF.Width = gridCreateIF.Width;
            groupBoxCreateIF.Height = gridCreateIF.Height;
            groupBoxCreateIF.Header = "Создание файла импорта";
            groupBoxCreateIF.Content = gridCreateIF;
            groupBoxCreateIF.VerticalAlignment = VerticalAlignment.Bottom;
            groupBoxCreateIF.Margin = new Thickness(0,0,0,8);

            labelEnterNameFI.Width = 124;
            labelEnterNameFI.Height = 28;
            labelEnterNameFI.Content = "Имя файла импорта:";
            labelEnterNameFI.HorizontalAlignment = HorizontalAlignment.Left;
            labelEnterNameFI.VerticalAlignment = VerticalAlignment.Top;
            labelEnterNameFI.Margin = new Thickness(0,-2,0,0);

            textBoxEnterName.Width = groupBoxCreateIF.Width - labelEnterNameFI.Width - 18;
            textBoxEnterName.Height = 22;
            textBoxEnterName.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxEnterName.VerticalAlignment = VerticalAlignment.Top;
            textBoxEnterName.Margin = new Thickness(labelEnterNameFI.Width+4,2,0,0);

            btnCreateImportFile.Width = 92;
            btnCreateImportFile.Height = 25;
            btnCreateImportFile.HorizontalAlignment = HorizontalAlignment.Right;
            btnCreateImportFile.VerticalAlignment = VerticalAlignment.Bottom;
            btnCreateImportFile.Margin = new Thickness(0, 0, 12, btnCreateImportFile.Height);
            btnCreateImportFile.Content = "Сформировать";
            btnCreateImportFile.Click += new RoutedEventHandler(btnCreateImportFileClick);

            btnOverviewCreateFI.Width = 60;
            btnOverviewCreateFI.Height = 25;
            btnOverviewCreateFI.Content = "Обзор";
            btnOverviewCreateFI.HorizontalAlignment = HorizontalAlignment.Right;
            btnOverviewCreateFI.VerticalAlignment = VerticalAlignment.Bottom;
            btnOverviewCreateFI.Margin = new Thickness(0,0,btnCreateImportFile.Width+16,btnCreateImportFile.Margin.Bottom);
            btnOverviewCreateFI.Click += new RoutedEventHandler(btnOverviewClickCreateFI);

            labelFileOrigin.Width = groupBoxCreateIF.Width - btnCreateImportFile.Width - btnOverviewCreateFI.Width - 20;
            labelFileOrigin.Height = 28;
            labelFileOrigin.HorizontalAlignment = HorizontalAlignment.Left;
            labelFileOrigin.VerticalAlignment = VerticalAlignment.Bottom;
            labelFileOrigin.HorizontalContentAlignment = HorizontalAlignment.Left;
            labelFileOrigin.Margin = new Thickness(0, 0, 0, labelFileOrigin.Height - 6);

            gridCreateIF.Children.Add(labelEnterNameFI);
            gridCreateIF.Children.Add(labelFileOrigin);
            gridCreateIF.Children.Add(textBoxEnterName);
            gridCreateIF.Children.Add(btnCreateImportFile);
            gridCreateIF.Children.Add(btnOverviewCreateFI);

            #endregion
            // создание импорт файла }

            // создание файла статистики {
            #region CreateFileStatistic

            gridCreateIF.Width = this.Width - 24;
            gridCreateIF.Height = 80;

            groupBoxCreateStatisticF.Width = gridCreateIF.Width;
            groupBoxCreateStatisticF.Height = gridCreateIF.Height;
            groupBoxCreateStatisticF.Header = "Создание файла статистики";
            groupBoxCreateStatisticF.Content = gridCreateStatisticF;
            groupBoxCreateStatisticF.VerticalAlignment = VerticalAlignment.Bottom;
            groupBoxCreateStatisticF.Margin = new Thickness(0, 0, 0, groupBoxCreateIF.Margin.Bottom+groupBoxCreateIF.Height);

            labelStatisticF.Width = 135;
            labelStatisticF.Height = 28;
            labelStatisticF.Content = "Имя файла статистики:";
            labelStatisticF.HorizontalAlignment = HorizontalAlignment.Left;
            labelStatisticF.VerticalAlignment = VerticalAlignment.Top;
            labelStatisticF.Margin = new Thickness(0, -2, 0, 0);

            textBoxEnterNameStatisticF.Width = groupBoxCreateStatisticF.Width - labelStatisticF.Width - 18;
            textBoxEnterNameStatisticF.Height = 22;
            textBoxEnterNameStatisticF.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxEnterNameStatisticF.VerticalAlignment = VerticalAlignment.Top;
            textBoxEnterNameStatisticF.Margin = new Thickness(labelStatisticF.Width + 4, 2, 0, 0);

            btnCreateStatisticF.Width = 92;
            btnCreateStatisticF.Height = 25;
            btnCreateStatisticF.HorizontalAlignment = HorizontalAlignment.Right;
            btnCreateStatisticF.VerticalAlignment = VerticalAlignment.Bottom;
            btnCreateStatisticF.Margin = new Thickness(0, 0, 0, 4);
            btnCreateStatisticF.Content = "Сформировать";
            btnCreateStatisticF.Click += new RoutedEventHandler(btnCreateStatisticFClick);

            btnOverviewCreateFStatistic.Width = 60;
            btnOverviewCreateFStatistic.Height = 25;
            btnOverviewCreateFStatistic.Content = "Обзор";
            btnOverviewCreateFStatistic.HorizontalAlignment = HorizontalAlignment.Right;
            btnOverviewCreateFStatistic.VerticalAlignment = VerticalAlignment.Bottom;
            btnOverviewCreateFStatistic.Margin = new Thickness(0, 0, btnCreateStatisticF.Width + 4, btnCreateStatisticF.Margin.Bottom);
            btnOverviewCreateFStatistic.Click += new RoutedEventHandler(btnOverviewClickCreateFStatistic);

            labelFileOriginS.Width = groupBoxCreateStatisticF.Width - btnCreateStatisticF.Width - btnOverviewCreateFStatistic.Width - 20;
            labelFileOriginS.Height = 28;
            labelFileOriginS.HorizontalAlignment = HorizontalAlignment.Left;
            labelFileOriginS.VerticalAlignment = VerticalAlignment.Bottom;
            labelFileOriginS.HorizontalContentAlignment = HorizontalAlignment.Left;
            labelFileOriginS.Margin = new Thickness(0, 0, 0, 4);

            gridCreateStatisticF.Children.Add(labelStatisticF);
            gridCreateStatisticF.Children.Add(textBoxEnterNameStatisticF);
            gridCreateStatisticF.Children.Add(btnCreateStatisticF);
            gridCreateStatisticF.Children.Add(btnOverviewCreateFStatistic);
            gridCreateStatisticF.Children.Add(labelFileOriginS);

            #endregion
            // создание файла статистики }


            labelValueMustTrueAnswer.Width = this.Width;
            labelValueMustTrueAnswer.Height = 28;
            labelValueMustTrueAnswer.HorizontalAlignment = HorizontalAlignment.Left;
            labelValueMustTrueAnswer.VerticalAlignment = VerticalAlignment.Top;
            labelValueMustTrueAnswer.Margin = new Thickness(0, 0, 0, 0);
            labelValueMustTrueAnswer.Content = "Требуемое кол-во правильных ответов";
            labelValueMustTrueAnswer.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            labelValueMustTrueAnswer.Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250));

            tbValueMustTrueAnswer.Width = 30;
            tbValueMustTrueAnswer.Height = 20;
            tbValueMustTrueAnswer.HorizontalAlignment = HorizontalAlignment.Left;
            tbValueMustTrueAnswer.VerticalAlignment = VerticalAlignment.Top;
            tbValueMustTrueAnswer.Margin = new Thickness(labelValueMustTrueAnswer.Width - tbValueMustTrueAnswer.Width - 20, labelValueMustTrueAnswer.Margin.Top + 3, 0, 0);
            tbValueMustTrueAnswer.Text = MainWind.ValueMustTrueAnswerP + "";

            labelValueShowWrods.Width = this.Width;
            labelValueShowWrods.Height = 28;
            labelValueShowWrods.HorizontalAlignment = HorizontalAlignment.Left;
            labelValueShowWrods.VerticalAlignment = VerticalAlignment.Top;
            labelValueShowWrods.Margin = new Thickness(0, labelValueMustTrueAnswer.Margin.Top + labelValueMustTrueAnswer.Height + 2, 0, 0);
            labelValueShowWrods.Content = "Кол-во генерируемых слов для вопросов";
            labelValueShowWrods.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            labelValueShowWrods.Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250));

            tbValueShowWrods.Width = 30;
            tbValueShowWrods.Height = 20;
            tbValueShowWrods.HorizontalAlignment = HorizontalAlignment.Left;
            tbValueShowWrods.VerticalAlignment = VerticalAlignment.Top;
            tbValueShowWrods.Margin = new Thickness(tbValueMustTrueAnswer.Margin.Left, labelValueShowWrods.Margin.Top + 3, 0, 0);
            tbValueShowWrods.Text = MainWind.ValueShowWrodsP + "";

            labelFI.Width = this.Width;
            labelFI.Height = 28;
            labelFI.HorizontalAlignment = HorizontalAlignment.Left;
            labelFI.VerticalAlignment = VerticalAlignment.Top;
            labelFI.Margin = new Thickness(0, labelValueShowWrods.Margin.Top + labelValueShowWrods.Height + 2, 0,0);
            labelFI.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            labelFI.Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250));
            labelFI.Content = strLabelFI + MainWind.StrFIsafe;

            btnOverviewSaveFi.Width = 60;
            btnOverviewSaveFi.Height = 25;
            btnOverviewSaveFi.Content = "Обзор";
            btnOverviewSaveFi.HorizontalAlignment = HorizontalAlignment.Right;
            btnOverviewSaveFi.VerticalAlignment = VerticalAlignment.Top;
            btnOverviewSaveFi.Margin = new Thickness(0, labelValueShowWrods.Margin.Top + labelValueShowWrods.Height + 3, 4, 0);
            btnOverviewSaveFi.Click += new RoutedEventHandler(btnOverviewSaveFiClick);

            BtnSave.Width = 70;
            BtnSave.Height = 25;
            BtnSave.HorizontalAlignment = HorizontalAlignment.Right;
            BtnSave.VerticalAlignment = VerticalAlignment.Top;
            BtnSave.Margin = new Thickness(0, labelFI.Margin.Top + labelFI.Height + 4, 5, 0);
            BtnSave.Content = "Сохранить";
            BtnSave.Click += new RoutedEventHandler(btnSaveClick);

            btnResetProgress.Width = 110;
            btnResetProgress.Height = 25;
            btnResetProgress.HorizontalAlignment = HorizontalAlignment.Left;
            btnResetProgress.VerticalAlignment = VerticalAlignment.Top;
            btnResetProgress.Margin = new Thickness(4, labelFI.Margin.Top + labelFI.Height + 4, 0, 0);
            btnResetProgress.Content = "Сброс прогресса";
            btnResetProgress.Click += new RoutedEventHandler(btnResetProgressClick);

            this.AddChild(grid);
            grid.Children.Add(labelValueShowWrods);
            grid.Children.Add(labelValueMustTrueAnswer);
            grid.Children.Add(labelFI);
            grid.Children.Add(BtnSave);
            grid.Children.Add(btnResetProgress);
            grid.Children.Add(btnOverviewSaveFi);
            grid.Children.Add(tbValueMustTrueAnswer);
            grid.Children.Add(tbValueShowWrods);
            grid.Children.Add(groupBoxCreateIF);
            grid.Children.Add(groupBoxCreateStatisticF);
        }

        void btnCreateStatisticFClick(object s, RoutedEventArgs e)
        {
            wndProgress = new WindowProgress(this);
            wndProgress.Show();
            Thread thr = new Thread(GenerateStatisticFile);
            thr.Start();
        }

        void btnOverviewClickCreateFStatistic(object s, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.ShowDialog();
            labelFileOriginS.Content = openFileDialog.SafeFileName;
            textBoxEnterNameStatisticF.Text = TextFileRedactor.GetNameOfLine(openFileDialog.SafeFileName, ".");
        }

        void btnOverviewSaveFiClick(object s, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.ShowDialog();
            strFI = TextFileRedactor.GetNameOfLine(openFileDialog.SafeFileName, ".");
            strAllFI = openFileDialog.FileName;
            labelFI.Content = strLabelFI + strFI;
        }

        void btnOverviewClickCreateFI(object s, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.ShowDialog();
            labelFileOrigin.Content = openFileDialog.SafeFileName;
            textBoxEnterName.Text = TextFileRedactor.GetNameOfLine(openFileDialog.SafeFileName, ".");
        }

        void btnSaveClick(object s, RoutedEventArgs e)
        {
            MainWind.ValueShowWrodsP = Convert.ToInt32(TextFileRedactor.DeleteSpace(tbValueShowWrods.Text));
            MainWind.ValueMustTrueAnswerP = Convert.ToInt32(TextFileRedactor.DeleteSpace(tbValueMustTrueAnswer.Text));
            MainWind.StrFI = strAllFI;
            MainWind.StrFIsafe = strFI;
            MainWind.ChangeSettings();
            MainWind.LoadFile();
        }

        void btnResetProgressClick(object s, RoutedEventArgs e)
        {
            MainWind.ClikResetProgrees(s, e);
        }

        void btnCreateImportFileClick(object s, RoutedEventArgs e)
        {
            GenerateImportFile();
            WillNameIF = textBoxEnterName.Text;
        }

        void GenerateImportFile()
        {
            List<string> fileContent = new List<string>(File.ReadAllLines(openFileDialog.FileName));
            Thread thrTranslate = new Thread(TranslateImportFile);
            wndProgress = new WindowProgress(this);
            wndProgress.Show();
            thrTranslate.Start((object)TextFileRedactor.GenerateFileImport(fileContent));
        }

        void GenerateStatisticFile()
        {
            List<string> generateFile = TextFileRedactor.GenerateFileStatistic(new List<string>(File.ReadAllLines(openFileDialog.FileName)), this);

            if (TextFileRedactor.work)
            {
                generateFile = TextFileRedactor.Sorting(generateFile, "-", ";");
                Dispatcher.Invoke(() => { SetValueProgress(100, true); });
                Dispatcher.Invoke(() => { File.WriteAllLines("Statistic/" + textBoxEnterNameStatisticF.Text + ".txt", generateFile); });
            }
        }

        void TranslateImportFile(object words)
        {
            List<string> WordsBook = new List<string>(File.ReadAllLines("WordBookAll.wordbok"));
            List<string> newImport = (List<string>)words;
            List<string> import = new List<string>();
            List<string> errorWords = new List<string>();
            isWork = true;
            for (int i = 0; i < newImport.Count && isWork; i++)
            {
                Action action1 = () => { SetValueProgress((int)Math.Round((double)i / (double)newImport.Count * (double)100), false); };
                Dispatcher.Invoke(action1);
                if (TextFileRedactor.GetValueNF(WordsBook, newImport[i] + " ") != "error 2")
                    import.Add(newImport[i] + " - " + TextFileRedactor.GetValueNF(WordsBook, newImport[i] + " "));
                else
                    errorWords.Add(newImport[i] + " - " + TextFileRedactor.GetValueNF(WordsBook, newImport[i] + " "));
            }

            if (isWork)
            {

                Action action2 = () => { SetValueProgress(100, true); };
                Dispatcher.Invoke(action2);
                File.Delete("Import/" + WillNameIF + ".txt");
                File.WriteAllLines("Import/" + WillNameIF + ".txt", import);

                File.Delete("Import/" + WillNameIF + "_Error" + ".txt");
                File.WriteAllLines("Import/" + WillNameIF + "_Error" + ".txt", errorWords);
            }
        }

        public void StopTranslateImportFile()
        {
            isWork = false;
        }

        public void SetValueProgress(int progress, bool end)
        {
            wndProgress.ProgBar.Value = progress;

            if (end)
            {
                wndProgress.LabelTitlle.Content = "Фаил создан";
                wndProgress.BtnMain.Content = "Ок";
            }

        }

    }
}
