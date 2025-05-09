﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace isMyLoginSecureDesktopDemo
{
    public partial class LicenceDialog : Form
    {
        public LicenceDialog()
        {
            InitializeComponent();

            textBoxLicence.Text = "IsMyLoginSecure\r\n" +
                                  "\r\n" +
                                  "Copyright 2017-2025 Software Verify Limited\r\n" +
                                  "\r\n" +
                                  "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/ or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\r\n" +
                                  "\r\n" +
                                  "The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\r\n" +
                                  "\r\n" +
                                  "THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.\r\n";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabelOpenSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabelOpenSource.LinkVisited = true;

            // Navigate to a URL.

            System.Diagnostics.Process.Start("https://opensource.org/license/MIT");
        }

        private void linkLabelisMyLoginSecure_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabelisMyLoginSecure.LinkVisited = true;

            // Navigate to a URL.

            System.Diagnostics.Process.Start("https://github.com/PipingSnail/ismyloginsecure");
        }
    }
}
