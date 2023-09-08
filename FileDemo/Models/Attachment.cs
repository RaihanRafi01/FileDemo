using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FileDemo.Models
{
    public class Attachment
    {
        [Key]
        public int id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string FileName { set; get; }
        [Column(TypeName = "nvarchar(50)")]
        public string Description { set; get; }
        [Column(TypeName = "varbinary(MAX)")]
        public byte[] attachment { set; get; }

        [ForeignKey("Student")]
        [Required]
        public int SId { get; set; }
        public virtual Student Student { get; set; }
    }
}
