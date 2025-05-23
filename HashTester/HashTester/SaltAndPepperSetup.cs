﻿using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HashTester
{
    public partial class SaltAndPepperSetup : Form
    {
        public SaltAndPepperSetup()
        {
            InitializeComponent();
            if (Settings.UseSalt) groupBoxSalt.Enabled = true;
            if (Settings.UsePepper) groupBoxPepper.Enabled = true;
            textBoxHashID.Text = SetHashID();
        }

        public SaltAndPepperSetup(bool useSalt, bool usePepper)
        {
            InitializeComponent();
            if (useSalt) groupBoxSalt.Enabled = true;
            if (usePepper) groupBoxPepper.Enabled = true;
            textBoxHashID.Text = SetHashID();
        }

        public SaltAndPepperSetup(bool useSalt, bool usePepper, string hashID)
        {
            InitializeComponent();
            if (useSalt) groupBoxSalt.Enabled = true;
            if (usePepper) groupBoxPepper.Enabled = true;
            if (!String.IsNullOrEmpty(hashID)) textBoxHashID.Text = SetHashID(hashID);
            else textBoxHashID.Text = SetHashID();
        }
        private void SaltAndPepperQuestion_Load(object sender, EventArgs e)
        {
            FormManagement.SetUpFormTheme(this);
            #region Languages
            this.Name = Languages.Translate(708);
            groupBoxSalt.Text = Languages.Translate(402);
            groupBoxPepper.Text = Languages.Translate(403);
            radioButtonSaltGen.Text = Languages.Translate(404); //hehe funny web number
            radioButtonPepperGen.Text = Languages.Translate(405);
            labelLenghtSalt.Text = Languages.Translate(406);
            labelLenghtPepper.Text = Languages.Translate(407);
            radioButtonSaltOwn.Text = Languages.Translate(408);
            radioButtonPepperOwn.Text = Languages.Translate(409);
            labelOwnSalt.Text = Languages.Translate(410);
            labelOwnPepper.Text = Languages.Translate(411);
            labelID.Text = Languages.Translate(412);
            buttonGenerate.Text = Languages.Translate(413);
            #endregion
            textBoxSalt.Enabled = false;
            textBoxPepper.Enabled = false;
        }

        private void textBoxGenerate_Click(object sender, EventArgs e)
        {
            if (textBoxHashID.Text != "")
            {
                if (CheckHashID(textBoxHashID.Text))
                {
                    DialogResult = DialogResult.OK;
                }                
            }
            else
            {

                if (MessageBox.Show(Languages.Translate(414), Languages.Translate(10030), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                }
            }            
        }

        public void GetSaltPepperInformation(out bool generateSalt,  out int lenghtSalt, out string ownSalt, out bool generatePepper, out int lenghtPepper, out string ownPepper, out string hashID)
        {
            generateSalt = false;
            lenghtSalt = 0;
            ownSalt = "";
            generatePepper = false;
            lenghtPepper = 0;
            ownPepper = "";
            //salt
            if (groupBoxSalt.Enabled)
            {
                generateSalt = radioButtonSaltGen.Checked;
                if (generateSalt)
                {
                    lenghtSalt = int.Parse(textBoxSaltLenght.Text);
                    ownSalt = null;
                }
                else
                {
                    lenghtSalt = 0;
                    ownSalt = textBoxSalt.Text;
                    //Console.WriteLine("SaltAndPepperQuestion Form SALT: " + ownSalt);
                }
            }
            if (groupBoxPepper.Enabled)
            {
                //pepper
                generatePepper = radioButtonPepperGen.Checked;
                if (generatePepper)
                {
                    lenghtPepper = int.Parse(textBoxPepperLenght.Text);
                    ownPepper = null;
                }
                else
                {
                    lenghtPepper = 0;
                    ownPepper = textBoxPepper.Text;
                    //Console.WriteLine("SaltAndPepperQuestion Form Pepper: " + ownPepper);
                }
            }
            //hashID
            hashID = textBoxHashID.Text;            
        }
   
        #region HashID
        /// <summary>
        /// Trying to find a name for a hash that isnt already used
        /// </summary>
        /// <returns></returns>
        private string SetHashID()
        {
            string name = "automaticallyGeneratedHashID";
            int index = 1;
            while (true)
            {
                string path = Path.Combine(Settings.DirectoryToHashData, name + index + ".txt");
                if (!File.Exists(path))
                {
                    return name + index.ToString();
                }
                index++;
            }
        }

        private string SetHashID(string name)
        {
            //check if file exists
            string path = Path.Combine(Settings.DirectoryToHashData, name + ".txt");
            if (!File.Exists(path))
            {
                return name;
            }
            //automatically generate with index
            int index = 1;
            while (true)
            {
                path = Path.Combine(Settings.DirectoryToHashData, name + index + ".txt");
                if (!File.Exists(path))
                {
                    return name + index.ToString();
                }
                index++;
            }
        }

        private bool CheckHashID(string hashID)
        {
            string path = Path.Combine(Settings.DirectoryToHashData, hashID + ".txt");
            if (File.Exists(path))
            {                
                if (MessageBox.Show(Languages.Translate(401), Languages.Translate(10025), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            else return true;
        }
        #endregion

        #region RadioButtonsControls
        private void radioButtonPepperGen_CheckedChanged(object sender, EventArgs e)
        {
            PepperRadioButtonCheck();
        }

        private void radioButtonPepperOwn_CheckedChanged(object sender, EventArgs e)
        {
            PepperRadioButtonCheck();
        }


        //Salt
        private void radioButtonSaltGen_CheckedChanged(object sender, EventArgs e)
        {
            SaltRadioButtonCheck();
        }

        private void radioButtonSaltOwn_CheckedChanged(object sender, EventArgs e)
        {
            SaltRadioButtonCheck();
        }

        private void SaltRadioButtonCheck()
        {
            if (radioButtonSaltGen.Checked) //Generate Salt
            {
                textBoxSaltLenght.Enabled = true;
                textBoxSalt.Enabled = false;
            }
            else //Include own Salt
            {
                textBoxSaltLenght.Enabled = false;
                textBoxSalt.Enabled = true;
            }
        }

        private void PepperRadioButtonCheck()
        {
            if (radioButtonPepperGen.Checked) //Generate Salt
            {
                textBoxPepperLenght.Enabled = true;
                textBoxPepper.Enabled = false;
            }
            else //Include own Salt
            {
                textBoxPepperLenght.Enabled = false;
                textBoxPepper.Enabled = true;
            }
        }
        #endregion
    }
}
