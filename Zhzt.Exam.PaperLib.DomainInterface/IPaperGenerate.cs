using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhzt.Exam.PaperLib.DomainModel;

namespace Zhzt.Exam.PaperLib.DomainInterface
{
    public interface IPaperGenerate
    {
        public string GeneratePaper(DocPaper paper);

    }
}
