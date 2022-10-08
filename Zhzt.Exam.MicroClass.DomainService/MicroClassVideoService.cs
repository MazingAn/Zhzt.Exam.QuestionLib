using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using Xabe.FFmpeg;
using Zhzt.Exam.MicroClass.DomainInterface;
using Zhzt.Exam.MicroClass.DomainModel;

namespace Zhzt.Exam.MicroClass.DomainService
{
    public class MicroClassVideoService : BaseService, IMicroClassVideoService
    {
        public MicroClassVideoService(ISqlSugarClient client) : base(client)
        {

        }

        /// <summary>
        /// 附加分类信息
        /// </summary>
        /// <param name="microClassVideos"></param>
        public void AttachVideoCategory(IEnumerable<MicroClassVideo>? microClassVideos)
        {
            if (microClassVideos != null)
            {
                foreach (var item in microClassVideos)
                {
                    AttachVideoCategory(item);
                }
            }
        }

        /// <summary>
        /// 附加分类信息
        /// </summary>
        /// <param name="microClassVideos"></param>
        public void AttachVideoCategory(MicroClassVideo? microClassVideo)
        {
            if (microClassVideo != null)
            {
                var cachedVideoCategory = _client?.DataCache.Get<VideoCategory>(microClassVideo.VideoCategoryId.ToString());
                if (cachedVideoCategory != null)
                {
                    microClassVideo.Category = cachedVideoCategory;
                }
                else
                {
                    var videoCategory = _client?.Queryable<VideoCategory>().Where(x => x.Id == microClassVideo.VideoCategoryId).Single();
                    var microClassVideoTypeChilds = _client?.Queryable<VideoCategory>().ToTree(it => it.Child, it => it.ParentId, microClassVideo.VideoCategoryId);
                    if (videoCategory != null)
                    {
                        videoCategory.Child = microClassVideoTypeChilds ?? new();
                        _client?.DataCache.Add(microClassVideo.VideoCategoryId.ToString(), videoCategory);

                    }
                    microClassVideo.Category = videoCategory;
                }
            }
        }

        /// <summary>
        /// 获取视频截图
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public async void GenThumbAsync(string input, string  output)
        {
            IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(input, output, TimeSpan.FromSeconds(12));
            await conversion.Start();
        }

        #region 开发测试功能
        /// <summary>
        /// 创建测试数据
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateSeedData()
        {
            if(_client?.Queryable<MicroClassVideo>().Count() == 0)
            {
                _client?.Ado.BeginTran();
                var cates = _client?.Queryable<VideoCategory>().ToList();
                if (cates!=null)
                {
                    foreach (var cat in cates)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            MicroClassVideo mcv = new MicroClassVideo
                            {
                                VideoCategoryId = cat.Id,
                                Title = $"{cat.Name}-{i}",
                                VideoUrl = $"{i % 5 + 1}.mp4",
                                Thumb = $"{i % 5 + 1}.png",
                                Description = $"{cat.Name}-{i}的描述信息",
                            };
                            Save(mcv);
                        }
                    }
                }
                _client?.Ado.CommitTran();
            }
        }
        #endregion
    }
}