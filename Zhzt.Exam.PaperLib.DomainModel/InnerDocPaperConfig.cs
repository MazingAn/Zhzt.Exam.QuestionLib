using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.PaperLib.DomainModel
{
    public class InnerDocPagerConfig
    {
        // 试卷总分
        public float TotalScore { get; set; }

        // 单选数量 总分
        public int SingleChoiceCount { get; set; }
        public float SingleChoiceTotalScore { get; set; }

        // 多选数量 总分
        public int MultiChoiceCount { get; set; }
        public float MultiChoiceTotalScore { get; set; }

        // 判断数量 总分
        public int JudgeCount { get; set; }
        public float JudgeTotalScore { get; set; }

        // 填空数量 总分
        public int BlankFillCount { get; set; }
        public float BlankFillTotalScore { get; set; }

        // 问答数量 总分
        public int QuesAnswerCount { get; set; }
        public float QuesAnswereTotalScore { get; set; }


        // 名词解释数量 总分
        public int NounParsingCount { get; set; }
        public float NounParsingTotalScore { get; set; }


        // 论述数量 总分
        public int EssayCount { get; set; }
        public float EssayTotalScore { get; set; }


        // 计算数量 总分
        public int ComputeCount { get; set; }
        public float ComputeTotalScore { get; set; }
    }
}
