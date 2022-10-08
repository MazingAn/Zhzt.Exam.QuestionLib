namespace Zhzt.Exam.Auth.DomainDtoModel
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string RePassword { get; set; } = null!;

        public long DepartmentId { get; set; }

        public string RealName { get; set; } = null!;

        public bool Validate()
        {
            return RePassword.Equals(Password);
        }
    }
}