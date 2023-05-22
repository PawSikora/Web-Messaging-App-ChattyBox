namespace MVCWebAppWithoutBLL.Models.UserDtos
{
    public class UserDTO
    {
        public int Id { get; set; }
        
        public string Email { get; set; }

        public string Username { get; set; }

        public DateTime? LastLog { get; set; }


        public DateTime Created { get; set; }

    }
}
