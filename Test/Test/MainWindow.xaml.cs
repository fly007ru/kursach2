using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hap = HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Xml;
using System.Xml.XPath;
using System.Data;


namespace Test
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            return;
            string parentsite = "http://www.umk3plus.utmn.ru/";
            txb.Text = "work";
            //MessageBox.Show("Start");
            string url = //"http://www.umk3plus.utmn.ru/?section=speciality&id=645"; 
                "http://www.umk3plus.utmn.ru/?section=faculty&id=162";
            //"http://www.umk3plus.utmn.ru/?section=faculty&id=169";
            var web = new Hap.HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.Default,
            };
            //MessageBox.Show("Loadstring");
            Hap.HtmlDocument doc = web.Load(url);
            //MessageBox.Show("LoadDOM");
            var content = doc.DocumentNode.SelectNodes("//table[@class='text']").ToArray()[1];
            if (content == null) MessageBox.Show("null");
            //"//a[@class='menu']").First(); //doc.DocumentNode.SelectNodes("/html/head/title").First(); //doc.DocumentNode.SelectNodes("//h3").First();
            int i = 0, j = 5, k = 0;
            string urlspec = "";
            Hap.HtmlDocument docspec;
            Directory.CreateDirectory("ИМиКН");
            foreach (var node in content.ChildNodes)
            {
                //txb.Text += n.InnerHtml;
                if (i > 2 && i % 2 == 1 && node.ChildNodes.Count > 5)
                {
                    XElement xspec = new XElement("info", null);

                    xspec.SetAttributeValue("name", node.ChildNodes[5 - k].InnerText);
                    xspec.SetAttributeValue("form", node.ChildNodes[7 - k].InnerText);
                    xspec.SetAttributeValue("URL", parentsite + node.ChildNodes[9 - k].ChildNodes[0].Attributes["href"].Value);

                    string s = xspec.ToString();
                    //x.SetAttributeNode("version", "1.0");
                    //txb.Text += "Специальность " + n.ChildNodes[5 - k].InnerText;
                    //txb.Text += "Форма обучения " + n.ChildNodes[7 - k].InnerText;
                    //txb.Text += "URL " + parentsite + n.ChildNodes[9 - k].ChildNodes[0].Attributes["href"].Value + " \n";
                    urlspec = parentsite + node.ChildNodes[9 - k].ChildNodes[0].Attributes["href"].Value;
                    docspec = web.Load(urlspec);
                    string zapros = "//table[@cellspacing='4']/tbody";
                    bool ok = true;
                    Hap.HtmlNode[] h2 = null;

                    try
                    {
                        h2 = docspec.DocumentNode.SelectNodes("//table[@cellspacing='4']/tr[2]/td[2]/table").ToArray();
                    }
                    catch { ok = false; }
                    if (ok)
                    {
                        string nameprof = "";
                        for (int i1 = 0; i1 < h2.Count(); i1++)
                        {
                            nameprof = "";
                            int semestr = -1;
                            XElement xsem = new XElement("semestr", null);
                            //txb.Text += h2[i1].InnerHtml + "\n Новое \n";
                            for (int i2 = 3; i2 < h2[i1].ChildNodes.Count; i2 += 2)
                            {
                                if (h2[i1].ChildNodes[i2].ChildNodes.Count > 5)
                                {

                                    if (semestr == -1)
                                    {
                                        semestr = Convert.ToInt16(h2[i1].ChildNodes[i2].ChildNodes[1].InnerText.Split('.')[1]);
                                        xsem.SetAttributeValue("number", semestr);
                                    }
                                    Subject subj = new Subject(nameprof, h2[i1].ChildNodes[i2].ChildNodes[1].InnerText,
                                        h2[i1].ChildNodes[i2].ChildNodes[3].InnerText.Replace("&nbsp;", ""),
                                        "",
                                        h2[i1].ChildNodes[i2].ChildNodes[7].InnerText.Trim().Replace("&nbsp;", ""),
                                        Convert.ToInt16(h2[i1].ChildNodes[i2].ChildNodes[9].InnerText.Trim().Replace("&nbsp;", "")  ));
                                    XElement xsubj = new XElement("subject", null);
                                    xsubj.SetAttributeValue("profile", subj.profile);
                                    xsubj.SetAttributeValue("number", subj.number);
                                    xsubj.SetAttributeValue("cikl", subj.cikl);
                                    if (h2[i1].ChildNodes[i2].ChildNodes[5].ChildNodes.Count > 1)
                                    {
                                       subj.name = h2[i1].ChildNodes[i2].ChildNodes[5].ChildNodes[1].InnerText.Trim().Replace("&nbsp;", "");
                                    }
                                    else
                                    {
                                        subj.name = h2[i1].ChildNodes[i2].ChildNodes[5].InnerText.Trim().Replace("&nbsp;", "");
                                    }
                                    xsubj.SetAttributeValue("subject",subj.name);
                                    xsubj.SetAttributeValue("forma", subj.forma);
                                    xsubj.SetAttributeValue("time", subj.time);
                                    xsem.Add(xsubj);
                                    //  txb.Text += nameprof + h2[i1].ChildNodes[i2].ChildNodes[1].InnerText + " ";
                                    //  txb.Text += h2[i1].ChildNodes[i2].ChildNodes[3].InnerText.Replace("&nbsp;", "") + " ";
                                    //if (h2[i1].ChildNodes[i2].ChildNodes[3].InnerText.Replace("&nbsp;", "").Contains("ОФ")) 
                                    //  j = 0;
                                    // txb.Text += h2[i1].ChildNodes[i2].ChildNodes[5].InnerText.Trim().Replace("&nbsp;", "") + " "; //ChildNodes[1].InnerText+ " ";
                                    // txb.Text += h2[i1].ChildNodes[i2].ChildNodes[7].InnerText.Trim().Replace("&nbsp;", "") + " ";
                                    //  txb.Text += h2[i1].ChildNodes[i2].ChildNodes[9].InnerText.Trim().Replace("&nbsp;", "") + " \n";
                                    //XmlElement semestr = 
                                }
                                else
                                {
                                    if (h2[i1].ChildNodes[i2].ChildNodes.Count > 3)
                                    {
                                        // txb.Text += h2[i1].ChildNodes[i2].ChildNodes[1].InnerText + " ";
                                           nameprof = h2[i1].ChildNodes[i2].ChildNodes[3].InnerText;//потом убрать пробел
                                        // txb.Text += h2[i1].ChildNodes[i2].ChildNodes[3].InnerText + " \n";
                                    }
                                    //txb.Text += h2[i1].ChildNodes[i2].InnerHtml + " \n";
                                }
                            }
                            //break;
                            //txb.Text+=n1.InnerText;
                            if (semestr != -1)
                                xspec.Add(xsem);
                        }
                        XDocument SpecDoc = new XDocument();
                        SpecDoc.Add(xspec);
                        SpecDoc.Save("ИМиКН\\" + xspec.Attributes("name").ToArray()[0].Value.Replace(':',' ') + ".xml");
                        SpecDoc = XDocument.Load("ИМиКН\\" + xspec.Attributes("name").ToArray()[0].Value.Replace(':', ' ') + ".xml");
                        //SpecDoc = XDocument.Load("Test.xml");
                        for (int i5 = 0; i5 <= SpecDoc.Nodes().Count(); i5++)
                        {
                            XElement xe = SpecDoc.Descendants("semestr").ToArray()[i5];
                            for (int i6 = 0; i6 < xe.Descendants("subject").Count(); i6++)
                            {
                              //  Subject Subj = new Subject(xe.Descendants("subject").ToArray()[i6].Attributes("profile").ToArray()[0].Value,
                               //     xe.Descendants("subject").ToArray()[i6].Attributes("number").ToArray()[0].Value,
                                //    xe.Descendants("subject").ToArray()[i6].Attributes("cikl").ToArray()[0].Value,
                                //    xe.Descendants("subject").ToArray()[i6].Attributes("subject").ToArray()[0].Value,
                                //    xe.Descendants("subject").ToArray()[i6].Attributes("forma").ToArray()[0].Value,
                                //    Convert.ToInt16(xe.Descendants("subject").ToArray()[i6].Attributes("time").ToArray()[0].Value));

                                txb.Text += xe.Descendants("subject").ToArray()[i6].Attributes("profile").ToArray()[0].Value;
                                txb.Text += xe.Descendants("subject").ToArray()[i6].Attributes("number").ToArray()[0].Value;
                                txb.Text += xe.Descendants("subject").ToArray()[i6].Attributes("cikl").ToArray()[0].Value;
                                txb.Text += xe.Descendants("subject").ToArray()[i6].Attributes("subject").ToArray()[0].Value;
                                txb.Text += xe.Descendants("subject").ToArray()[i6].Attributes("forma").ToArray()[0].Value;
                                txb.Text += xe.Descendants("subject").ToArray()[i6].Attributes("time").ToArray()[0].Value + " \n";
                            }
                        }
                        //XElement xe= SpecDoc.Descendants("semestr").ToArray()[1];//.Attributes("nomerpo").ToArray()[0].Value;
                        //txb.Text+=xe.Descendants("subject").Attributes("nomerpo").ToArray()[0].Value;
                        j = 0;
                        //break;
                    }

                    //break;
                }


                i++;
                if (i > 3) k = 2;

            }

            MessageBox.Show("First");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            var filename = "";

            if (dlg.ShowDialog() == true)
            {
                filename = dlg.FileName;
            }

            /*XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            var root = doc.DocumentElement;
            DataTable dt = new DataTable();
            dt.Columns.Add("Имя");
            dt.Columns.Add("Контроль");
            dt.Columns.Add("Часы");
            foreach (XmlNode semestr in root)
            {
                dt.Rows.Add("Семестр " + semestr.Attributes[0].Value);
                foreach (XmlNode subject in semestr)
                {
                    dt.Rows.Add(subject.Attributes[3].Value, subject.Attributes[4].Value, subject.Attributes[5].Value);
                }
            }
            curplan.ItemsSource = dt.DefaultView;*/
            
            //тестирум считывание из экселя - Лимон
            //List<Subject> in_ls = new List<Subject>();
            //ExcelDoc exceldoc = new ExcelDoc(filename);
            //нумерация в документе не с нуля
            /*for (int i = 1; i <= exceldoc.usedRowsNum; i++)
            {
                try
                {
                    Subject s = new Subject("", "", "", exceldoc.GetCellValue(i, 1), exceldoc.GetCellValue(i, 3), Convert.ToInt16(exceldoc.GetCellValue(i, 2)));
                    in_ls.Add(s);
                }
                catch { }
            }
            exceldoc.Close();*/

            //тестируем сохранение Словаря - Лимон - вроде норм
            /*DictionaryItem di = new DictionaryItem("Дифф. геометрия", true);
            XDocument SpecDoc = new XDocument();
            XElement xzag = new XElement("subj",null);
            xzag.SetAttributeValue("name", "Геометрия");
            xzag.Add(di.XML());
            SpecDoc.Add(xzag);
            SpecDoc.Save("test.xml");*/

            //тестируем считывание словаря, почему-то спускается ниже чем надо сразу. решил проблему добавлением одного тега 
            List<SubjForCompare> lsfb = new List<SubjForCompare>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            var root = doc.DocumentElement;
            //doc.
            //doc;
            foreach (XmlNode Sfbf in root)
            {
               SubjForCompare sfb = new SubjForCompare(new Subject("", "", "", Sfbf.Attributes[0].Value, "", 0), null, null);  
                foreach (XmlNode di in Sfbf)
                {
                    sfb.lDic.Add(new DictionaryItem(di.Attributes[0].Value, Convert.ToBoolean(di.Attributes[1].Value)));
                    //(subject.Attributes[3].Value, subject.Attributes[4].Value, subject.Attributes[5].Value);
                }
                int i = 0;
            }
            
        }
    }
}
