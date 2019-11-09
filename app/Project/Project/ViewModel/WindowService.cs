using Microsoft.Win32;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project.ViewModel
{
    public class WindowService
    {
        public Stack<Window> Windows { get; set; }
        public Command Closing { get; set; }
        private bool isClosing { get; set; }
        public Frame Frame { get; set; }

        public WindowService()
        {
            Windows = new Stack<Window>();
            Closing = new Command(_Closing);

            isClosing = false;
        }

        public void Push(Window window)
        {
            foreach (var item in Windows)
            {
                item.Hide();
            }
            Windows.Push(window);
            Windows.Peek().Show();
        }

        public void Back()
        {
            isClosing = true;
            Windows.Pop().Close();
            isClosing = false;
            Windows.Peek().Show();
        }

        public void Navigate(Page page)
        {
            if (Frame == null || page == null) return;
            Frame.Navigate(page);
        }
        public void NavigateBack()
        {
            if (Frame == null || !Frame.CanGoBack) return;
            Frame.GoBack();
        }
        public void NavigateForward()
        {
            if (Frame == null || !Frame.CanGoForward) return;
            Frame.GoForward();
        }

        public void ShowErrorMessage(string text)
        {
            MessageBox.Show(text, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowOkMessage(string text)
        {
            MessageBox.Show(text, "OK", MessageBoxButton.OK);
        }

        public string OpenFileDialog(string filters)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filters;//"ImageFiles(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (dialog.ShowDialog() ?? false)
                return dialog.FileName;
            return null;
        }

        private void _Closing()
        {
            if (isClosing) return;
            isClosing = true;
            Windows.Pop();
            foreach (var item in Windows)
            {
                item.Close();
            }
            Windows.Clear();
        }
    }
}
