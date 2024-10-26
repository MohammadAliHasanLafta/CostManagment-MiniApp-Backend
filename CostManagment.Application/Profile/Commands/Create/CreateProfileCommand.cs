﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CostManagment.Application.Profile.Commands.Create;

public class CreateProfileCommand : IRequest<long>
{
    public string UserName { get; set; } = "نام کاربر";
    [EmailAddress]
    public string Email { get; set; } = "example@gmail.com";
    public long UserId { get; set; }
    public string? PhoneNumber { get; set; }
}
