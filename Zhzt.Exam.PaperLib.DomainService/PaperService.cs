using MicroService.NacosDiscover.Helper;
using Microsoft.Extensions.Options;
using MongoDb.Extensions.DomainHelper;
using MongoDb.Extensions.Options;
using Netcore.Extensions.WebModels;
using System.Net;
using Zhzt.Exam.PaperLib.DomainInterface;
using Zhzt.Exam.PaperLib.DomainModel;

namespace Zhzt.Exam.PaperLib.DomainService
{
    public class PaperService : BaseService<DocPaper>, IPaperService
    {
        private readonly NacosServiceDiscover _discover;

        public PaperService(IOptions<MongoDbDataSettings> databaseSettings, NacosServiceDiscover discover) : base(databaseSettings)
        {
            _discover = discover;
        }

        // 生成试卷
        public DocPaper GenerateQuestions(DocPaper docPaper)
        {
            if (docPaper.PagerConfig != null)
            {
                List<Task> tasks = new List<Task>();
                if (docPaper.PagerConfig.SingleChoiceCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.SingleChoiceQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.SingleChoiceCount, 1);
                    }));
                }
                if (docPaper.PagerConfig.MultiChoiceCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.MultiChoiceQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.MultiChoiceCount, 2);
                    }));
                }
                if (docPaper.PagerConfig.JudgeCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.JudgeQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.JudgeCount, 3);
                    }));
                }
                if (docPaper.PagerConfig.BlankFillCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.BlankFillQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.BlankFillCount, 4);
                    }));
                }
                if (docPaper.PagerConfig.QuesAnswerCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.QuesAnswerQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.QuesAnswerCount, 5);
                    }));
                }
                if (docPaper.PagerConfig.NounParsingCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.NounParsingQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.QuesAnswerCount, 6);
                    }));
                }
                if (docPaper.PagerConfig.ComputeCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.ComputeQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.QuesAnswerCount, 7);
                    }));
                }
                if (docPaper.PagerConfig.EssayCount > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        docPaper.EssayQuestions = await LoadRandomQuestionsAsync(docPaper.Subject.SubjectId, docPaper.PagerConfig.QuesAnswerCount, 8);
                    }));
                }
                if (tasks.Count > 0)
                {
                    Task.WaitAll(tasks.ToArray());
                }
            }
            return docPaper;
        }

        // 远程调用获取试题
        private async Task<IEnumerable<InnerDocPaperQuestion>> LoadRandomQuestionsAsync(string subjectId, int count, int quesCls)
        {
            string requestUrl = $"/api/questionlib/question/random?QuestionTypeId={subjectId}&QuestionClass={quesCls}&pageSize={count}";
            string realUrl = _discover.GetRealUrl(requestUrl, "QuestionLib");
            HttpClient client = new HttpClient();
            var retStr = await client.GetStringAsync(realUrl);
            HttpJsonResponse res = Newtonsoft.Json.JsonConvert.DeserializeObject<HttpJsonResponse>(retStr) ?? null!;
            if (res!=null && res.Success)
            {
                List<InnerDocPaperQuestion> questions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InnerDocPaperQuestion>>(res.Data!.ToString()!)!;
                return questions;
            }
            else
            {
                throw new Exception(res?.Msg);
            }
        }

        //更新试题
        public DocPaper UpdatePaper(DocPaper paper)
        {
            if (paper.ReGenerateQuestions)
            {
                var newPaper = GenerateQuestions(paper);
                return Update(newPaper);
            }
            else
            {
                var oldPaper = GetOneById(paper.Id);
                if (oldPaper != null)
                {
                    paper.SingleChoiceQuestions = oldPaper.SingleChoiceQuestions;
                    paper.MultiChoiceQuestions = oldPaper.MultiChoiceQuestions;
                    paper.JudgeQuestions = oldPaper.JudgeQuestions;
                    paper.BlankFillQuestions = oldPaper.BlankFillQuestions;
                    paper.QuesAnswerQuestions = oldPaper.QuesAnswerQuestions;
                    return Update(paper);
                }
                else
                {
                    throw new Exception("没有找到对应的数据，无法执行更新操作");
                }
            }
        }
    }

}