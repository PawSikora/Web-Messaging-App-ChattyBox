namespace DAL.Database.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserChat>? UserChats { get; set; }
    }
}
