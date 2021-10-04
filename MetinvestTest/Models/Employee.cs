using Foolproof;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace MetinvestTest.Models
{
    public class Employee
    {
        [Key]
        [BindingBehavior(BindingBehavior.Optional)]
        public int Id { get; set; }
        [MaxLength(128)]
        [RequiredIfTrue(nameof(Staffed), ErrorMessage = "Поле должно быть заполнено для штатного сотрудника")]
        [Display(Name = "Табельный номер")]
        public string PersonnelNumber { get; set; }
        [MaxLength(256)]
        [Display(Name = "ФИО")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Пол")]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Sex Sex { get; set; }
        [Display(Name = "Штатный сотрудник")]
        [JsFormatter("boolFormatter")]
        public bool? Staffed { get; set; } = false;
    }
    public enum Sex
    {
        Женский = 0,
        Мужской = 1
    }
}