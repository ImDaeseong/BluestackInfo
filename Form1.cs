using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BluestackInfo
{
    public partial class Form1 : Form
    {
        clsBluestack objBluestack = clsBluestack.getInstance;

        public Form1()
        {
            InitializeComponent();

            InitListView();
        }

        private void InitListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Clickable;
            listView1.CheckBoxes = true;
            listView1.OwnerDraw = true;

            listView1.Columns.Add("", 25, HorizontalAlignment.Left);
            listView1.Columns.Add("게임명", 180, HorizontalAlignment.Left);
            listView1.Columns.Add("패키지명", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("액티비티명", 250, HorizontalAlignment.Left);
            listView1.Columns.Add("버전", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("앱스토어", 40, HorizontalAlignment.Left);
            listView1.Columns.Add("시스템", 40, HorizontalAlignment.Left);
            listView1.Columns.Add("이미지", 40, HorizontalAlignment.Left);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            objBluestack.GetBluestackGameInfo();

            int FullScreen = objBluestack.GetBluestackkey("FullScreen");
            int GuestWidth = objBluestack.GetBluestackkey("GuestWidth");
            int WindowWidth = objBluestack.GetBluestackkey("WindowWidth");
            int GuestHeight = objBluestack.GetBluestackkey("GuestHeight");
            int WindowHeight = objBluestack.GetBluestackkey("WindowHeight");
            int Memory = objBluestack.GetBluestackMemory();

            if (FullScreen == 1)
                chkFullScreen.Checked = true;
            else
                chkFullScreen.Checked = false;

            txtWindowWidth.Text = WindowWidth.ToString();
            txtWindowHeight.Text = WindowHeight.ToString();
            txtMemory.Text = Memory.ToString();


            for (int i = 0; i < objBluestack.BLUESTACK_ITEM.Count; i++)
            { 
                ListViewItem item = new ListViewItem();
                item.Text = "";
                item.SubItems.Add(objBluestack.BLUESTACK_ITEM[i].NAME);
                item.SubItems.Add(objBluestack.BLUESTACK_ITEM[i].PACKAGE);
                item.SubItems.Add(objBluestack.BLUESTACK_ITEM[i].ACTIVITY);
                item.SubItems.Add(objBluestack.BLUESTACK_ITEM[i].VERSION);
                item.SubItems.Add(objBluestack.BLUESTACK_ITEM[i].APPSTORE);
                item.SubItems.Add(objBluestack.BLUESTACK_ITEM[i].SYSTEM);
                item.SubItems.Add(objBluestack.BLUESTACK_ITEM[i].IMG);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FullScreen
            if(chkFullScreen.Checked)
                objBluestack.SetBluestackkey("FullScreen", 1);
            else
                objBluestack.SetBluestackkey("FullScreen", 0);
            
            //Width
            if (txtWindowWidth.Text != "")
            {
                int WindowWidth = Convert.ToInt32(txtWindowWidth.Text);                
                objBluestack.SetBluestackkey("WindowWidth", WindowWidth);                
            }

            //Height
            if (txtWindowHeight.Text != "")
            {
                int WindowHeight = Convert.ToInt32(txtWindowHeight.Text);
                objBluestack.SetBluestackkey("WindowHeight", WindowHeight);
            }
            
            //memory
            if (txtMemory.Text != "")
            {
                int Memory = Convert.ToInt32(txtMemory.Text);
                objBluestack.SetBluestackMemory(Memory);
            }

            //objBluestack.SetBluestackkey("GuestWidth", GuestWidth);
            //objBluestack.SetBluestackkey("GuestHeight", GuestHeight);
        }

        private void InitChkBox()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
                listView1.Items[i].Checked = false;
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(e.Header.Tag);
                }
                catch (Exception)
                {
                }
                CheckBoxRenderer.DrawCheckBox(e.Graphics,
                    new Point(e.Bounds.Left + 4, e.Bounds.Top + 4),
                    value ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
                    System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listView1.SelectedItems.Count > 0)
            //{
            //    string gamename = listView1.SelectedItems[0].SubItems[1].Text.ToString();
            //    string pkgname = listView1.SelectedItems[0].SubItems[2].Text.ToString();
            //    string activename = listView1.SelectedItems[0].SubItems[3].Text.ToString();
            //    string version = listView1.SelectedItems[0].SubItems[4].Text.ToString();
            //    string appstore = listView1.SelectedItems[0].SubItems[5].Text.ToString();
            //    string system = listView1.SelectedItems[0].SubItems[6].Text.ToString();
            //    string image = listView1.SelectedItems[0].SubItems[7].Text.ToString();
            //    objBluestack.HDRunAPP(pkgname, activename);
            //}
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string gamename = listView1.SelectedItems[0].SubItems[1].Text.ToString();
                string pkgname = listView1.SelectedItems[0].SubItems[2].Text.ToString();
                string activename = listView1.SelectedItems[0].SubItems[3].Text.ToString();
                string version = listView1.SelectedItems[0].SubItems[4].Text.ToString();
                string appstore = listView1.SelectedItems[0].SubItems[5].Text.ToString();
                string system = listView1.SelectedItems[0].SubItems[6].Text.ToString();
                string image = listView1.SelectedItems[0].SubItems[7].Text.ToString();
                objBluestack.HDRunAPP(pkgname, activename);
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(listView1.Columns[e.Column].Tag);
                }
                catch (Exception)
                {
                }
                listView1.Columns[e.Column].Tag = !value;
                foreach (ListViewItem item in listView1.Items)
                    item.Checked = !value;

                listView1.Invalidate();
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }
    }
}
