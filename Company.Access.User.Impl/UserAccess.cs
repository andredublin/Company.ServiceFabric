﻿using Company.Access.User.Interface;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Company.Access.User.Impl
{
    public class UserAccess
        : IUserAccess
    {
        private readonly ILogger _Logger;

        public UserAccess(ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CheckUserExistsAsync(string name)
        {
            _Logger.Information($@"{nameof(CheckUserExistsAsync)} Invoked");
            _Logger.Information($@"{nameof(CheckUserExistsAsync)} {name}");

            await Task.Delay(new Random().Next(100, 500)).ConfigureAwait(false);

            return await Task.FromResult(false).ConfigureAwait(false);
        }

        public async Task<string> CreateUserAsync(string name)
        {
            _Logger.Information($@"{nameof(CreateUserAsync)} Invoked");
            _Logger.Information($@"{nameof(CreateUserAsync)} {name}");


            throw new Exception();

            await Task.Delay(new Random().Next(100, 500)).ConfigureAwait(false);

            return await Task.FromResult($"\r\n        UserAccess.CreateUserAsync -> {name} -> {DateTime.UtcNow}").ConfigureAwait(false);
        }
    }
}
