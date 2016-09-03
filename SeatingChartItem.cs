using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;


namespace SeatingChart
{
    public class SeatingChartItem : DataTable
    {
        private string _sFullName = "";
        private string _sFooterText = "";

        public static class S
        {
            public const string ImagePath = "ImagePath";
            public const string Index = "Index";
            public const string Name = "Name";
            public const string Show = "Show";
        };

        public SeatingChartItem(string sFullName)
        {
            Columns.Add(S.ImagePath, typeof(string));
            Columns.Add(S.Index, typeof(int));
            Columns.Add(S.Name, typeof(string));
            Columns.Add(S.Show, typeof(bool));

            FullName = sFullName;
        }

        public string FullName
        {
            get
            {
                return _sFullName;
            }
            set
            {
                _sFullName = value;
                base.TableName = this.Name;
            }
        }

        public string FolderPath
        {
            get
            {
                return Path.GetDirectoryName( _sFullName );
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FullName);
            }
        }

        public string FooterText
        {
            get { return _sFooterText; }
            set { _sFooterText = value; }
        }

        public void Load()
        {
            Rows.Clear();
            _sFooterText = "";

            XmlDocument doc = new XmlDocument();
			using (StreamReader rdr = File.OpenText(FullName))
			{
				doc.Load(rdr);
			}
            XmlElement eleRoot = doc.DocumentElement;
            XmlElement dataTableEle = null;

            // get FooterText
            XmlElement ele = eleRoot.SelectSingleNode( @"/doc/FooterText" ) as XmlElement;
            if (ele == null)
            {
            }
            else
            {
                _sFooterText = ele.InnerText;
            }

            dataTableEle = eleRoot.SelectSingleNode(@"/doc/NewDataSet") as XmlElement;
            if (dataTableEle == null)
            {
                dataTableEle = eleRoot.SelectSingleNode(@"/NewDataSet") as XmlElement;
            }

            if (dataTableEle == null)
            {
            }
            else
            {
                string sOuter = dataTableEle.OuterXml;
                using (MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(sOuter)))
                {
                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        this.ReadXml(reader);
                    }
                }
            }
        }

        public void Save()
        {
            try
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                string sText = ToString();
                File.WriteAllText(FullName, sText);
            }
            catch (Exception ex)
            {
                if (File.Exists(FullName))
                {
                    File.Delete(FullName);
                }
                throw ex;
            }
        }

        public string ToString0()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (StringWriter writer = new StringWriter(sb) )
                {
                    WriteXml(writer, XmlWriteMode.WriteSchema);
                }
            }
            catch (Exception ex)
            {
                sb.Clear();
                sb.Append(ex.Message);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            string sText = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement rootEle = doc.CreateElement("doc");
                doc.AppendChild(rootEle);
                XmlElement ele = doc.CreateElement("FooterText");
                ele.AppendChild(doc.CreateTextNode(FooterText.Trim(new char[] { ' ', '\t', '\r', '\n' })));
                rootEle.AppendChild(ele);

                using (StringWriter writer = new StringWriter())
                {
                    WriteXml(writer, XmlWriteMode.WriteSchema);
                    XmlDocumentFragment frag = doc.CreateDocumentFragment();
                    frag.InnerXml = writer.ToString();
                    rootEle.AppendChild(frag);
                }

                using (StringWriter writer = new StringWriter())
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
                    {
                        doc.WriteTo(xmlWriter);
                    }
                    sText = writer.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sText;        
        }

    
    }
}
