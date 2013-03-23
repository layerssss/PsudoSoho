using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing;
namespace SimpleOffice
{
    public class ExcelFile:IDisposable
    {        ///
        /// 构建ExcelClass类
        ///
        public ExcelFile()
        {
            //别忘了需要添加Excel Library的引用
            this.m_objExcel = new Microsoft.Office.Interop.Excel.Application();
        }
        ///
        /// 构建ExcelClass类
        ///
        ///
        public ExcelFile(Microsoft.Office.Interop.Excel.Application objExcel)
        {
            this.m_objExcel = objExcel;
            
        }
 
        ///
        /// 列标号，Excel最大列数是256
        ///
        private string[] ALists = new string[] {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ",
            "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ",
            "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ",
            "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ",
            "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ",
            "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ",
            "GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ",
            "HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ",
            "IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II", "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV"
        };
 
        ///
        /// 获取描述区域的字符
        ///
        ///
 
        ///
 
        ///
        public string GetAix(int x, int y)
        {
            if (x > 256) { return ""; }
            string s = "";
            s = s + ALists[x - 1].ToString();
            s = s + y.ToString();
            return s;
        }
 
        ///
        /// 给单元格赋值1
        ///
        ///行号
        ///列号
        ///对齐（CENTER、LEFT、RIGHT）
        ///值
        public void setValue(int y, int x, string align, string text)
        {
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
            range.set_Value(miss, text);
            if (align.ToUpper() == "CENTER")
            {
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            }
            if (align.ToUpper() == "LEFT")
            {
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            }
            if (align.ToUpper() == "RIGHT")
            {
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            }
        }
        /// <summary>
        /// 将图片插入到指定的单元格位置，并设置图片的宽度和高度。
        /// 注意：图片必须是绝对物理路径
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <param name="PicturePath">要插入图片的绝对路径。</param>
        /// <param name="PictuteWidth">插入后，图片在Excel中显示的宽度。</param>
        /// <param name="PictureHeight">插入后，图片在Excel中显示的高度。</param>
        public void InsertPicture(int y,int x, string PicturePath, float PictuteWidth, float PictureHeight)
        {
            try
            {
                Bitmap.FromFile(PicturePath).Dispose();
            }
            catch
            {
                throw (new Exception("无法识别图片的格式"));
            }
            var m_objSheet = sheet;
            var m_objRange = m_objSheet.get_Range(GetAix(x,y), miss);
            m_objRange.Select();
            float PicLeft, PicTop;
            PicLeft = Convert.ToSingle(m_objRange.Left);
            PicTop = Convert.ToSingle(m_objRange.Top);
            //参数含义：
            //图片路径
            //是否链接到文件
            //图片插入时是否随文档一起保存
            //图片在文档中的坐标位置（单位：points）
            //图片显示的宽度和高度（单位：points）
            //参数详细信息参见：http://msdn2.microsoft.com/zh-cn/library/aa221765(office.11).aspx

            m_objSheet.Shapes.AddPicture(PicturePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, PicLeft, PicTop, PictuteWidth, PictureHeight);


        }
        ///
        /// 给单元格赋值2
        ///
        ///行号
        ///列号
        ///值
        public void setValue(int y, int x, string text)
        {
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
            range.set_Value(miss, text);
        }

        public void setLink(int y, int x, string text, string navigation)
        {

            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
            sheet.Hyperlinks.Add(range, navigation, miss, "点击打开", text);
        }
        ///
        /// 给单元格赋值3
        ///
        ///行号
        ///列号
        ///值
        ///字符格式
        ///颜色
        public void setValue(int y, int x, string text, System.Drawing.Font font, int color)
        {
            this.setValue(x, y, text);
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
            range.Font.Size = font.Size;
            range.Font.Bold = font.Bold;
            //这里是int型的颜色

            range.Font.Color = ColorTranslator.ToOle(ColorTranslator.FromWin32(color));
            range.Font.Name = font.Name;
            range.Font.Italic = font.Italic;
            range.Font.Underline = font.Underline;
        }
 
        ///
        /// 给单元格赋值3
        ///
        ///行号
        ///列号
        ///值
        ///字符格式
        ///颜色
        public void setValue(int y, int x, string text, System.Drawing.Font font, int color, string align)
        {
            this.setValue(x, y, text);
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
            range.Font.Size = font.Size;
            range.Font.Bold = font.Bold;
            //这里是int型的颜色
            range.Font.Color = ColorTranslator.ToOle(ColorTranslator.FromWin32(color));
            range.Font.Name = font.Name;
            range.Font.Italic = font.Italic;
            range.Font.Underline = font.Underline;
 
            if (align.ToUpper() == "CENTER")
            {
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            }
            if (align.ToUpper() == "LEFT")
            {
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            }
            if (align.ToUpper() == "RIGHT")
            {
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            }
        }
 
        ///
        /// 插入新行
        ///
        ///模板行号
        public void insertRow(int y)
        {
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(GetAix(1, y), GetAix(255, y));
            range.Copy(miss);
            range.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown, miss);
            range.get_Range(GetAix(1, y), GetAix(255, y));
            range.Select();
            sheet.Paste(miss, miss);
 
        }
 
