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

namespace Test
{
    public class Subject 
    {
        string profile_, number_, cikl_, name_, forma_;
        int time_;

        public string profile
        {
            get { return profile_ ; }
            set { profile_ = value; }
        }

        public string number 
        {
            get { return number_; }
            set { number_ = value; }

        }

        public string cikl
        {
            get { return cikl_; }
            set { cikl_ = value; }
        }
        public string name
        {
            get { return name_; }
            set { name_ = value; }
        }

        public string forma
        {
            get { return forma_; }
            set { forma_ = value; }
        }
        public int time
        {
            get { return time_; }
            set
            {
                if (time < 0) time_ = 0; else time_ = value;

            }
        }
        public Subject(string in_profile, string in_number, string in_cikl, string in_name, string in_forma ,int in_time)
        {
            profile = in_profile;
            number = in_number;
            cikl = in_cikl;
            name = in_name;
            forma = in_forma;
            time = in_time;
        }
    }

    public class Semestr
    {
        int number_;
        public List<Subject> LS_;

        public int number
        {
            get { return number_; }
            set
            {
                if (number < 0) number_ = 0; else number_ = number;

            }
        }

        public Semestr(int in_number, List<Subject> in_LS)
        {
            LS_ = new List<Subject>();
            LS_.AddRange(in_LS);
            number = in_number;
        }
    }
    public class Spec
    {
        string name_, form_, URL_;
        public List<Semestr> LSem_;

        public string name
        {
            get { return name_; }
            set { name_ = name; }
        }
        public string form
        {
            get { return form_; }
            set { form_ = form; }
        }
        public string URL
       {
            get { return URL_; }
            set { URL_ = URL; }
        }

        public Spec(string in_name, string in_form, string in_URL, List<Semestr> in_LSem)
        {
            LSem_ = new List<Semestr>();
            LSem_.AddRange(in_LSem);
            name = in_name;
            form = in_form;
            URL = in_URL;
        }
    }
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
            var h1 = doc.DocumentNode.SelectNodes("//table[@class='text']").ToArray()[1];
            if (h1 == null) MessageBox.Show("null");
            //"//a[@class='menu']").First(); //doc.DocumentNode.SelectNodes("/html/head/title").First(); //doc.DocumentNode.SelectNodes("//h3").First();
            int i = 0, j = 5, k = 0;
            string urlspec = "";
            Hap.HtmlDocument docspec;
            Directory.CreateDirectory("ИМиКН");
            foreach (var n in h1.ChildNodes)
            {

                //txb.Text += n.InnerHtml;
                if (i > 2 && i % 2 == 1 && n.ChildNodes.Count > 5)
                {
                    XElement xspec = new XElement("info", null);

                    xspec.SetAttributeValue("name", n.ChildNodes[5 - k].InnerText);
                    xspec.SetAttributeValue("form", n.ChildNodes[7 - k].InnerText);
                    xspec.SetAttributeValue("URL", parentsite + n.ChildNodes[9 - k].ChildNodes[0].Attributes["href"].Value);

                    string s = xspec.ToString();
                    //x.SetAttributeNode("version", "1.0");
                    //txb.Text += "Специальность " + n.ChildNodes[5 - k].InnerText;
                    //txb.Text += "Форма обучения " + n.ChildNodes[7 - k].InnerText;
                    //txb.Text += "URL " + parentsite + n.ChildNodes[9 - k].ChildNodes[0].Attributes["href"].Value + " \n";
                    urlspec = parentsite + n.ChildNodes[9 - k].ChildNodes[0].Attributes["href"].Value;
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
    }
}
