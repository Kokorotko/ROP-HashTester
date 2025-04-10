﻿using HashTester.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HashTester.Hasher;
using static HashTester.Settings;

namespace HashTester
{
    public class OutputHandler
    {
        Hasher.HashingAlgorithm algorithm;
        Hasher hasher = new Hasher();
        public OutputHandler(Hasher.HashingAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }
        public OutputHandler() { }
        public string OutputStyleString(string originalString, string hash, int indexOfHash, bool isSaltUsed, bool isPepperUsed, string salt, string pepper)
        {
            //Console.WriteLine("**OutputStyleString**");
            string outputString = hash;
            if (Settings.OutputStyleIncludeOriginalString)
            {
                //Console.WriteLine("OutputStyleIncludeOriginalString");
                outputString = originalString + ": " + outputString;
            }
            if (Settings.OutputStyleIncludeHashAlgorithm)
            {
                //Console.WriteLine("OutputStyleIncludeHashAlgorithm");
                switch (algorithm)
                {
                    case HashingAlgorithm.MD5: outputString = "(MD5) " + outputString; break;
                    case HashingAlgorithm.SHA1: outputString = "(SHA1) " + outputString; break;
                    case HashingAlgorithm.SHA256: outputString = "(SHA256) " + outputString; break;
                    case HashingAlgorithm.SHA512: outputString = "(SHA512) " + outputString; break;
                    case HashingAlgorithm.RIPEMD160: outputString = "(RIPEMD160) " + outputString; break;
                    case HashingAlgorithm.CRC32: outputString = "(CRC32) " + outputString; break;
                }
            }
            if (Settings.OutputStyleIncludeNumberOfHash)
            {
                //Console.WriteLine("OutputStyleIncludeNumberOfHash");
                outputString = indexOfHash.ToString() + ". " + outputString;
            }
            if (Settings.OutputStyleIncludeSaltPepper)
            {
                //Console.WriteLine("OutputStyleIncludeSaltPepper");
                if (isSaltUsed) outputString += " (" + Languages.Translate(402) + ": " + salt + ")";
                if (isPepperUsed) outputString += " (" + Languages.Translate(403) + ": " + pepper + ")";
            }
            return outputString;
        }

        public string[] OutputStyleString(string originalString, string[] hash, bool isSaltUsed, bool isPepperUsed, string salt, string pepper)
        {
            string[] output = new string[hash.Length];   
            for (int i = 0; i < hash.Length; i++)
            {
                string partOfTheOriginalString = originalString.Substring(0, i + 1);
                output[i] = OutputStyleString(partOfTheOriginalString, hash[i], i + 1, isSaltUsed, isPepperUsed, salt, pepper);
            }
            return output;
        }

        public void OutputTypeShowChecksum(string[] checksums, Hasher.HashingAlgorithm[] algorithm, ListBox listBox)
        {
            string[] output = new string[checksums.Length];
            for (int i = 0; i < checksums.Length; i++)
            {
                output[i] = algorithm[i].ToString() + ": " + checksums[i];
            }
            OutputTypeShow(output, listBox);
        }

        /// <summary>
        /// Handles output based on user settings.
        /// </summary>
        public void OutputTypeShow(string outputString, ListBox listBox)
        {
            switch (Settings.OutputType)
            {
                case OutputTypeEnum.MessageBox:
                    MessageBox.Show(outputString);
                    break;
                case OutputTypeEnum.Listbox:
                    listBox.Items.Add(outputString);
                    listBox.TopIndex = listBox.Items.Count - 1; //go all the way down
                    break;
                case OutputTypeEnum.TXTFile:
                    try
                    {
                        SaveFileDialog saveFileDialogCustom = new SaveFileDialog();
                        saveFileDialogCustom.InitialDirectory = Settings.DirectoryExeBase;
                        saveFileDialogCustom.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        saveFileDialogCustom.DefaultExt = "txt";
                        saveFileDialogCustom.FileName = "Hashed text";
                        saveFileDialogCustom.AddExtension = true;
                        if (saveFileDialogCustom.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllText(saveFileDialogCustom.FileName, outputString);
                        }
                        else MessageBox.Show(Languages.Translate(55), Languages.Translate(10025), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Languages.Translate(56) + "\n" + ex.Message, Languages.Translate(10025), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
            }
        }

        /// <summary>
        /// Handles output based on user settings for multiple string.
        /// </summary>
        public void OutputTypeShow(string[] arrayOutputString, ListBox listBox)
        {
            switch (Settings.OutputType)
            {
                case OutputTypeEnum.MessageBox:
                    MessageBox.Show(ArrayStringToOne(arrayOutputString));
                    break;
                case OutputTypeEnum.Listbox:
                    foreach (string s in arrayOutputString)
                    {
                        if (s != "") listBox.Items.Add(s);
                    }
                    listBox.TopIndex = listBox.Items.Count - 1; //go all the way down
                    break;
                case OutputTypeEnum.TXTFile:
                    try
                    {
                        SaveFileDialog saveFileDialogCustom = new SaveFileDialog();
                        saveFileDialogCustom.InitialDirectory = Settings.DirectoryExeBase;
                        saveFileDialogCustom.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        saveFileDialogCustom.DefaultExt = "txt";
                        saveFileDialogCustom.FileName = "Multiple hashed texts";
                        saveFileDialogCustom.AddExtension = true;
                        if (saveFileDialogCustom.ShowDialog() == DialogResult.OK)
                        {                            
                            File.WriteAllText(saveFileDialogCustom.FileName, ArrayStringToOne(arrayOutputString));
                        }
                        else MessageBox.Show(Languages.Translate(55), Languages.Translate(10025), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Languages.Translate(56) + "\n" + ex.Message, Languages.Translate(10025), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
            }
        }

        private string ArrayStringToOne(string[] array)
        {
            string outputString = "";
            foreach (string s in array)
            {
                outputString += s + "\n";
            }
            outputString = outputString.TrimEnd('\n');
            return outputString;
        }
    }
}
