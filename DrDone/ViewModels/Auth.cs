using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DrDone.ViewModels
{
    public class AuthLogin
    {
        public virtual bool isSignup { get; set; }

        [MinLength(4, ErrorMessage = "Kullanıcı adı en az 4 karakter olabilir")]
        [MaxLength(10,ErrorMessage ="Kullanıcı adı en fazla 10 karakter olabilir")]
        [Required(ErrorMessage = "Kullancı adı boş bırakılamaz")]
        public string Username { get; set; }

        [MinLength(5, ErrorMessage = "Şifre en fazla 5 karakter olabilir")]
        [MaxLength(10, ErrorMessage = "Şifre en az 10 karakter olabilir")]
        [Required(ErrorMessage = "Şifre boş bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class Signup{
        [Required(ErrorMessage ="Kullanıcı Adı boş bırakılamaz"),MinLength(4), MaxLength(128)]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Required, MaxLength(128), DataType(DataType.EmailAddress)]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Telefon numarası")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
    }
}