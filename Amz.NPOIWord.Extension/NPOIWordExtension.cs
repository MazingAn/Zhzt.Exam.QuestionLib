using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;

namespace Amz.NPOIWord.Extension
{
    public static class NPOIWordExtension
    {
        /// <summary>
        /// 使用普通国标A4文档的设置
        /// </summary>
        /// <param name="land">是否是横向</param>
        /// <returns></returns>
        public static XWPFDocument UseA4Size(this XWPFDocument doc,  bool land = false)
        {
            doc.Document.body.sectPr = new CT_SectPr();
            doc.Document.body.sectPr.pgSz.h = (ulong) (land ? 11906 : 16838); //高度 1厘米=567提
            doc.Document.body.sectPr.pgSz.w = (ulong) (land ? 16838 : 11906); //宽度 1厘米=567提
            doc.Document.body.sectPr.pgMar.left = (ulong)800;//左边距
            doc.Document.body.sectPr.pgMar.right = (ulong)800;//右边距
            doc.Document.body.sectPr.pgMar.top = "850";//上边距
            doc.Document.body.sectPr.pgMar.bottom = "850";//下边距
            return doc;
        }

        /// <summary>
        /// 使用普通国标A3文档的设置
        /// </summary>
        /// <param name="land">是否是横向</param>
        /// <returns></returns>
        public static XWPFDocument UseA3Size(this XWPFDocument doc, bool land = false)
        {
            doc.Document.body.sectPr = new CT_SectPr();
            doc.Document.body.sectPr.pgSz.h = (ulong)(land ? 29.7 * 567 : 42*567); //高度 1厘米=567提
            doc.Document.body.sectPr.pgSz.w = (ulong)(land ? 42 * 567 : 29.7 * 42); //宽度 1厘米=567提
            doc.Document.body.sectPr.pgMar.left = (ulong)800;//左边距
            doc.Document.body.sectPr.pgMar.right = (ulong)800;//右边距
            doc.Document.body.sectPr.pgMar.top = "850";//上边距
            doc.Document.body.sectPr.pgMar.bottom = "850";//下边距
            return doc;
        }

        public static XWPFDocument BreakPage(this XWPFDocument doc)
        {
            var paragraph = doc.CreateParagraph();
            paragraph.CreateRun().AddBreak(BreakType.PAGE);
            return doc;
        }

        /// <summary>
        /// 添加标题
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fontSize"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static XWPFDocument AddHeader(this XWPFDocument doc, string text, 
            int fontSize = 32, 
            int lineSpaceUp = 100,
            int lineSpaceDown = 100,
            string fontName = "黑体",
            ParagraphAlignment align=ParagraphAlignment.LEFT)
        {
            var paragraph = doc.CreateParagraph();
            paragraph.Alignment = align; //字体居中
            var run = paragraph.CreateRun();
            run.IsBold = true;
            run.SetText(text);
            run.FontSize = fontSize;
            //设置黑体
            run.SetFontFamily("黑体", FontCharRange.None); 
            //控制段落与其他元素的上下距离
            paragraph.SpacingBeforeLines = lineSpaceUp;//上方距离
            paragraph.SpacingAfterLines = lineSpaceDown;//下方距离
            return doc;
        }

        /// <summary>
        /// 添加正文
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fontSize"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static XWPFDocument AddContent(this XWPFDocument doc, string text, 
            int linSpaceUp = 20,
            int lineSpaceDown = 20,
            string fontName="楷体",
            int indent = 0, 
            int fontSize = 14, 
            ParagraphAlignment align = ParagraphAlignment.LEFT)
        {
            var paragraph = doc.CreateParagraph();
            paragraph.Alignment = align; //字体居中
            paragraph.IndentationLeft = indent;
            var run = paragraph.CreateRun();
            run.SetText(text);
            run.FontSize = fontSize;
            //设置黑体
            run.SetFontFamily(fontName, FontCharRange.None);
            //控制段落与其他元素的上下距离
            paragraph.SpacingBeforeLines = linSpaceUp;//上方距离
            paragraph.SpacingAfterLines = lineSpaceDown;//下方距离
            return doc;
        }

        /// <summary>
        /// 向doc文档中添加表格
        /// </summary>
        /// <param name="doc">文档</param>
        /// <param name="tData">表格数据内容</param>
        /// <returns></returns>
        public static XWPFDocument AddTable(this XWPFDocument doc, CustomerNPOITableData tData)
        {
            var table = doc.CreateTable(tData.Rows.Count + 1, tData.Headers.Count);
            table.Width = 5000;
            // 设置头
            for (int i = 0; i < tData.Headers.Count; i++)
            {
                var cellParagraph = GenTableText(table, tData.Headers[i], 12, ParagraphAlignment.CENTER, TextAlignment.CENTER, "黑体");
                table.GetRow(0).GetCell(i).SetParagraph(cellParagraph);
                table.GetRow(0).Height = 567;
            }
            // 设置数据行
            for (int rowIdx = 0; rowIdx < tData.Rows.Count; rowIdx++)
            {
                for (int colIdx = 0; colIdx < tData.Headers.Count; colIdx++)
                {
                    table.GetRow(rowIdx+1).GetCell(colIdx).SetParagraph(GenTableText(table, tData.Rows[rowIdx][colIdx]));
                    table.GetRow(rowIdx+1).Height = 567;
                }
            }
            return doc;
        }

        /// <summary>
        /// 生成单元格内容
        /// </summary>
        /// <param name="table"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="hAlign"></param>
        /// <param name="vAlign"></param>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static XWPFParagraph GenTableText(XWPFTable table, string text, int fontSize = 14,
            ParagraphAlignment hAlign = ParagraphAlignment.CENTER,
            TextAlignment vAlign = TextAlignment.CENTER,
            string fontName = "黑体")
        {
            var para = new CT_P();
            var pCell = new XWPFParagraph(para, table.Body);
            pCell.Alignment = hAlign; //字体水平居中
            pCell.VerticalAlignment = vAlign; //字体垂直居中

            var r1c1 = pCell.CreateRun();
            r1c1.SetText(text);
            r1c1.FontSize = 12;
            r1c1.SetFontFamily(fontName, FontCharRange.None);
            pCell.SpacingAfterLines = 60;
            pCell.SpacingBeforeLines = 60;
            return pCell;
        }
    }
}