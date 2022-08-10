using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcore.Extensions.WebModels
{
    /// <summary>
    /// 标准的httpJson响应
    /// </summary>
    public class HttpJsonResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Msg { get; set; } = string.Empty;
        public object? Data { get; set; }

        public static HttpJsonResponse SuccessResult(object? data, string msg="success")
        {
            return new HttpJsonResponse()
            {
                Success = true,
                Code = 0,
                Msg = msg,
                Data = data
            };
        }

        public static HttpJsonResponse FailedResult(string msg = "error")
        {
            return new HttpJsonResponse()
            {
                Success = false,
                Code = -1,
                Msg = msg
            };
        }


    }
}
