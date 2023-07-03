using System;
using System.Windows;
using System.Windows.Controls;

namespace VirtualKeyboard.Models
{
    public class VButton : Button
    {
        public KeyboardCommand KbCommand;

        public static readonly DependencyProperty ShiftTextProperty =
            DependencyProperty.Register(nameof(ShiftText), typeof(string), typeof(TextBlock), new FrameworkPropertyMetadata(""));

        public string ShiftText
        {
            get => (string)GetValue(ShiftTextProperty);
            set => SetValue(ShiftTextProperty, value);
        }

        // Контент кнопки
        public string Title
        {
            set
            {
                if (value.StartsWith("\\u")) ParseUnicode(value);
                else Content = value;
            }
        }

        public string Code
        {
            get => (string)GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }

        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register(nameof(Code), typeof(string), typeof(VButton), new PropertyMetadata());

        public VButton()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Code))
                KbCommand.KbKeys = Code.Split(' ');
            PreviewMouseLeftButtonDown += BtnTouch_Down;
            PreviewMouseLeftButtonUp += BtnTouch_Up;
            PreviewTouchDown += BtnTouch_Down;
            PreviewTouchUp += BtnTouch_Up;
        }

        private static void BtnTouch_Down(object sender, EventArgs e)
        {
            if (sender is VButton btn)
                KeyLoopHandler.BeginKeypress(btn.KbCommand);
        }

        private static void BtnTouch_Up(object sender, EventArgs e)
        {
            KeyLoopHandler.EndKeypress();
        }

        private void ParseUnicode(string txt)
        {
            int pos = 0;
            string final = "";

            //Проверьте, есть ли в строке только один экранированный символ юникода.
            if (txt.Length == 6)
            {
                Content = (char)int.Parse(txt.Substring(2), System.Globalization.NumberStyles.HexNumber);
                return;
            }

            //Более одного возможного символа юникода
            while (pos < txt.Length)
            {
                //Если не экранирован юникод, добавьте в окончательный вариант
                if (txt[pos] != '\\')
                {
                    final += txt[pos];
                    pos++;
                }
                else
                {
                    //экранированный юникод, проанализируйте его.
                    var tmp = txt.Substring(pos + 2, 4);
                    final += (char)int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                    pos += 6;
                }
            }

            Content = final;
        }
    }
}