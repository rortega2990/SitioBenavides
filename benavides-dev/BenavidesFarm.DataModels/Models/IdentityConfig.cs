using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace BenavidesFarm.DataModels.Models
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    // *** PASS IN TYPE ARGUMENT TO BASE CLASS:
    public class MyApplicationUserManager : UserManager<MyApplicationUser, int>
    {
        private readonly IUserStore<MyApplicationUser, int> store;

        // *** ADD INT TYPE ARGUMENT TO CONSTRUCTOR CALL:
        public MyApplicationUserManager(IUserStore<MyApplicationUser, int> store)
            : base(store)
        {
            this.store = store;
        }

        public virtual async Task<IdentityResult> UpdatePassword(MyApplicationUser user, string newPassword)
        {
            var passwordStore = Store as IUserPasswordStore<MyApplicationUser, int>;

            if (passwordStore == null)
                throw new Exception("UserManager store does not implement IUserPasswordStore");

            var result = await base.UpdatePassword(passwordStore, user, newPassword);

            if (result.Succeeded)
                result = await base.UpdateAsync(user);

            return result;
        }

        public static MyApplicationUserManager Create(IdentityFactoryOptions<MyApplicationUserManager> options, IOwinContext context)
        {
            // *** PASS CUSTOM APPLICATION USER STORE AS CONSTRUCTOR ARGUMENT:
            var manager = new MyApplicationUserManager(new MyApplicationUserStore(context.Get<MyApplicationDbContext>()));

            // Configure validation logic for usernames

            // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
            manager.UserValidator = new CustomUserValidator<MyApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false,
                RequireUniqueCard = true,
                RequireUniqueUsername = false
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. 
            // This application uses Phone and Emails as a step of receiving a 
            // code for verifying the user You can write your own provider and plug in here.

            // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
            manager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<MyApplicationUser, int>
                {
                    MessageFormat = "Your security code is: {0}"
                });

            // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<MyApplicationUser, int>
                {
                    Subject = "SecurityCode",
                    BodyFormat = "Your security code is {0}"
                });

            manager.EmailService = new MyEmailService();
            manager.SmsService = new MySmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
                manager.UserTokenProvider = new DataProtectorTokenProvider<MyApplicationUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public async Task<MyApplicationUser> FindByCardAsync(string card)
        {
            var s = store as MyApplicationUserStore;
            return await s?.FindByCardAsync(card);
        }

        public MyApplicationUser FindByCard(string card)
        {
            var s = store as MyApplicationUserStore;
            return s?.FindByCard(card);
        }
    }

    // PASS CUSTOM APPLICATION ROLE AND INT AS TYPE ARGUMENTS TO BASE:
    public class MyApplicationRoleManager : RoleManager<MyApplicationRole, int>
    {
        // PASS CUSTOM APPLICATION ROLE AND INT AS TYPE ARGUMENTS TO CONSTRUCTOR:
        public MyApplicationRoleManager(IRoleStore<MyApplicationRole, int> roleStore)
            : base(roleStore)
        {
        }

        // PASS CUSTOM APPLICATION ROLE AS TYPE ARGUMENT:
        public static MyApplicationRoleManager Create(IdentityFactoryOptions<MyApplicationRoleManager> options, IOwinContext context)
        {
            return new MyApplicationRoleManager(new MyApplicationRoleStore(context.Get<MyApplicationDbContext>()));
        }
    }

    public class MyEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class MySmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class MyApplicationSignInManager : SignInManager<MyApplicationUser, int>
    {
        readonly MyApplicationUserManager manager;

        public MyApplicationSignInManager(MyApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) {
            manager = userManager;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(MyApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((MyApplicationUserManager)UserManager);
        }

        public static MyApplicationSignInManager Create(IdentityFactoryOptions<MyApplicationSignInManager> options, IOwinContext context)
        {
            return new MyApplicationSignInManager(context.GetUserManager<MyApplicationUserManager>(), context.Authentication);
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string cardUsername, string password, bool isPersistent, bool shouldLockout)
        {
            var user = await manager.FindByCardAsync(cardUsername);
            if(user != null)
            {
                return await base.PasswordSignInAsync(user.UserName, password, isPersistent, shouldLockout);
            }

            return await base.PasswordSignInAsync(cardUsername, password, isPersistent, shouldLockout);
        }
    }

    public class CustomUserValidator<TUser> : UserValidator<TUser, int> where TUser : MyApplicationUser
    {
        private UserManager<TUser, int> Manager { get; }

        public bool RequireUniqueCard { get; set; }

        public bool RequireUniqueUsername { get; set; }

        public CustomUserValidator(UserManager<TUser, int> manager) : base(manager)
        {
            Manager = manager;
        }

        //private static readonly Regex EmailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public override async Task<IdentityResult> ValidateAsync(TUser item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var errors = new List<string>();
            if (RequireUniqueUsername)
            {
                await ValidateUserName(item, errors);
            }
            if (RequireUniqueEmail)
            {
                await ValidateEmail(item, errors);
            }
            //if (RequireUniqueCard)
            //{
            //    await ValidateCard(item, errors);
            //}
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private async Task ValidateUserName(TUser user, ICollection<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add("El campo Nombre de usuario es requerido");
            }
            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, @"^[A-Za-z0-9@_\.]+$"))
            {
                // If any characters are not letters or digits, its an illegal user name
                errors.Add($"Formato de nombre de usuario incorrecto: {user.UserName}");
            }
            else
            {
                var owner = await Manager.FindByNameAsync(user.UserName);
                if (owner != null && !EqualityComparer<int>.Default.Equals(owner.Id, user.Id))
                {
                    errors.Add($"Ya existe un nombre de usuario registrado con ese valor: {user.UserName}");
                }
            }
        }

        // make sure email is not empty, valid, and unique
        private async Task ValidateEmail(TUser user, ICollection<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                errors.Add("El campo Email es requerido");
                return;
            }
            try
            {
                var m = new MailAddress(user.Email);
            }
            catch (FormatException)
            {
                errors.Add($"Formato de correo electrónico incorrecto: {user.Email}");
                return;
            }
            var owner = await Manager.FindByEmailAsync(user.Email);
            if (owner != null && !EqualityComparer<int>.Default.Equals(owner.Id, user.Id))
            {
                errors.Add($"Ya existe un correo electrónico registrado con ese valor: {user.Email}");
            }
        }

        private async Task ValidateCard(TUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.CardUser))
            {
                errors.Add("El campo Tarjeta es requerido");
                return;
            }
            var m = Manager as MyApplicationUserManager;
            if (m != null)
            {
                var owner = await m.FindByCardAsync(user.CardUser);
                if (owner != null && !EqualityComparer<int>.Default.Equals(owner.Id, user.Id))
                {
                    errors.Add($"Ya existe una Tarjeta registrada con ese número: {user.CardUser}");
                }
            }
        }
    }
}
