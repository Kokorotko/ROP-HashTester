﻿using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HashTester
{
    public static class FormManagement
    {
        #region Enum
        public enum FolderType
        {
            Base,
            Password,
            Collision,
            Log
        }
        #endregion
        public static void SaveLog(ListBox listbox, Form form)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Settings.DirectoryPathToLogs;
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.FileName = "log.txt";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.WriteLine(Languages.Translate(551)  + ": " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    writer.WriteLine(Languages.Translate(552) +  ": " + form.Name);
                    foreach (string item in listbox.Items)
                    {
                        writer.WriteLine(item);
                    }
                }
                MessageBox.Show(Languages.Translate(553), Languages.Translate(10028), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show(Languages.Translate(554), Languages.Translate(10019), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }        
        public static int NumberOfThreadsToUse()
        {
            int percentage = Settings.ThreadsUsagePercentage;
            if (percentage == 0) return 1; //Single Thread
            return (int)Math.Ceiling(percentage / 100.0 * Environment.ProcessorCount);
        }
        public static bool UseMultiThread()
        {
            int percentage = Settings.ThreadsUsagePercentage;
            if (percentage == 0) return false;
            else return true;
        }
        public static bool UseLightMode()
        {
            switch(Settings.VisualMode)
            {
                case Settings.VisualModeEnum.System: return RegistryUseLightMode();
                case Settings.VisualModeEnum.Light: return true;
                case Settings.VisualModeEnum.Dark: return false;
                default: return true;
            }
        }
        private static bool RegistryUseLightMode()
        {
            string registryKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            string valueName = "AppsUseLightTheme";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    object value = key.GetValue(valueName);
                    if (value is int intValue)
                    {
                        if (intValue == 1) return true;
                        else return false;
                    }
                }
            }
            Console.WriteLine("Couldnt find Registry for AppsUseLightTheme.");
            Console.WriteLine("Settings theme as light");
            return true;
        }
        #region FormTheme
        public static void SetUpFormTheme(Form form)
        {
            Color controlColor;
            Color controlText;
            Color windows;
            Color windowsText;
            Color borderColor;
            bool lightMode = true;
            if (UseLightMode()) //Light Theme
            {
                controlColor = SystemColors.Control;
                controlText = SystemColors.ControlText;
                windows = SystemColors.Window;
                windowsText = SystemColors.WindowText;
                borderColor = SystemColors.ControlDarkDark;
            }
            else //Dark Theme
            {
                lightMode = false;
                controlColor = Color.FromArgb(45, 45, 48);
                controlText = Color.WhiteSmoke;
                windows = Color.FromArgb(30, 30, 30);
                windowsText = Color.WhiteSmoke;
                borderColor = SystemColors.ControlLight;
            }
            form.BackColor = controlColor;
            foreach (Control control in form.Controls) 
            {
                if (control is Label || control is Button || control is CheckBox || control is RadioButton)
                {
                    control.BackColor = controlColor; //background
                    control.ForeColor = controlText; //text
                    if (control is Button button)
                    {                        
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderSize = 1;
                        button.FlatAppearance.BorderColor = borderColor;
                    }
                }
                else if (control is GroupBox box)
                {
                    box.BackColor = controlColor;
                    box.ForeColor = controlText;
                    foreach (Control child in box.Controls) //just apply for childs
                    {
                        child.BackColor = controlColor; //background
                        child.ForeColor = controlText; //text
                        if (child is Button button)
                        {
                            button.FlatStyle = FlatStyle.Flat;
                            button.FlatAppearance.BorderSize = 1;
                            button.FlatAppearance.BorderColor = borderColor;
                        }
                    }
                }
                else if (control is ProgressBar)
                {
                    control.BackColor = controlColor; //background
                    control.ForeColor = Color.Green; //text
                }
                else if (control is ToolStrip menuStrip)
                {
                    menuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable(!lightMode));
                    ApplyThemeToMenu(menuStrip, controlColor, controlText);
                }
                else
                {
                    control.BackColor = windows; //background
                    control.ForeColor = windowsText; //text
                }
            }
        }
        private static void ApplyThemeToMenu(ToolStrip menuStrip, Color backColor, Color textColor)
        {
            menuStrip.BackColor = backColor;
            menuStrip.ForeColor = textColor;

            foreach (ToolStripMenuItem menuItem in menuStrip.Items)
            {
                ApplyThemeToMenuItem(menuItem, backColor, textColor); //Dont worry, it stops...one day
            }
        }
        private static void ApplyThemeToMenuItem(ToolStripMenuItem menuItem, Color backColor, Color textColor)
        {
            menuItem.BackColor = backColor;
            menuItem.ForeColor = textColor;

            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenuItem)
                {
                    ApplyThemeToMenuItem(subMenuItem, backColor, textColor);
                }
            }
        }
        #endregion
    }
}
