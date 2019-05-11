using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DrDone.ViewModels
{
    public class AuthLogin
    {
        [MinLength(5, ErrorMessage = "Kullanıcı adı en fazla 5 karakter olabilir")]
        [MaxLength(10,ErrorMessage ="Kullanıcı adı en az 10 karakter olabilir")]
        [Required(ErrorMessage = "Kullancı adı boş bırakılamaz")]
        public string Username { get; set; }

        [MinLength(5, ErrorMessage = "Şifre en fazla 5 karakter olabilir")]
        [MaxLength(10, ErrorMessage = "Şifre en az 10 karakter olabilir")]
        [Required(ErrorMessage = "Şifre boş bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}