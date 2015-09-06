using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel= Microsoft.Office.Interop.Excel;

namespace Test
{
    public class ExcelDoc
    {
        private Excel.Application _application = null;
        private Excel.Workbook _workBook = null;
        private Excel.Worksheet _workSheet = null;
        private object _missingObj = System.Reflection.Missing.Value;

       public int usedRowsNum;

       public int usedColumnsNum;

        //КОНСТРУКТОР
        public ExcelDoc()
        {
            _application = new Excel.Application();
            _workBook = _application.Workbooks.Add(_missingObj);
            _workSheet = (Excel.Worksheet)_workBook.Worksheets.get_Item(1);
        }

        public ExcelDoc(string pathToTemplate)
        {
            object pathToTemplateObj = pathToTemplate;

            _application = new Excel.Application();
            _workBook = _application.Workbooks.Add(pathToTemplateObj);
            _workSheet = (Excel.Worksheet)_workBook.Worksheets.get_Item(1);
            usedRowsNum = _workSheet.UsedRange.Rows.Count;
            usedColumnsNum = _workSheet.UsedRange.Columns.Count;
        }

        // ВИДИМОСТЬ ДОКУМЕНТА
        public bool Visible
        {
            get
            {
                return _application.Visible;
            }
            set
            {
                _application.Visible = value;
            }
        }

        public void Close()
        {
            _workBook.Close(false, _missingObj, _missingObj);

            _application.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(_application);

            _application = null;
            _workBook = null;
            _workSheet = null;

            System.GC.Collect();
        }

        public string GetCellValue(int rowIndex, int columnIndex)
        {
            string cellValue = "";

            Excel.Range cellRange = (Excel.Range)_workSheet.Cells[rowIndex, columnIndex];
            if (cellRange.Value != null)
            {
                cellValue = cellRange.Value.ToString();
            }
            return cellValue;
        }

    }

}
