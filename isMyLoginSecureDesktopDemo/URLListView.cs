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
using System.Drawing;
using isMyLoginSecure;

namespace isMyLoginSecureDesktopDemo
{
    class URLListView : System.Windows.Forms.ListView
    {
        /// <summary>
        /// Owner drawn list view. Double buffered to reduce flickering.
        /// </summary>
        public URLListView()
        {
            this.OwnerDraw = true;
            this.DoubleBuffered = true;
            this.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(MyListView_DrawColumnHeader);
            this.DrawItem += new DrawListViewItemEventHandler(MyListView_DrawItem);
            this.DrawSubItem += new DrawListViewSubItemEventHandler(MyListView_DrawSubItem);

            this.FullRowSelect = true;
        }

        /// <summary>
        /// Helper method for owner drawn list view
        /// </summary>
        /// <param name="sender">The list view control sending this event.</param>
        /// <param name="e">The event args.</param>
        private void MyListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Not interested in changing the way columns are drawn - this works fine

            e.DrawDefault = true;
        }

        /// <summary>
        /// Helper method for owner drawn list view
        /// </summary>
        /// <param name="sender">The list view control sending this event.</param>
        /// <param name="e">The event args.</param>
        private void MyListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Helper method for owner drawn list view. 
        /// This draws each item in the control with appropriate colour coding.
        /// </summary>
        /// <param name="sender">The list view control sending this event.</param>
        /// <param name="e">The event args.</param>
        private void MyListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            websiteStatus ws = (websiteStatus)e.Item.Tag;

            if (ws.getIsQueryingURL())
                e.SubItem.ForeColor = Color.Blue;
            else if (ws.getFetchFailed() && !ws.getHasBadCertificate())
                e.SubItem.ForeColor = Color.Purple;
            else if (!ws.getIsSecure())
                e.SubItem.ForeColor = Color.Red;
            else
                e.SubItem.ForeColor = Color.Black;

            switch (e.ColumnIndex)
            {
                case 0: // ID
                    e.SubItem.Text = ws.getID().ToString();
                    break;

                case 1: // Name
                    e.SubItem.Text = ws.getBusinessName();
                    break;

                case 2: // Transport
                    e.SubItem.Text = ws.getStatusAsString();
                    e.SubItem.Text += " ";
                    e.SubItem.Text += ws.getSecurityProtocolQueryAsString();
                    break;

                case 3: // Secure
                    // we do this because some failed fetches also have bad security certificates

                    if (!ws.getIsSecure())
                    {
                        e.SubItem.Text = ws.getIsSecureAsString();
                    }
                    else
                    {
                        if (ws.getFetchFailed())
                            e.SubItem.Text = "???";
                        else
                            e.SubItem.Text = ws.getIsSecureAsString();
                    }
                    break;

                case 4: // Protocol
                    e.SubItem.Text = ws.getSecurityProtocolAsString();
                    break;

                case 5: // Redirect to https
                    e.SubItem.Text = ws.getHttpRedirectsToHttpsAsString();
                    break;

                case 6: // security certificate
                    e.SubItem.Text = ws.getSSLPolicyErrorStatusAsString();
                    break;

                case 7: // secure content
                    e.SubItem.Text = ws.getSecureContentAsString();
                    break;

                case 8: // security headers
                    e.SubItem.Text = ws.getSecurityHeaderGrade();
                    break;

                case 9: // test URL
                    e.SubItem.Text = ws.getURL();
                    break;

                case 10: // Actual URL
                    e.SubItem.Text = ws.getResponseURL();
                    break;

                case 11: // User Agent
                    e.SubItem.Text = ws.getUserFriendlyUserAgent();
                    break;

                case 12: // User Agent String
                    e.SubItem.Text = ws.getUserAgent();
                    break;
            }

            e.DrawText();
        }

        // https://stackoverflow.com/questions/14133225/listview-autoresizecolumns-based-on-both-column-content-and-header
        public void AutoResizeBothColumns()
        {
            for (int i = 1; i < Columns.Count; i++)
            { 
                AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

                int colWidth = TextRenderer.MeasureText(Columns[i].Text, Font).Width + 10;
                if (colWidth > Columns[i].Width)
                {
                    Columns[i].Width = colWidth;
                }
            }
        }
    }
}
