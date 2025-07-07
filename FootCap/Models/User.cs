namespace FootCap.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

    public class User: IdentityUser
{

    public string FullName { get; set; }
    public string Address { get; set; }


    public Cart Cart { get; set; }
    public ICollection<Order> Orders { get; set; }
}

