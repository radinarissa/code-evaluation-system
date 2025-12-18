namespace CodeEvaluator.Application.DTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public UserDto? User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
