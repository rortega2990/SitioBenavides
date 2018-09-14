using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BenavidesFarm.DataModels.Models
{
    public class MyApplicationUser : IdentityUser<int, MyApplicationUserLogin, MyApplicationUserRole, MyApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<MyApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string CardUser { get; set; }

        [Required]
        public string TypeUser { get; set; }

        public virtual UserProfileInfo UserProfileInfo { get; set; }
    }

    public class MyApplicationUserLogin : IdentityUserLogin<int> { }

    public class MyApplicationUserClaim : IdentityUserClaim<int> { }

    public class MyApplicationUserRole : IdentityUserRole<int> { }

    public class MyApplicationRole : IdentityRole<int, MyApplicationUserRole>
    {
        public string Description { get; set; }

        public MyApplicationRole() { }

        public MyApplicationRole(string name)
            : this()
        {
            Name = name;
        }

        public MyApplicationRole(string name, string description)
            : this(name)
        {
            Description = description;
        }
    }

    public class MyApplicationUserStore : UserStore<MyApplicationUser, MyApplicationRole, int, MyApplicationUserLogin, MyApplicationUserRole, MyApplicationUserClaim>
    {
        private readonly DbContext context;
        public MyApplicationUserStore()
            : this(new MyApplicationDbContext())
        {
            DisposeContext = true;
        }

        public MyApplicationUserStore(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<MyApplicationUser> FindByCardAsync(string card)
        {
            var c = context as MyApplicationDbContext;
            if (c != null)
            {
                return await c.Users.FirstOrDefaultAsync(x => x.CardUser == card);
            }
            return null;
        }

        public MyApplicationUser FindByCard(string card)
        {
            var c = context as MyApplicationDbContext;
            if (c != null)
            {
                return c.Users.FirstOrDefault(x => x.CardUser == card);
            }
            return null;
        }
    }

    public class MyApplicationRoleStore : RoleStore<MyApplicationRole, int, MyApplicationUserRole>
    {
        public MyApplicationRoleStore()
            : base(new MyApplicationDbContext())
        {
            DisposeContext = true;
        }

        public MyApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }

    /// <summary>
    /// Clase que contiene los datos delperfil de usuario, 
    /// Incluida en la clase MyApplicationUser que extiende IdentityUser 
    /// </summary>
    public class UserProfileInfo
    {
        public int UserProfileInfoId { get; set; }

        [Required(ErrorMessage = "El campo Nombre(s) es requerido")]
        public string UserNames { get; set; }

        [Required(ErrorMessage = "El campo Apellido paterno es requerido")]
        public string UserLastName1 { get; set; }

        [Required(ErrorMessage = "El campo Apellido materno es requerido")]
        public string UserLastName2 { get; set; }

        [Required(ErrorMessage = "El campo Fecha de nacimiento es requerido")]
        [DataType(DataType.DateTime)]
        public DateTime? UserBirthDate { get; set; }

        [Required(ErrorMessage = "El campo Género es requerido")]
        public bool UserFemale { get; set; }

        [Required(ErrorMessage = "El campo ¿Tienes hijos? es requerido")]
        public bool UserHasChildren { get; set; }

        public bool? UserClubPeques { get; set; }

        public string UserImagePerfil { get; set; }

        public float UserMount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UserCreationDateClubPeques { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UserCreationDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UserUpdateDate { get; set; }
        public string UserCodePostal { get; set; }
        public string UserCity { get; set; }
    }
}