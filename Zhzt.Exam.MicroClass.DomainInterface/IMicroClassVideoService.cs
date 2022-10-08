using SqlSugar.Extension.DomainHelper;
using Zhzt.Exam.MicroClass.DomainModel;

namespace Zhzt.Exam.MicroClass.DomainInterface
{
    public interface IMicroClassVideoService : IBaseService
    {
        void AttachVideoCategory(IEnumerable<MicroClassVideo>? microClassVideos);
        void AttachVideoCategory(MicroClassVideo? microClassVideo);
        void GenThumbAsync(string input, string output);

        #region 开发选项
        void CreateSeedData();
        #endregion
    }
}