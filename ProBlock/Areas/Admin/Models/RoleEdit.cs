﻿using Microsoft.AspNetCore.Identity;

namespace ProBlock.Areas.Admin.Models;

public class RoleEdit
{
    public IdentityRole? Role { get; set; }
    public IEnumerable<IdentityUser> Members { get; set; }
    public IEnumerable<IdentityUser> NonMembers { get; set; }
}
