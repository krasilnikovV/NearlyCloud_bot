using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NearlyCloud_bot
{
    public class TgUser
    {

        [Key]
        public int id { get; set; }
        public long userId;
        public string username;
        public string firstname;
        public string lastname;
    }

    
}
