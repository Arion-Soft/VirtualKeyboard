using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using VirtualKeyboard.Models;
using Timer = System.Timers.Timer;

namespace VirtualKeyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VirtualKeyboardWindow : INotifyPropertyChanged
    {
        private bool _mustQuit;
        public VirtualKeyboardWindow()
        {
            InitializeComponent();
            // var timer = new Timer(100);
            // timer.Elapsed += TimerOnElapsed;
            // timer.Start();

            new Thread(() =>
            {
                while (!_mustQuit)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var lang = "";
                        var empty = InputLanguage.CurrentInputLanguage.LayoutName;
                        lang = $"{InputLanguage.CurrentInputLanguage.Culture.Name}";
                        Title = lang;

                        if (lang == "ru-RU")
                        {
                            Dispatcher.Invoke(() =>
                            {
                                BtnQ.Content = Console.CapsLock ? "Й" : "Й".ToLower();
                                BtnW.Content = Console.CapsLock ? "Ц" : "Ц".ToLower();
                                BtnE.Content = Console.CapsLock ? "У" : "У".ToLower();
                                BtnR.Content = Console.CapsLock ? "К" : "К".ToLower();
                                BtnT.Content = Console.CapsLock ? "Е" : "Е".ToLower();
                                BtnY.Content = Console.CapsLock ? "Н" : "Н".ToLower();
                                BtnU.Content = Console.CapsLock ? "Г" : "Г".ToLower();
                                BtnI.Content = Console.CapsLock ? "Ш" : "Ш".ToLower();
                                BtnO.Content = Console.CapsLock ? "Щ" : "Щ".ToLower();
                                BtnP.Content = Console.CapsLock ? "З" : "З".ToLower();
                                BtnA.Content = Console.CapsLock ? "Ф" : "Ф".ToLower();
                                BtnS.Content = Console.CapsLock ? "Ы" : "Ы".ToLower();
                                BtnD.Content = Console.CapsLock ? "В" : "В".ToLower();
                                BtnF.Content = Console.CapsLock ? "А" : "А".ToLower();
                                BtnG.Content = Console.CapsLock ? "П" : "П".ToLower();
                                BtnH.Content = Console.CapsLock ? "Р" : "Р".ToLower();
                                BtnJ.Content = Console.CapsLock ? "О" : "О".ToLower();
                                BtnK.Content = Console.CapsLock ? "Л" : "Л".ToLower();
                                BtnL.Content = Console.CapsLock ? "Д" : "Д".ToLower();
                                BtnZ.Content = Console.CapsLock ? "Я" : "Я".ToLower();
                                BtnX.Content = Console.CapsLock ? "Ч" : "Ч".ToLower();
                                BtnC.Content = Console.CapsLock ? "С" : "С".ToLower();
                                BtnV.Content = Console.CapsLock ? "М" : "М".ToLower();
                                BtnB.Content = Console.CapsLock ? "И" : "И".ToLower();
                                BtnN.Content = Console.CapsLock ? "Т" : "Т".ToLower();
                                BtnM.Content = Console.CapsLock ? "Ь" : "Ь".ToLower();
                                BtnOpenBracket.Content = Console.CapsLock ? "Х" : "Х".ToLower();
                                BtnCloseBracket.Content = Console.CapsLock ? "Ъ" : "Ъ".ToLower();
                                BtnSeparator.Content = Console.CapsLock ? "Ж" : "Ж".ToLower();
                                BtnComma.Content = Console.CapsLock ? "Б" : "Б".ToLower();
                                BtnPeriod.Content = Console.CapsLock ? "Ю" : "Ю".ToLower();
                                BtnQuotes.Content = Console.CapsLock ? "Э" : "Э".ToLower();
                            });
                        }
                        else if (lang == "en-US")
                        {
                            Dispatcher.Invoke(() =>
                            {
                                BtnQ.Content = Console.CapsLock ? "Q" : "Q".ToLower();
                                BtnW.Content = Console.CapsLock ? "W" : "W".ToLower();
                                BtnE.Content = Console.CapsLock ? "E" : "E".ToLower();
                                BtnR.Content = Console.CapsLock ? "R" : "R".ToLower();
                                BtnT.Content = Console.CapsLock ? "T" : "T".ToLower();
                                BtnY.Content = Console.CapsLock ? "Y" : "Y".ToLower();
                                BtnU.Content = Console.CapsLock ? "U" : "U".ToLower();
                                BtnI.Content = Console.CapsLock ? "I" : "I".ToLower();
                                BtnO.Content = Console.CapsLock ? "O" : "O".ToLower();
                                BtnP.Content = Console.CapsLock ? "P" : "P".ToLower();
                                BtnA.Content = Console.CapsLock ? "A" : "A".ToLower();
                                BtnS.Content = Console.CapsLock ? "S" : "S".ToLower();
                                BtnD.Content = Console.CapsLock ? "D" : "D".ToLower();
                                BtnF.Content = Console.CapsLock ? "F" : "F".ToLower();
                                BtnG.Content = Console.CapsLock ? "G" : "G".ToLower();
                                BtnH.Content = Console.CapsLock ? "H" : "H".ToLower();
                                BtnJ.Content = Console.CapsLock ? "J" : "J".ToLower();
                                BtnK.Content = Console.CapsLock ? "K" : "K".ToLower();
                                BtnL.Content = Console.CapsLock ? "L" : "L".ToLower();
                                BtnZ.Content = Console.CapsLock ? "Z" : "Z".ToLower();
                                BtnX.Content = Console.CapsLock ? "X" : "X".ToLower();
                                BtnC.Content = Console.CapsLock ? "C" : "C".ToLower();
                                BtnV.Content = Console.CapsLock ? "V" : "V".ToLower();
                                BtnB.Content = Console.CapsLock ? "B" : "B".ToLower();
                                BtnN.Content = Console.CapsLock ? "N" : "N".ToLower();
                                BtnM.Content = Console.CapsLock ? "M" : "M".ToLower();
                                BtnOpenBracket.Content = "[";
                                BtnCloseBracket.Content = "]";
                                BtnSeparator.Content = ";";
                                BtnComma.Content = ",";
                                BtnPeriod.Content = ".";
                                BtnQuotes.Content = "'";
                            });
                        }
                    });
                    Thread.Sleep(100);
                }
            }).Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
        }

        private WindowController _mWinController;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _mWinController = new WindowController(this);
            //Когда окно теряет фокус, не позволяйте windows автоматически возвращать фокус при нажатии на что-либо
            _mWinController.DisableFocus();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VirtualKeyboardWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _mustQuit = true;
        }

        private void VirtualKeyboardWindow_OnClosed(object sender, EventArgs e)
        {
            _mustQuit = true;
        }
    }
}