        ///
        /// 把剪切内容粘贴到当前区域
        ///
        public void paste()
        {
            string s = "a,b,c,d,e,f,g";
            sheet.Paste(sheet.get_Range(this.GetAix(10, 10), miss), s);
        }

        ///
        /// 设置边框
        ///
        /// 
        /// 
        /// 
        /// 
        /// 
        public void setBorder(int x1, int y1, int x2, int y2, int Width)
        {
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x1, y1), this.GetAix(x2, y2));
            range.Borders.Weight = Width;
        }
        public void mergeCell(int x1, int y1, int x2, int y2)
        {
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x1, y1), this.GetAix(x2, y2));
            range.Merge(true);
        }
 
        public Microsoft.Office.Interop.Excel.Range getRange(int x1, int y1, int x2, int y2)
        {
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range(this.GetAix(x1, y1), this.GetAix(x2, y2));
            return range;
        }

        private object miss = System.Reflection.Missing.Value; //忽略的参数OLENULL
        private Microsoft.Office.Interop.Excel.Application m_objExcel;//Excel应用程序实例
        private Microsoft.Office.Interop.Excel.Workbooks m_objBooks;//工作表集合
        private Microsoft.Office.Interop.Excel.Workbook m_objBook;//当前操作的工作表
        private Microsoft.Office.Interop.Excel.Worksheet sheet;//当前操作的表格
 
        public Microsoft.Office.Interop.Excel.Worksheet CurrentSheet
        {
            get
            {
                return sheet;
            }
            set
            {
                this.sheet = value;
            }
        }
 
        public Microsoft.Office.Interop.Excel.Workbooks CurrentWorkBooks
        {
            get
            {
                return this.m_objBooks;
            }
            set
            {
                this.m_objBooks = value;
            }
        }
 
        public Microsoft.Office.Interop.Excel.Workbook CurrentWorkBook
        {
            get
            {
                return this.m_objBook;
            }
            set
            {
                this.m_objBook = value;
            }
        }
 
        ///
        /// 打开Excel文件
        ///
        ///路径
        public void OpenExcelFile(string filename)
        {
            UserControl(false);
 
            m_objExcel.Workbooks.Open(filename, miss, miss, miss, miss, miss, miss, miss,
                                    miss, miss, miss, miss, miss, miss, miss);
 
            m_objBooks = (Microsoft.Office.Interop.Excel.Workbooks)m_objExcel.Workbooks;
 
            m_objBook = m_objExcel.ActiveWorkbook;
            sheet = (Microsoft.Office.Interop.Excel.Worksheet)m_objBook.ActiveSheet;
        }

        public void CloseExcelFile()
        {
            m_objExcel.ActiveWorkbook.Close(miss, miss, miss);
        }
        public void UserControl(bool usercontrol)
        {
            if (m_objExcel == null) { return; }
            m_objExcel.UserControl = usercontrol;
            m_objExcel.DisplayAlerts = usercontrol;
            m_objExcel.Visible = usercontrol;
        }
 
        public void CreateExceFile()
        {
            UserControl(false);
            m_objBooks = (Microsoft.Office.Interop.Excel.Workbooks)m_objExcel.Workbooks;
            m_objBook = (Microsoft.Office.Interop.Excel.Workbook)(m_objBooks.Add(miss));
            sheet = (Microsoft.Office.Interop.Excel.Worksheet)m_objBook.ActiveSheet;
        }
 
        public void SaveAs(string FileName)
        {
            m_objBook.SaveAs(FileName, miss, miss, miss, miss,
             miss, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
             Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges,
             miss, miss, miss, miss);
            //m_objBook.Close(false, miss, miss);
        }
 
        protected void ReleaseExcel()
        {
            m_objExcel.Quit();
 
            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)m_objExcel);
            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)m_objBooks);
            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)m_objBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)sheet);
 
            sheet = null;
            m_objBook = null;
            m_objBooks = null;
            m_objExcel = null;
 
            GC.Collect(0);
        }
 
        #region KillAllExcel
        protected bool KillAllExcel()
        {
            try
            {
                if (m_objExcel != null) // isRunning是判断xlApp是怎么启动的flag.
                {
                    m_objExcel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objExcel);
                    //释放COM组件，其实就是将其引用计数减1
                    //System.Diagnostics.Process theProc;
                    foreach (System.Diagnostics.Process theProc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                    {
                        //先关闭图形窗口。如果关闭失败...有的时候在状态里看不到图形窗口的excel了，
                        //但是在进程里仍然有EXCEL.EXE的进程存在，那么就需要杀掉它:p
                        if (theProc.CloseMainWindow() == false)
                        {
                            theProc.Kill();
                        }
                    }
                    m_objExcel = null;
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
 
        #region Kill Special Excel Process
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        //推荐这个方法，找了很久，不容易啊
        protected void KillSpecialExcel()
        {
            try
            {
                if (m_objExcel != null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId(new IntPtr(m_objExcel.Hwnd), out lpdwProcessId);
 
                    System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete Excel Process Error:" + ex.Message);
            }
        }
        #endregion

        public void Dispose()
        {
            this.ReleaseExcel();
            this.KillSpecialExcel();
        }
    }
}
