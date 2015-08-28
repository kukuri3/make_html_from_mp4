using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int gFileNum;
        string gSrcPath="F:\\tv\\mp4";
        string gDstPath = "G:\\mp4";

        public Form1()
        {
            InitializeComponent();
        }
        //===============================================================================================
        private void xLog(String s)
        {
            //ログ出力
            DateTime dt = DateTime.Now;
            textBox2.AppendText(dt.ToString() + " " + s + "\n");
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@"log.txt", true, System.Text.Encoding.GetEncoding("shift_jis"));
            sw.Write(dt.ToString() + s + "\r\n");
            sw.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //リストの取得
            xGetList(textBox1.Text);
        }

        private void xGetList(string pathstring)
        {
            //ファイルリストの取得
            xLog("xGetList("+pathstring+")");

            //フォルダ内のファイル一覧を取得
            IEnumerable<string> files = Directory.EnumerateFiles(pathstring, "*.*", SearchOption.TopDirectoryOnly);
            //datagridへのデータ追加
            gFileNum = 0;
            dataGridView2.Rows.Clear();
            foreach (string file in files)
            {
                //xLog(file + "\n");
                DateTime dtUpdate = System.IO.File.GetLastWriteTime(file);
                dataGridView2.Rows.Add(file, Path.GetDirectoryName(file), Path.GetFileName(file), dtUpdate.ToString("yyyyMMdd_HHmmss"));
                gFileNum++;

            }
            //label7.Text = gFileNum.ToString();
            //ファイルが他のプロセスでオープンされているかチェック
            for (int i = 0; i < gFileNum; i++)
            {
                string fn = dataGridView2[0, i].Value.ToString();
                if (xOpenable(fn) == true)
                {
                    dataGridView2[4, i].Value = "o"; //オープン可能のフラグ=o
                    string fn2 = fn.Replace("#", "＃");  //ファイル名中のヤバい文字の置き換え

 //                   xLog("src=" + srcfn + "\r\ndst=" + dstfn + "\r\n");
                    if (!System.IO.File.Exists(fn2))
                    {
                        System.IO.File.Move(fn, fn2);   //ファイルをリネーム
                        /*
                        string dstdir = Path.GetDirectoryName(fn2);
                        string dstfnwithoutext = Path.GetFileNameWithoutExtension(fn2);   //ファイル名
                        string dstext = Path.GetExtension(fn2); //拡張子
                        DateTime now = DateTime.Now;
                        string newfn = dstdir + "\\" + dstfnwithoutext + now.ToString("yyyyMMdd_HHmmss_ffff") + dstext;
                        xLog(fn2 + "はすでにあるので、ファイル名を" + newfn + "に変更します。\r\n");
                        fn2 = newfn;
                    */
                    }
 


 
                }
                else
                {
                    dataGridView2[4, i].Value = "x";
                }
            }


            dataGridView2.Sort(dataGridView2.Columns["dt"], ListSortDirection.Ascending);   //ソートを行う
            dataGridView2.Refresh();
            label1.Text = gFileNum.ToString();
            label1.Refresh();
            //xLog("gFileNum," + gFileNum);

        }

        private bool xOpenable(string f)
        {
            //fが書き込み可能か調べる
            bool b;

            b = true;
            try
            {
                FileStream fs = new FileStream(f, FileMode.Open, FileAccess.Write);
                fs.Close();
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }


        private void xMakeHtml2()
        {

            dataGridView2.Sort(dataGridView2.Columns["fn"], ListSortDirection.Ascending);   //ソートを行う
            dataGridView2.Refresh();

            textBox3.ResetText();   //クリア
            textBox3.AppendText("<html>\r\n");
            textBox3.AppendText("<head>\r\n");
            textBox3.AppendText("<title>ムービー倉庫テスト中</title>\r\n");
            textBox3.AppendText("ムービー倉庫テスト中<br><br>\r\n");
            textBox3.AppendText("<a href=\"movie1.html\">日付順はこちら</a href><br><br><br>\r\n");

            xWriteFileHtml();
        }
        private void xMakeHtml()
        {
            dataGridView2.Sort(dataGridView2.Columns["dt"], ListSortDirection.Descending);   //ソートを行う
            dataGridView2.Refresh();

            textBox3.ResetText();   //クリア
             textBox3.AppendText("<html>\r\n");
            textBox3.AppendText("<head>\r\n");
            textBox3.AppendText("<title>ムービー倉庫テスト中 </title>\r\n");
            textBox3.AppendText("<a name=\"#top\">\r\n");
            textBox3.AppendText("ムービー倉庫テスト中<br><br>\r\n");
            textBox3.AppendText("<a href=\"movie2.html\">名前順はこちら</a href><br><br><br>\r\n");

            xWriteFileHtml();

        }

        private void xMakeHtml3()
        {
            dataGridView2.Sort(dataGridView2.Columns["dt"], ListSortDirection.Descending);   //ソートを行う
            dataGridView2.Refresh();

            textBox3.ResetText();   //クリア
            textBox3.AppendText("<html>\r\n");
            textBox3.AppendText("<head>\r\n");
            textBox3.AppendText("<title>ムービー倉庫テスト中 </title>\r\n");
            textBox3.AppendText("<a name=\"#top\">\r\n");
            textBox3.AppendText("ムービー倉庫テスト中<br><br>\r\n");
            textBox3.AppendText("<a href=\"movie2.html\">名前順はこちら</a href><br><br><br>\r\n");

            xWriteFileHtml3();

        }


        private void xWriteFileHtml()
        {
            //ファイルリストをHTMLで書き出す
            DateTime dt = DateTime.Now;
            System.IO.DriveInfo cdrive = new System.IO.DriveInfo("F");
            System.IO.DriveInfo ddrive = new System.IO.DriveInfo("G");

            textBox3.AppendText("全部で" + gFileNum + "項目(" + (cdrive.TotalSize - cdrive.TotalFreeSpace) / 1073741824 + "GB)あります<br><br>");
            textBox3.AppendText(dt.ToString() + " 更新<br><br>\r\n");
            textBox3.AppendText("ts ドライブ 全容量=" + ddrive.TotalSize / 1073741824 + "GB 使用=" + (ddrive.TotalSize - ddrive.TotalFreeSpace) / 1073741824 + "GB 残り=" + ddrive.TotalFreeSpace / 1073741824 + "GB<br>");
            textBox3.AppendText("mp4ドライブ 全容量=" + cdrive.TotalSize / 1073741824 + "GB 使用=" + (cdrive.TotalSize - cdrive.TotalFreeSpace) / 1073741824 + "GB 残り=" + cdrive.TotalFreeSpace / 1073741824 + "GB<br><br>");
            for (int i = 0; i < gFileNum; i++)
            {
                //ページ内アンカーの作成
                int mm = 1000;  //何個ごとにアンカーをつけるか
                int a = i % mm;
                int b = (int)Math.Floor((double)i / mm);
                int c = (int)Math.Floor((double)gFileNum / mm);
                if (a == 0)
                {
                    textBox3.AppendText("<br><a name=\"" + b + "\">\r\n");  //ページ内アンカー
                    textBox3.AppendText("<a href=\"#top\">[TOP]</a>\r\n");  //TOPへのリンク
                    for (int j = 0; j < c+1; j++)
                    {
                        textBox3.AppendText("<a href=\"#" + j + "\">[" + j * mm + "]</a>");   //ページ内アンカーへのリンク
                    }
                    textBox3.AppendText("<br><br>\r\n");
                }

                //ファイル名
                textBox3.AppendText(i.ToString("D5") + " <a href=\"./");
                textBox3.AppendText(dataGridView2["fn", i].Value.ToString());
                textBox3.AppendText("\">" + dataGridView2["fn", i].Value.ToString() + "</a><br>\r\n");

            }
            textBox3.AppendText("<br>");

        }

        private void xWriteFileHtml3()
        {
            //ファイルリストをHTMLで書き出す
            DateTime dt = DateTime.Now;
            System.IO.DriveInfo cdrive = new System.IO.DriveInfo("F");
            System.IO.DriveInfo ddrive = new System.IO.DriveInfo("G");

            textBox3.AppendText("全部で" + gFileNum + "項目(" + (cdrive.TotalSize - cdrive.TotalFreeSpace) / 1073741824 + "GB)あります<br><br>");
            textBox3.AppendText(dt.ToString() + " 更新<br><br>\r\n");
            textBox3.AppendText("ts ドライブ 全容量=" + ddrive.TotalSize / 1073741824 + "GB 使用=" + (ddrive.TotalSize - ddrive.TotalFreeSpace) / 1073741824 + "GB 残り=" + ddrive.TotalFreeSpace / 1073741824 + "GB<br>");
            textBox3.AppendText("mp4ドライブ 全容量=" + cdrive.TotalSize / 1073741824 + "GB 使用=" + (cdrive.TotalSize - cdrive.TotalFreeSpace) / 1073741824 + "GB 残り=" + cdrive.TotalFreeSpace / 1073741824 + "GB<br><br>");
            for (int i = 0; i < gFileNum; i++)
            {
                //ページ内アンカーの作成
                int mm = 1000;  //何個ごとにアンカーをつけるか
                int a = i % mm;
                int b = (int)Math.Floor((double)i / mm);
                int c = (int)Math.Floor((double)gFileNum / mm);
                if (a == 0)
                {
                    textBox3.AppendText("<br><a name=\"" + b + "\">\r\n");  //ページ内アンカー
                    textBox3.AppendText("<a href=\"#top\">[TOP]</a>\r\n");  //TOPへのリンク
                    for (int j = 0; j < c + 1; j++)
                    {
                        textBox3.AppendText("<a href=\"#" + j + "\">[" + j * mm + "]</a>");   //ページ内アンカーへのリンク
                    }
                    textBox3.AppendText("<br><br>\r\n");
                }

                //ファイル名
                string mp3fn = dataGridView2["fn", i].Value.ToString();
                string fn_ext=System.IO.Path.GetExtension(mp3fn);
                if (fn_ext.Contains("mp3"))
                {
                    textBox3.AppendText(i.ToString("D5") + " <a href=\"./");
                    textBox3.AppendText(dataGridView2["fn", i].Value.ToString());
                    textBox3.AppendText("\">" + dataGridView2["fn", i].Value.ToString() + "</a><br>\r\n");
                }
            }
            textBox3.AppendText("<br>");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            xMoveAndMakeHtml();

        }
        private void xMoveAndMakeHtml()
        {
            //ファイルのmove
            string srcpath = textBox1.Text;
            string dstpath = textBox4.Text;

            if (checkBox1.Checked == true)
            {
                xGetList(textBox1.Text);
                xGetList(textBox1.Text);    //ファイル名の書き換え（#など)があったとき反映させるため


                for (int i = 0; i < gFileNum; i++)
                {
                    if (dataGridView2["openable", i].Value.ToString().Equals("o"))
                    {
                        string fn = dataGridView2["fn", i].Value.ToString();    //対象のファイル名のみ
                        string srcfn = dataGridView2["path", i].Value.ToString();
                        string dstfn = dstpath + "\\" + fn;
                        xLog("src=" + srcfn + "\r\ndst=" + dstfn + "\r\n");
                        if (!System.IO.File.Exists(dstfn))
                        {
                            /*string dstdir = Path.GetDirectoryName(dstfn);
                            string dstfnwithoutext = Path.GetFileNameWithoutExtension(dstfn);   //ファイル名
                            string dstext = Path.GetExtension(dstfn); //拡張子
                            DateTime now = DateTime.Now;
                            string newfn = dstdir + "\\" + dstfnwithoutext + now.ToString("yyyyMMdd_hhmmss_ffff") + dstext;
                            xLog(dstfn + "はすでにあるので、ファイル名を" + newfn + "に変更します。\r\n");
                            dstfn = newfn;*/
                            System.IO.File.Move(srcfn, dstfn);  //ファイルの移動

                        }

                    }
                }
            }
            xGetList(textBox4.Text);

            //HTMLを作る
            xMakeHtml();
            xSaveHtml("\\movie1.html");
            xMakeHtml2();
            xSaveHtml("\\movie2.html");
            xMakeHtml3();
            xSaveHtml("\\mp3.html");  
        }

        private void xSaveHtml(string s)
        {
            string filepathname=textBox4.Text+s;
            try
            {
                if (System.IO.File.Exists(filepathname)) System.IO.File.Delete(filepathname);  //ファイルがあったら消す
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filepathname, true, System.Text.Encoding.GetEncoding("shift_jis"));
                sw.Write(textBox3.Text);
                sw.Close();
            }
            catch (Exception ex)
            {
                xLog(ex.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //
            xMoveAndMakeHtml();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            xLog("タイマーをスタートしました。\r\n");
            //xLog("動作間隔="+timer1.Tick.toString()+"ms\r\n");

            textBox1.Text = gSrcPath;
            textBox4.Text = gDstPath;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            xGetList(textBox4.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //src pathの設定
            //set mp4 path
            //フォルダの指定
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "フォルダを選んでください";

            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
               // Properties.Settings.Default.srcpath = fbd.SelectedPath;


            }
            //Properties.Settings.Default.Save();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //dst pathの設定
            //フォルダの指定
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "フォルダを選んでください";

            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                textBox4.Text = fbd.SelectedPath;
                //Properties.Settings.Default.dstpath = fbd.SelectedPath;


            }
            //Properties.Settings.Default.Save();
        }


 
    }
}
