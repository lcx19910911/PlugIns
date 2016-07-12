namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ���� ����Ʒ ��Ʒ���ࣩ
    /// </summary>
    [Table("Category")]
    public partial class Category
    {
        /// <summary>
        /// ����
        /// </summary>
        [Key]      
        public string UNID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        [Display(Name = "������")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        [Display(Name = "����id")]
        public string ShopId { get; set; }
        /// <summary>
        /// �û�id
        /// </summary>
        [Display(Name = "�û�id")]
        public string PersonId { get; set; }
        /// <summary>
        /// ͼ��
        /// </summary>
        [Display(Name = "ͼ��")]
        [MaxLength(500)]
        public string Image { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [Display(Name = "����")]
        public int Sort { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Display(Name = "����ʱ��")]
        public System.DateTime CreatedTime { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        [Display(Name= "�޸�ʱ��")]
        public System.DateTime UpdatedTime { get; set; }
    }
}
