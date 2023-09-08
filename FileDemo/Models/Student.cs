using System.Net.Mail;

namespace FileDemo.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
        public Student()
        {
            Attachments = new List<Attachment>();
        }
    }
}
