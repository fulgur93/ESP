using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ESP.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ForumUser class
public class ForumUser : IdentityUser
{
    [DisplayName("Imie")]
    public string FirstName { get; set; }
    [DisplayName("Nazwisko")]
    public string? LastName { get; set; }

}

