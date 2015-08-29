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
using Hap= HtmlAgilityPack;
using System.Net;
using System.IO;

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
            string parentsite = "http://www.umk3plus.utmn.ru/";
            txb.Text = "work";
            MessageBox.Show("Start");
            string url = //"http://www.umk3plus.utmn.ru/?section=speciality&id=645"; 
                "http://www.umk3plus.utmn.ru/?section=faculty&id=162";
                //"http://www.umk3plus.utmn.ru/?section=faculty&id=169";
            var web = new Hap.HtmlWeb
            {
              AutoDetectEncoding = false,
              OverrideEncoding = Encoding.Default,
            };
            MessageBox.Show("Loadstring");
            Hap.HtmlDocument doc = web.Load(url);
            MessageBox.Show("LoadDOM");
            var h1 = doc.DocumentNode.SelectNodes("//table[@class='text']").ToArray()[1];
       
            if (h1 == null) MessageBox.Show("null");
            //"//a[@class='menu']").First(); //doc.DocumentNode.SelectNodes("/html/head/title").First(); //doc.DocumentNode.SelectNodes("//h3").First();
            int i=0,j = 5, k=0;
            string urlspec = "";
            Hap.HtmlDocument docspec;
            foreach (var n in h1.ChildNodes)
            {
               //txb.Text += n.InnerHtml;
               if (i>15 && i%2==1 && n.ChildNodes.Count>5)
               {
                   txb.Text += "Специальность " + n.ChildNodes[5-k].InnerText;
                   txb.Text += "Форма обучения " + n.ChildNodes[7-k].InnerText;
                   txb.Text += "URL " +parentsite+ n.ChildNodes[9-k].ChildNodes[0].Attributes["href"].Value + " \n";
                   urlspec = parentsite + n.ChildNodes[9 - k].ChildNodes[0].Attributes["href"].Value;
                   docspec = web.Load(urlspec);
                   string zapros = "//table[@cellspacing='4']/tbody";
                   var h2 = docspec.DocumentNode.SelectNodes("//table[@cellspacing='4']/tr[2]/td[2]/table").ToArray();
                   for (int i1=0;i1<h2.Count();i1++)
                   {
                       //txb.Text += h2[i1].InnerHtml + "\n Новое \n";
                       for (int i2 = 3; i2 < h2[i1].ChildNodes.Count; i2 += 2)
                       {
                           if (h2[i1].ChildNodes[i2].ChildNodes.Count > 5)
                           {
                               txb.Text += h2[i1].ChildNodes[i2].ChildNodes[1].InnerText + " ";
                               txb.Text += h2[i1].ChildNodes[i2].ChildNodes[3].InnerText.Replace("&nbsp;", "") + " ";
                               txb.Text += h2[i1].ChildNodes[i2].ChildNodes[5].InnerText.Trim().Replace("&nbsp;", "") + " ";
                               txb.Text += h2[i1].ChildNodes[i2].ChildNodes[7].InnerText.Trim().Replace("&nbsp;", "") + " ";
                               txb.Text += h2[i1].ChildNodes[i2].ChildNodes[9].InnerText.Trim().Replace("&nbsp;", "") + " \n";
                           }
                       }
                       //break;
                       //txb.Text+=n1.InnerText;
                   }
                       
                   break;
               }
                
               i++;
               if (i > 3) k = 2;
               
            }


            MessageBox.Show("First");
            txb.Text += h1.InnerText;
           // txb.Text+="Здесь пошел хтмл";
          //  txb.Text+=h1.InnerHtml;
        }
    }
}
