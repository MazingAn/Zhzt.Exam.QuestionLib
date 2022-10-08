using Aspose.Words.Fonts;
using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System.Drawing;
using Zhzt.Exam.Document.DomainInterface;
using Zhzt.Exam.Document.DomainModel;

namespace Zhzt.Exam.Document.DomainService
{
    public class DocumentService : BaseService, IDocumentService
    {
        public DocumentService(ISqlSugarClient client) : base(client)
        {
        }

        public void AttachCategory(FileDocument doc)
        {
            if (doc != null)
            {
                var cached = _client?.DataCache.Get<DocumentCategory>(doc.CategoryId.ToString());
                if (cached != null)
                {
                    doc.DocCategory = cached;
                }
                else
                {
                    var cate = _client?.Queryable<DocumentCategory>().Where(x => x.Id == doc.CategoryId).Single();
                    var cateChilds = _client?.Queryable<DocumentCategory>().ToTree(it => it.Child, it => it.ParentId, doc.CategoryId);
                    if (cate != null)
                    {
                        cate.Child = cateChilds ?? new();
                        _client?.DataCache.Add(doc.CategoryId.ToString(), cate);

                    }
                    doc.DocCategory = cate;
                }
            }
        }

        public void AttachCategory(IEnumerable<FileDocument> docs)
        {
            if (docs != null)
            {
                foreach (var doc in docs)
                {
                    AttachCategory(doc);
                }
            }
        }

        public void GenPdf(string input, string output)
        {
            var ext = Path.GetExtension(input);
            switch (ext)
            {
                case ".doc":
                case ".docx":
                    // Word文档目前可以正常转换为pdf 但是最好还是直接上传pdf
                    Aspose.Words.LoadOptions opt = new Aspose.Words.LoadOptions();
                    FontSettings fst = FontSettings.DefaultInstance;
                    fst.SetFontsFolder("fonts", false);
                    opt.FontSettings = fst;
                    Aspose.Words.Document document = new Aspose.Words.Document(input, opt);
                    document.Save(output, Aspose.Words.SaveFormat.Pdf);
                    break;
                case ".ppt":
                case ".pptx":
                    /* ppt暂不制止，除非破解
                    Aspose.Slides.LoadOptions optSl = new Aspose.Slides.LoadOptions();
                    FontSettings fstSl = FontSettings.DefaultInstance;
                    fstSl.SetFontsFolder("fonts", false);
                    Aspose.Slides.Presentation presentation = new Aspose.Slides.Presentation(input, optSl);
                    presentation.Save(output, Aspose.Slides.Export.SaveFormat.Pdf);
                    */
                    break;
                case ".xls":
                case ".xlsx":
                    // xls文件转pdf没有问题 但是存在水印
                    Aspose.Cells.LoadOptions optCl = new Aspose.Cells.LoadOptions();
                    FontSettings fstCl = FontSettings.DefaultInstance;
                    fstCl.SetFontsFolder("fonts", false);
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(input);
                    workbook.Save(output, Aspose.Cells.SaveFormat.Pdf);
                    break;
                case ".pdf":
                    break;
                default:
                    break;
            }
        }

        public void GenThumb(string input, string output)
        {
            Spire.Pdf.PdfDocument document = new Spire.Pdf.PdfDocument(input);
            Image img = document.SaveAsImage(1, Spire.Pdf.Graphics.PdfImageType.Bitmap);
            img.Save(output);
        }

        #region 开发测试功能
        /// <summary>
        /// 创建测试数据
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateSeedData()
        {
            if (_client?.Queryable<FileDocument>().Count() == 0)
            {
                _client?.Ado.BeginTran();
                var cates = _client?.Queryable<DocumentCategory>().ToList();
                if (cates != null)
                {
                    foreach (var cat in cates)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            FileDocument fd = new FileDocument
                            {
                                CategoryId = cat.Id,
                                Name = $"{cat.Name}-{i}",
                                DocUrl = $"{i % 5 + 1}.pdf",
                                Thumb = $"{i % 5 + 1}.png",
                                PdfUrl = $"{i % 5 + 1}.pdf",
                            };
                            Save(fd);
                        }
                    }
                }
                _client?.Ado.CommitTran();
            }
        }
        #endregion
    }
}