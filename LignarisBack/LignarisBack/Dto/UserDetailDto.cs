﻿using Lignaris.DTO;

namespace LignarisBack.Dto
{
    public class UserDetailDto
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string[]? Roles { get; set; }
        public PersonaDTO? Persona { get; set; }
    }
}
