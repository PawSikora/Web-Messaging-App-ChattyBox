namespace DAL.Database.Entities
{
    public abstract class Message
    {
        public int Id { get; set; }
        
        public DateTime TimeStamp { get; set; }

        public int SenderId { get; set; }
        
        public virtual User Sender { get; set; }

        public int ChatId { get; set; }
        
        public virtual Chat Chat { get; set; }
    }
}
