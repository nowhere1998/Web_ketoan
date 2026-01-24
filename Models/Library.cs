using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Models
{
    public partial class Library
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên thư viện")]
        [StringLength(255, ErrorMessage = "Tên tối đa 255 ký tự")]
        public string? Name { get; set; }

        public string? Tag { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình ảnh")]
        public string? Image { get; set; }

        public string? File { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? Info { get; set; }

        [Range(0, 100, ErrorMessage = "Độ ưu tiên từ 0 đến 100")]
        public int? Priority { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public int? Active { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhóm thư viện")]
        public int? GroupLibraryId { get; set; }

        public int? MemberId { get; set; }

        [StringLength(10)]
        public string? Lang { get; set; }

        [ForeignKey("GroupLibraryId")]
        public virtual GroupLibrary? GroupLibrary { get; set; }
    }
}
