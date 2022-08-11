using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.PaperLib.DomainModel
{
    public class InnerDocPaperQuestion
    {
        public string QuestionBody { get; set; } = null!;

        public string Answer1 { get; set; } = null!;

        public string Answer2 { get; set; } = null!;

        public string Answer3 { get; set; } = null!;

        public string Answer4 { get; set; } = null!;

        public string Answer5 { get; set; } = null!;

        public string Answer6 { get; set; } = null!;

        public string RightAnswer { get; set; } = null!;

        public int QuestionClass { get; set; }
    }
}
