// Implementation: Stephen Kellett 28 December 2017..10 January 2018 and March/April 2025
// Copyright (c) Software Verify, IsMyLoginSecure 2017-2025.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// The above licence is the MIT Licence. https://opensource.org/license/MIT

using System.Windows.Forms;

namespace isMyLoginSecureDesktopDemo
{
    partial class MainGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGUI));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAsHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAsXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userAgentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadURLsFromAFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutIsMyLoginSecureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.softwareLicenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxSecurityHeader = new System.Windows.Forms.TextBox();
            this.labelSecurityHeader = new System.Windows.Forms.Label();
            this.labelSecurityCertificate = new System.Windows.Forms.Label();
            this.textBoxSecurityCertificate = new System.Windows.Forms.TextBox();
            this.labelInsecureContentURLs = new System.Windows.Forms.Label();
            this.textBoxMixedContentURLs = new System.Windows.Forms.TextBox();
            this.urlProgressBar = new System.Windows.Forms.ProgressBar();
            this.timerGraphicsRefresh = new System.Windows.Forms.Timer(this.components);
            this.listViewResults = new isMyLoginSecureDesktopDemo.URLListView();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelBottomLeft = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelBottomMiddle = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelBottomRight = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            this.tableLayoutPanelBottom.SuspendLayout();
            this.tableLayoutPanelBottomLeft.SuspendLayout();
            this.tableLayoutPanelBottomMiddle.SuspendLayout();
            this.tableLayoutPanelBottomRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.userAgentToolStripMenuItem,
            this.testToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1124, 24);
            this.menuStrip.TabIndex = 8;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearResultsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem4,
            this.exportAsHTMLToolStripMenuItem,
            this.exportAsXMLToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // clearResultsToolStripMenuItem
            // 
            this.clearResultsToolStripMenuItem.Name = "clearResultsToolStripMenuItem";
            this.clearResultsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.clearResultsToolStripMenuItem.Text = "&Clear Results";
            this.clearResultsToolStripMenuItem.Click += new System.EventHandler(this.clearResultsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(163, 6);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadToolStripMenuItem.Text = "&Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.saveToolStripMenuItem.Text = "&Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(163, 6);
            // 
            // exportAsHTMLToolStripMenuItem
            // 
            this.exportAsHTMLToolStripMenuItem.Name = "exportAsHTMLToolStripMenuItem";
            this.exportAsHTMLToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exportAsHTMLToolStripMenuItem.Text = "Export as &HTML...";
            this.exportAsHTMLToolStripMenuItem.Click += new System.EventHandler(this.exportAsHTMLToolStripMenuItem_Click);
            // 
            // exportAsXMLToolStripMenuItem
            // 
            this.exportAsXMLToolStripMenuItem.Name = "exportAsXMLToolStripMenuItem";
            this.exportAsXMLToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exportAsXMLToolStripMenuItem.Text = "Export as &XML...";
            this.exportAsXMLToolStripMenuItem.Click += new System.EventHandler(this.exportAsXMLToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(163, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // userAgentToolStripMenuItem
            // 
            this.userAgentToolStripMenuItem.Name = "userAgentToolStripMenuItem";
            this.userAgentToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.userAgentToolStripMenuItem.Text = "&User Agent";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aURLToolStripMenuItem,
            this.loadURLsFromAFileToolStripMenuItem,
            this.toolStripMenuItem2});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.testToolStripMenuItem.Text = "&Test";
            // 
            // aURLToolStripMenuItem
            // 
            this.aURLToolStripMenuItem.Name = "aURLToolStripMenuItem";
            this.aURLToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.aURLToolStripMenuItem.Text = "A URL...";
            this.aURLToolStripMenuItem.Click += new System.EventHandler(this.aURLToolStripMenuItem_Click);
            // 
            // loadURLsFromAFileToolStripMenuItem
            // 
            this.loadURLsFromAFileToolStripMenuItem.Name = "loadURLsFromAFileToolStripMenuItem";
            this.loadURLsFromAFileToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.loadURLsFromAFileToolStripMenuItem.Text = "Load URLs from a file...";
            this.loadURLsFromAFileToolStripMenuItem.Click += new System.EventHandler(this.loadURLsFromAFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(192, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutIsMyLoginSecureToolStripMenuItem,
            this.helpManualToolStripMenuItem,
            this.softwareLicenceToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutIsMyLoginSecureToolStripMenuItem
            // 
            this.aboutIsMyLoginSecureToolStripMenuItem.Name = "aboutIsMyLoginSecureToolStripMenuItem";
            this.aboutIsMyLoginSecureToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.aboutIsMyLoginSecureToolStripMenuItem.Text = "About Is My Login Secure...";
            this.aboutIsMyLoginSecureToolStripMenuItem.Click += new System.EventHandler(this.aboutIsMyLoginSecureToolStripMenuItem_Click);
            // 
            // helpManualToolStripMenuItem
            // 
            this.helpManualToolStripMenuItem.Name = "helpManualToolStripMenuItem";
            this.helpManualToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.helpManualToolStripMenuItem.Text = "&Help Manual...";
            this.helpManualToolStripMenuItem.Click += new System.EventHandler(this.helpManualToolStripMenuItem_Click);
            // 
            // softwareLicenceToolStripMenuItem
            // 
            this.softwareLicenceToolStripMenuItem.Name = "softwareLicenceToolStripMenuItem";
            this.softwareLicenceToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.softwareLicenceToolStripMenuItem.Text = "Software Licence...";
            this.softwareLicenceToolStripMenuItem.Click += new System.EventHandler(this.softwareLicenceToolStripMenuItem_Click);
            // 
            // textBoxSecurityHeader
            // 
            this.textBoxSecurityHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSecurityHeader.Location = new System.Drawing.Point(2, 22);
            this.textBoxSecurityHeader.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSecurityHeader.Multiline = true;
            this.textBoxSecurityHeader.Name = "textBoxSecurityHeader";
            this.textBoxSecurityHeader.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSecurityHeader.Size = new System.Drawing.Size(362, 163);
            this.textBoxSecurityHeader.TabIndex = 4;
            // 
            // labelSecurityHeader
            // 
            this.labelSecurityHeader.AutoSize = true;
            this.labelSecurityHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSecurityHeader.Location = new System.Drawing.Point(2, 0);
            this.labelSecurityHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSecurityHeader.Name = "labelSecurityHeader";
            this.labelSecurityHeader.Size = new System.Drawing.Size(362, 13);
            this.labelSecurityHeader.TabIndex = 10;
            this.labelSecurityHeader.Text = "Security Header";
            // 
            // labelSecurityCertificate
            // 
            this.labelSecurityCertificate.AutoSize = true;
            this.labelSecurityCertificate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSecurityCertificate.Location = new System.Drawing.Point(2, 0);
            this.labelSecurityCertificate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSecurityCertificate.Name = "labelSecurityCertificate";
            this.labelSecurityCertificate.Size = new System.Drawing.Size(362, 13);
            this.labelSecurityCertificate.TabIndex = 12;
            this.labelSecurityCertificate.Text = "Security Certificate";
            // 
            // textBoxSecurityCertificate
            // 
            this.textBoxSecurityCertificate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSecurityCertificate.Location = new System.Drawing.Point(2, 22);
            this.textBoxSecurityCertificate.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSecurityCertificate.Multiline = true;
            this.textBoxSecurityCertificate.Name = "textBoxSecurityCertificate";
            this.textBoxSecurityCertificate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSecurityCertificate.Size = new System.Drawing.Size(362, 163);
            this.textBoxSecurityCertificate.TabIndex = 5;
            // 
            // labelInsecureContentURLs
            // 
            this.labelInsecureContentURLs.AutoSize = true;
            this.labelInsecureContentURLs.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInsecureContentURLs.Location = new System.Drawing.Point(2, 0);
            this.labelInsecureContentURLs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInsecureContentURLs.Name = "labelInsecureContentURLs";
            this.labelInsecureContentURLs.Size = new System.Drawing.Size(364, 13);
            this.labelInsecureContentURLs.TabIndex = 14;
            this.labelInsecureContentURLs.Text = "Insecure content URLs";
            // 
            // textBoxMixedContentURLs
            // 
            this.textBoxMixedContentURLs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMixedContentURLs.Location = new System.Drawing.Point(2, 22);
            this.textBoxMixedContentURLs.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMixedContentURLs.Multiline = true;
            this.textBoxMixedContentURLs.Name = "textBoxMixedContentURLs";
            this.textBoxMixedContentURLs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMixedContentURLs.Size = new System.Drawing.Size(364, 163);
            this.textBoxMixedContentURLs.TabIndex = 6;
            // 
            // urlProgressBar
            // 
            this.urlProgressBar.Location = new System.Drawing.Point(204, 1);
            this.urlProgressBar.Maximum = 5;
            this.urlProgressBar.Name = "urlProgressBar";
            this.urlProgressBar.Size = new System.Drawing.Size(912, 23);
            this.urlProgressBar.Step = 0;
            this.urlProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.urlProgressBar.TabIndex = 15;
            // 
            // listViewResults
            // 
            this.listViewResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(3, 3);
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.OwnerDraw = true;
            this.listViewResults.Size = new System.Drawing.Size(1118, 272);
            this.listViewResults.TabIndex = 3;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.AutoSize = true;
            this.tableLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelTop.ColumnCount = 1;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.Controls.Add(this.listViewResults, 0, 0);
            this.tableLayoutPanelTop.Controls.Add(this.tableLayoutPanelBottom, 0, 1);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 2;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.33333F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(1124, 477);
            this.tableLayoutPanelTop.TabIndex = 16;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.ColumnCount = 3;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelBottom.Controls.Add(this.tableLayoutPanelBottomLeft, 0, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.tableLayoutPanelBottomMiddle, 1, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.tableLayoutPanelBottomRight, 2, 0);
            this.tableLayoutPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(3, 281);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.RowCount = 1;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(1118, 193);
            this.tableLayoutPanelBottom.TabIndex = 4;
            // 
            // tableLayoutPanelBottomLeft
            // 
            this.tableLayoutPanelBottomLeft.ColumnCount = 1;
            this.tableLayoutPanelBottomLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBottomLeft.Controls.Add(this.labelSecurityHeader, 0, 0);
            this.tableLayoutPanelBottomLeft.Controls.Add(this.textBoxSecurityHeader, 0, 1);
            this.tableLayoutPanelBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBottomLeft.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelBottomLeft.Name = "tableLayoutPanelBottomLeft";
            this.tableLayoutPanelBottomLeft.RowCount = 2;
            this.tableLayoutPanelBottomLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBottomLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBottomLeft.Size = new System.Drawing.Size(366, 187);
            this.tableLayoutPanelBottomLeft.TabIndex = 0;
            // 
            // tableLayoutPanelBottomMiddle
            // 
            this.tableLayoutPanelBottomMiddle.ColumnCount = 1;
            this.tableLayoutPanelBottomMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBottomMiddle.Controls.Add(this.labelSecurityCertificate, 0, 0);
            this.tableLayoutPanelBottomMiddle.Controls.Add(this.textBoxSecurityCertificate, 0, 1);
            this.tableLayoutPanelBottomMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBottomMiddle.Location = new System.Drawing.Point(375, 3);
            this.tableLayoutPanelBottomMiddle.Name = "tableLayoutPanelBottomMiddle";
            this.tableLayoutPanelBottomMiddle.RowCount = 2;
            this.tableLayoutPanelBottomMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBottomMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBottomMiddle.Size = new System.Drawing.Size(366, 187);
            this.tableLayoutPanelBottomMiddle.TabIndex = 1;
            // 
            // tableLayoutPanelBottomRight
            // 
            this.tableLayoutPanelBottomRight.ColumnCount = 1;
            this.tableLayoutPanelBottomRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBottomRight.Controls.Add(this.labelInsecureContentURLs, 0, 0);
            this.tableLayoutPanelBottomRight.Controls.Add(this.textBoxMixedContentURLs, 0, 1);
            this.tableLayoutPanelBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBottomRight.Location = new System.Drawing.Point(747, 3);
            this.tableLayoutPanelBottomRight.Name = "tableLayoutPanelBottomRight";
            this.tableLayoutPanelBottomRight.RowCount = 2;
            this.tableLayoutPanelBottomRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBottomRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBottomRight.Size = new System.Drawing.Size(368, 187);
            this.tableLayoutPanelBottomRight.TabIndex = 2;
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1124, 501);
            this.Controls.Add(this.tableLayoutPanelTop);
            this.Controls.Add(this.urlProgressBar);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainGUI";
            this.Text = "Is My Login Secure";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelBottom.ResumeLayout(false);
            this.tableLayoutPanelBottomLeft.ResumeLayout(false);
            this.tableLayoutPanelBottomLeft.PerformLayout();
            this.tableLayoutPanelBottomMiddle.ResumeLayout(false);
            this.tableLayoutPanelBottomMiddle.PerformLayout();
            this.tableLayoutPanelBottomRight.ResumeLayout(false);
            this.tableLayoutPanelBottomRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //private System.Windows.Forms.ListView listViewResults;
        private URLListView listViewResults;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem userAgentToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem aURLToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem clearResultsToolStripMenuItem;
        private ToolStripMenuItem exportAsHTMLToolStripMenuItem;
        private ToolStripMenuItem exportAsXMLToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem1;
        private TextBox textBoxSecurityHeader;
        private Label labelSecurityHeader;
        private Label labelSecurityCertificate;
        private TextBox textBoxSecurityCertificate;
        private Label labelInsecureContentURLs;
        private TextBox textBoxMixedContentURLs;
        private ProgressBar urlProgressBar;
        private Timer timerGraphicsRefresh;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem loadURLsFromAFileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem helpManualToolStripMenuItem;
        private ToolStripMenuItem aboutIsMyLoginSecureToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanelTop;
        private TableLayoutPanel tableLayoutPanelBottom;
        private TableLayoutPanel tableLayoutPanelBottomLeft;
        private TableLayoutPanel tableLayoutPanelBottomMiddle;
        private TableLayoutPanel tableLayoutPanelBottomRight;
        private ToolStripMenuItem softwareLicenceToolStripMenuItem;
    }
}

