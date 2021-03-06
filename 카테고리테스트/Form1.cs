﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 카테고리테스트.DataBaseManagement;

namespace 카테고리테스트
{
    public partial class Form1 : Form
    {
        DataBaseManager dbm;
        DataFormat total;
        public Form1()
        {
            InitializeComponent();
            dbm = new DataBaseManager();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if(dbm.SetDB(textBoxAddress.Text, textBoxPort.Text, textBoxID.Text, textBoxPW.Text))
            {
                MessageBox.Show("접속 성공");
            }
            else
            {
                MessageBox.Show("접속 실패");
            }
        }

        private void buttonGetCategory_Click(object sender, EventArgs e)
        {
            if (dbm.dbLoginInfo == null)
            {
                MessageBox.Show("로그인을 먼저 해 주세요");
                return;
            }
            dbm.GetCategoryStart();
            treeViewCategoryList.Nodes.Clear();

            //하나로 뭉치기
            total = new DataFormat(0, "카테고리", 0);
            foreach(DataFormat df in dbm.list)
            {
                total.childList.Add(df);
            }

            treeViewCategoryList.Nodes.Add(SetTreeView(total));
        }

        private TreeNode SetTreeView(DataFormat df)
        {
            TreeNode tmpNode = new TreeNode(string.Format("{0:000} | {1} | {2:000}",df.index,df.name,df.parentIndex));
            for(int i = 0; i < df.childList.Count; i++)
            {
                tmpNode.Nodes.Add(SetTreeView(df.childList[i]));
            }

            return tmpNode;
        }

        private void buttonExpand_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeViewCategoryList.Nodes[0].IsExpanded) treeViewCategoryList.CollapseAll();
                else treeViewCategoryList.ExpandAll();
            }
            catch { }
        }

        private StringBuilder JsonWriter(DataFormat format)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{ \"index\" : \""+format.index+"\"," +
                "\"name\" : \""+format.name+"\","+
                "\"parentIndex\" : \""+format.parentIndex+"\","+
                "\"childList\" : [");

            foreach(DataFormat df in format.childList)
            {
                sb.Append(JsonWriter(df));
                sb.Append("},");
            }

            if (format.childList.Count != 0) sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb;
        }

        private void buttonJsonWrite_Click(object sender, EventArgs e)
        {
            StringBuilder sb = JsonWriter(total);
            sb = sb.Append("}");
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("복사되었습니다. 아무데나 붙여넣기 하세여");
        }
    }
}
