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
        public List<Spec> Lspec= new List<Spec>();
        public List<SubjForCompare> Lsfb= new List<SubjForCompare>();
        public List<Subject> in_lsubj = new List<Subject>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //тестируем считывание словаря, почему-то спускается ниже чем надо сразу. решил проблему добавлением одного тега 
            /*List<SubjForCompare> lsfb = new List<SubjForCompare>();*/
            XmlElement root;
            XmlDocument docDic = new XmlDocument();
            if (File.Exists("test.xml"))
            {
                docDic.Load("test.xml");
                root = docDic.DocumentElement;
                foreach (XmlNode Sfbf in root)
                {
                    SubjForCompare sfb1 = new SubjForCompare(new Subject("", "", "", Sfbf.Attributes[0].Value, "", 0), null, null);
                    foreach (XmlNode di in Sfbf)
                    {
                        sfb1.lDic.Add(new DictionaryItem(di.Attributes[0].Value, Convert.ToBoolean(di.Attributes[1].Value)));
                    }
                    Lsfb.Add(sfb1);
                }
            }
            if (Directory.Exists("ИМиКН"))
            {
                string[] paths= Directory.GetFiles("ИМиКН", "*.xml");
                foreach (string path in paths)
                {
                    XmlDocument docSpec = new XmlDocument();
                    docSpec.Load(path);
                    root = docSpec.DocumentElement;
                    Spec spec = new Spec(root.Attributes[0].Value, root.Attributes[1].Value, root.Attributes[2].Value, new List<Semestr>());
                    spec.name = root.Attributes[0].Value;
                    foreach (XmlNode xsemestr in root)
                    {
                        Semestr sem = new Semestr(Convert.ToInt16(xsemestr.Attributes[0].Value), new List<Subject>());
                        foreach (XmlNode xsubject in xsemestr)
                        {
                            Subject subj = new Subject(xsubject.Attributes[0].Value, xsubject.Attributes[1].Value, xsubject.Attributes[2].Value, xsubject.Attributes[3].Value, xsubject.Attributes[4].Value,Convert.ToInt16(xsubject.Attributes[5].Value));
                            sem.LS_.Add(subj);
                        }
                        spec.LSem_.Add(sem);
                    }
                    Lspec.Add(spec);
                    SpecList.Items.Add(spec.name);
                }
            }
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

        public void AddEdDI(string mainname, string in_subjname, bool state)
        {
            SubjForCompare tsfc;
            int tcount = Lsfb.Count((x => x.subj.name.ToUpper() == mainname.ToUpper()));
            if (tcount != 0)
            {
                tsfc = Lsfb.Where(x => x.subj.name.ToUpper() == mainname.ToUpper()).First();
                tcount = tsfc.lDic.Count(x => x.name.ToUpper() == in_subjname.ToUpper());
                DictionaryItem tdi;
                if (tcount != 0)
                {
                    tdi = tsfc.lDic.Where(x => x.name.ToUpper() == in_subjname.ToUpper()).First();
                    tdi.status = state;
                }
                else
                {
                    tdi = new DictionaryItem(in_subjname, state);
                    tsfc.lDic.Add(tdi);
                }
            }
            else
            {
                tsfc = new SubjForCompare(new Subject("", "", "", mainname, "", 0), null, null);
                tsfc.lDic.Add(new DictionaryItem(in_subjname, state));
                Lsfb.Add(tsfc);
            }
        }

        public void Search()
        {
            //с маленьким словарем и входным файлом работает шустро
            int delta=5;
          foreach(Spec spec in Lspec)
          {
              foreach(Semestr sem in spec.LSem_)
              {
                  foreach(Subject subj in sem.LS_)
                  {
                      List<DictionaryItem> ldi_no = new List<DictionaryItem>();
                      subj.check = false;
                      var tmps=in_lsubj.Where(x=>x.name.ToUpper()==subj.name.ToUpper()).ToList();
                      if (tmps.Count == 0) 
                      {
                          //пытаемся дополнить словарь
                          var ndi= in_lsubj.Where(x => x.name.ToUpper().Contains(subj.name.ToUpper()) || subj.name.ToUpper().Contains(x.name.ToUpper()));
                          foreach(Subject s in ndi)
                          {
                              int tcounti = Lsfb.Count(x => x.subj.name.ToUpper() == subj.name.ToUpper());
                              if (tcounti != 0)
                              {
                                  tcounti = Lsfb.Where(x => x.subj.name.ToUpper() == subj.name.ToUpper()).First().lDic.Count(y => y.name.ToUpper() == s.name.ToUpper());
                                  if (tcounti == 0)
                                  {
                                      //MessageBox.Show("Предлагаем добавить новый дикшинари айтем потомок");
                                      bool state=true; // спрашиваем в всплыывающем окне
                                      AddEdDI(subj.name, s.name, state);
                                  }
                              }
                              else
                              {
                                  //MessageBox.Show("Предлагаем добавить новый дикшинари айтем корень"); 
                                  bool state = true; // спрашиваем в всплыывающем окне
                                  AddEdDI(subj.name, s.name, state);
                              }
                          }
                      }
                      int tcount= Lsfb.Count(x=>x.subj.name.ToUpper()==subj.name.ToUpper());
                      if (tcount!=0)
                      {
                        foreach(Subject sfb in in_lsubj){
                            tcount= Lsfb.Where(x=>x.subj.name.ToUpper()==subj.name.ToUpper()).First().lDic.Count(y=>y.name.ToUpper()==sfb.name.ToUpper());
                            if (tcount!=0)
                            {
                                DictionaryItem tdi = Lsfb.Where(x=>x.subj.name==subj.name).First().lDic.Where(y=>y.name.ToUpper()==sfb.name.ToUpper()).First();
                                {
                                    if (tdi.status)
                                    tmps.Add(sfb); else ldi_no.Add(tdi);
                                    //надо запомнить, что статус нет и не предлагать потом добавление в словарь
                                }
                            }
                        }
                      }
                      foreach (Subject stmp in tmps)
                      {
                          if (stmp.forma == subj.forma && Math.Abs(stmp.time - subj.time) <= delta)
                          {
                              subj.check = true; break; //совпадает - пометить что такой предмет есть у соискателя один предмет - один предмет?
                          }
                      }
                      if (!subj.check)
                      {
                          
                          //проверить по подстроке в строке, заглянуть в дикшинариайтем, если нет отрицания,
                          //предложить добавить в случае успеха поиска.
                      }
                  }
                  if (sem.LS_.Count(x=>x.check)==0)
                  {
                      sem.stat = 0;
                  } 
                  else
                  {
                      if (sem.LS_.Count(x => x.check) == sem.LS_.Count) sem.stat = 2; else sem.stat = 1;
                  }
              }
          }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            var filename = "";

            if (dlg.ShowDialog() == true)
            {
                filename = dlg.FileName;
            }

            /*addCompare */
            
            
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
            ExcelDoc exceldoc = new ExcelDoc(filename);
            //нумерация в документе не с нуля
            for (int i = 1; i <= exceldoc.usedRowsNum; i++)
            {
                try
                {
                    Subject s = new Subject("", "", "", exceldoc.GetCellValue(i, 1), exceldoc.GetCellValue(i, 3), Convert.ToInt16(exceldoc.GetCellValue(i, 2)));
                    in_lsubj.Add(s);
                }
                catch { }
            }
            exceldoc.Close();

            //тестируем сохранение Словаря - Лимон - вроде норм



            /*List<SubjForCompare> lsfc = new List<SubjForCompare>();
            SubjForCompare sfb = new SubjForCompare(new Subject("", "", "", "Геометрия", "", 0), null, null);
            sfb.lDic.Add(new DictionaryItem("Дифф. геометрия", true));
            sfb.lDic.Add(new DictionaryItem("Топ. геометрия", true));
            lsfc.Add(sfb);
            sfb = new SubjForCompare(new Subject("", "", "", "Алгебра", "", 0), null, null);
            lsfc.Add(sfb);
            XDocument SpecDoc = new XDocument();
            XElement xfic = new XElement("subjects", null);
            SpecDoc.Add(xfic);
            for (int i8 = 0; i8 < lsfc.Count; i8++) 
            {
                XElement xsubj = new XElement("subj", null);
                xsubj.SetAttributeValue("name", lsfc[i8].subj.name);
                for (int i9 = 0; i9 < lsfc[i8].lDic.Count; i9++)
                {
                  //  XElement xdic = new XElement("DictionaryItem", null);
                  //  xdic.SetAttributeValue("name", lsfc[i8].lDic[i9].name);
                  //  xdic.SetAttributeValue("status", lsfc[i8].lDic[i9].status);

                    xsubj.Add(lsfc[i8].lDic[i9].XML());
                }
                xfic.Add(xsubj);
            }
            SpecDoc.Save("test.xml");*/
            
            /*string s1 = "Геометрия1", s2 = "Дифф. геометрия";
            bool state = true;
            //addeditSubjForCompare
            SubjForCompare tsfc;
            int tcount = Lsfb.Count((x => x.subj.name.ToUpper() == s1.ToUpper()));
            if (tcount!=0)
            {
                tsfc = Lsfb.Where(x => x.subj.name.ToUpper() == s1.ToUpper()).First();
                tcount = tsfc.lDic.Count(x => x.name.ToUpper() == s2.ToUpper());
                DictionaryItem tdi;
                if (tcount != 0) 
                { 
                    tdi = tsfc.lDic.Where(x => x.name.ToUpper() == s2.ToUpper()).First();
                    tdi.status = state; 
                } 
                else 
                { 
                    tdi = new DictionaryItem(s2, state);
                    tsfc.lDic.Add(tdi);
                }
            }
            else
            {
                tsfc = new SubjForCompare(new Subject("", "", "", s1, "", 0), null, null);
                tsfc.lDic.Add(new DictionaryItem(s2, state));
                Lsfb.Add(tsfc);
            }
            //int i = 0;

            //delete subjforcompare
            //s1 = "Тест"; s2 = "Тест2";

            //Lsfb.Where(x => x.subj.name.ToUpper() == s1.ToUpper()).First().lDic.RemoveAll(y => y.name.ToUpper() == s2.ToUpper());
            //if (Lsfb.Where(x => x.subj.name.ToUpper() == s1.ToUpper()).First().lDic.Count == 0) Lsfb.RemoveAll(x=>x.subj.name.ToUpper()==s1.ToUpper());*/
            Search();
            List<Subject> t = new List<Subject>();
            string plz = "";
            foreach (Spec s in Lspec)
            {
                foreach(Semestr sem in s.LSem_)
                {
                   if (sem.stat > 0) { plz += s.name + " "; }
                   t.AddRange(sem.LS_.Where(x => x.check));
                }
            }
            int k = 0;
        }
        
        private void SpecList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Профиль");
            dt.Columns.Add("Имя");
            dt.Columns.Add("Контроль");
            dt.Columns.Add("Часы");
            dt.Columns.Add("Сдано");
            foreach (Semestr semestr in Lspec[SpecList.SelectedIndex].LSem_)
            {
                dt.Rows.Add("Семестр " + semestr.number);
                foreach (Subject subject in semestr.LS_)
                {
                    dt.Rows.Add(subject.profile, subject.name, subject.forma, subject.time, subject.check);
                }
            }
            curplan.ItemsSource = dt.DefaultView;    
        }
    }
}
