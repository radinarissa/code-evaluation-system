using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface IMoodleAuthService
    {
        Task<MoodleAuthResult> AuthenticateAsync(string username, string password);
    }
}
