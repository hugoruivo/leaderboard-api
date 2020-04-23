using System;

namespace LeaderboardAPI.Models
{
    public class CustomApiResponse
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public Object Results { get; set; }

        public CustomApiResponse(bool err, string msg, Object res = null)
        {
            Error = err;
            Message = msg;
            Results = res;
        }
    }
}
